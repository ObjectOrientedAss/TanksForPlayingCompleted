using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public SpawnPoint[] SpawnPoints; //set in inspector

    public static Dictionary<ulong, Player> Players { get; private set; }
    public static Player MySelf { get; private set; }
    public static bool IsPaused;

    private void Awake()
    {
        gameObject.AddComponent<InteractablesManager>();
        gameObject.AddComponent<BulletsManager>();
        gameObject.AddComponent<StatsManager>();

        NW_EventSystem.OnHostChangedEvent += OnHostChanged;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        P2PPacketReader.OnSpawnTankPacketReceivedEvent += SpawnTank;
        GP_EventSystem.OnRespawnEvent += OnRespawn;
        GP_EventSystem.OnGameWinnerConfirmedEvent += OnGameWinnerConfirmed;

        Players = new Dictionary<ulong, Player>();
        foreach (Friend player in NetworkManager.CurrentLobby.Members)
        {
            if (player.Id == SteamClient.SteamId) //don't add me in this dictionary
                continue;
            Players.Add(player.Id, null);
        }

        NetworkManager.CurrentLobby.SetMemberData("Damage", "0");
        NetworkManager.CurrentLobby.SetMemberData("Kills", "0");
        NetworkManager.CurrentLobby.SetMemberData("Deaths", "0");
        NetworkManager.CurrentLobby.SetMemberData("Score", "0");

        NetworkManager.CurrentLobby.SetMemberData("TotalDamage", "0");
        NetworkManager.CurrentLobby.SetMemberData("TotalKills", "0");
        NetworkManager.CurrentLobby.SetMemberData("TotalDeaths", "0");
    }

    private void OnDestroy()
    {
        NW_EventSystem.OnHostChangedEvent -= OnHostChanged;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
        P2PPacketReader.OnSpawnTankPacketReceivedEvent -= SpawnTank;
        GP_EventSystem.OnRespawnEvent -= OnRespawn;
        GP_EventSystem.OnGameWinnerConfirmedEvent += OnGameWinnerConfirmed;
        //safety static reset
        Players.Clear();
        MySelf = null;
        IsPaused = false;
    }

    private void OnHostChanged(ulong formerHostID, ulong newHostID)
    {
        //if the host has left, and i am the new host, take care of the spawn crate procedure.
        if (newHostID == SteamClient.SteamId)
            StartCoroutine(CrateSpawner());
    }

    private void OnGameWinnerConfirmed(bool winner)
    {
        if (winner) //if there is a winner
        {
            //deactivate all tanks to avoid transform packets funny jokes
            MySelf.gameObject.SetActive(false);
            foreach (Player player in Players.Values)
            {
                player.gameObject.SetActive(false);
            }

            UI_EventSystem.StartLoadingAsync(2);
        }
        else //if there is not a single winner or a winner at all
        {
            WaitHandler.StartWaiting(() =>
            {
                foreach (Player player in Players.Values)
                {
                    SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteSingleOperation(Operation.PrepareRound));
                }
                UI_EventSystem.StopLoading();
                GP_EventSystem.PrepareRound();
            });

            if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId)) //AND i'm the host
            {
                //pick a spawn point for each player
                LoadingScreenBehaviour.LoadingAction = () =>
                {
                    //Spawn Players in random locations:
                    List<SpawnPoint> spots = new List<SpawnPoint>();
                    for (int i = 0; i < SpawnPoints.Length; i++)
                    {
                        spots.Add(SpawnPoints[i]);
                    }

                    //tell to each player his spawn point
                    foreach (Player player in Players.Values)
                    {
                        int randomSpot = Random.Range(0, spots.Count);
                        SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteResetRound(spots[randomSpot]));
                        spots.RemoveAt(randomSpot);
                    }

                    //do the same for me (the host)
                    int mySpot = Random.Range(0, spots.Count);
                    MySelf.transform.position = spots[mySpot].transform.position;
                    MySelf.transform.rotation = spots[mySpot].transform.rotation;
                    WaitHandler.PlayerReady();
                };
                UI_EventSystem.StartLoading();
            }
        }
    }

    private void OnLobbyMemberLeave(Lobby lobby, Friend user)
    {
        StartCoroutine(DestroyPlayer(user));
        if (NetworkManager.CurrentLobby.MemberCount == 1)
        {
            NW_EventSystem.LeaveLobby();
            UI_EventSystem.StartLoadingAsync(0);
        }
    }

    private IEnumerator DestroyPlayer(Friend player)
    {
        Player p = Players[player.Id];
        p.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(5);
        Players.Remove(p.SteamData.Id);
        Destroy(p.gameObject);
    }

    public void InitializeGame()
    {
        //only if i am the host
        if (NetworkManager.CurrentLobby.IsOwnedBy(SteamClient.SteamId))
        {
            //Spawn Players in random locations:
            List<SpawnPoint> spots = new List<SpawnPoint>();
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                spots.Add(SpawnPoints[i]);
            }

            foreach (Friend player in NetworkManager.CurrentLobby.Members)
            {
                int randomSpot = Random.Range(0, spots.Count);
                SpawnTank(spots[randomSpot].Index, player.Id);

                foreach (Friend other in NetworkManager.CurrentLobby.Members)
                {
                    if (other.Id != SteamClient.SteamId) //don't send it to myself!
                        SteamNetworking.SendP2PPacket(other.Id, P2PPacketWriter.WriteSpawnTank(spots[randomSpot].Index, player.Id));
                }

                spots.RemoveAt(randomSpot);
            }

            foreach (Player player in Players.Values) //send a Prepare Round packet to all the players
            {
                SteamNetworking.SendP2PPacket(player.SteamData.Id, P2PPacketWriter.WriteSingleOperation(Operation.PrepareRound));
            }

            UI_EventSystem.StopLoading(); //remove the loading screen
            GP_EventSystem.PrepareRound();

            StartCoroutine(CrateSpawner());
        }
    }

    private void SpawnTank(int spawnPointIndex, ulong playerID)
    {
        GameObject prefab;
        if (playerID != SteamClient.SteamId) //spawning the tank of another player...
            prefab = Resources.Load<GameObject>("Game/OtherTank");
        else //spawning my tank
            prefab = Resources.Load<GameObject>("Game/MySelfTank");

        GameObject tank = Instantiate(prefab, SpawnPoints[spawnPointIndex].transform.position, SpawnPoints[spawnPointIndex].transform.rotation);
        Player p = tank.GetComponent<Player>();

        MeshRenderer[] meshRenderers = tank.GetComponentsInChildren<MeshRenderer>();

        foreach (Friend player in NetworkManager.CurrentLobby.Members)
        {
            if (player.Id == playerID)
            {
                p.SteamData = player;
                break;
            }
        }

        if (playerID != SteamClient.SteamId)
        {
            Players[playerID] = p;
            Players[playerID].LagCompensation.Init();
        }
        else
            MySelf = p;

        //set the tank color
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetFloat("_colorOffset", float.Parse(NetworkManager.CurrentLobby.GetMemberData(p.SteamData, "TankColor")));
        }

        Resources.UnloadUnusedAssets();
    }

    private void OnRespawn()
    {
        List<SpawnPoint> spots = new List<SpawnPoint>();
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            spots.Add(SpawnPoints[i]);
        }

        for (int i = 0; i < spots.Count; i++)
        {
            int randomSpot = Random.Range(0, spots.Count);
            if (spots[randomSpot].IsFree())
            {
                MySelf.Respawn(spots[randomSpot].transform);
                break;
            }
            else
                spots.RemoveAt(randomSpot);
        }
    }

    private IEnumerator CrateSpawner()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(12, 20));
            InteractablesManager.GenerateCrate();
        }
    }
}
