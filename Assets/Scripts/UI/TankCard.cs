using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TankCard : MonoBehaviour
{
    public Friend friend { get; private set; } //the user data of this card
    public TextMeshProUGUI PlayerName; //set in inspector
    public RawImage Avatar; //set in inspector
    public GameObject Tank; //set in inspector
    public GameObject KickButton; //set in inspector
    public GameObject ColorSlider; //set in inspector
    private bool rotateTank;

    private Coroutine confirmColor;

    public void Init(Friend player, bool kickable = false)
    {
        KickButton.SetActive(kickable);

        friend = player;
        PlayerName.text = player.Name;
        //setup the host card
        if(NetworkManager.CurrentLobby.IsOwnedBy(player.Id))
        {
            //highlight the card with a different color. He is a VIP after all :)
            Image image = GetComponent<Image>();
            float alpha = image.color.a;
            image.color = new Color(0, 0, 255, alpha);
        }

        _ = GetAvatar();
        SteamFriends.RequestUserInformation(friend.Id, false);

        if (friend.Id != SteamClient.SteamId)
        {
            StartCoroutine(UpdateTankColor());
            ColorSlider.SetActive(false);
        }

        NW_EventSystem.OnHostChangedEvent += OnHostChangedEvent;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
    }

    private void OnDestroy()
    {
        NW_EventSystem.OnHostChangedEvent -= OnHostChangedEvent;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
    }

    private void OnLobbyMemberLeave(Steamworks.Data.Lobby lobby, Friend user)
    {
        //the user who left the lobby was the owner of this tank card...
        if(user.Id == friend.Id)
            Destroy(gameObject);
    }

    private void OnHostChangedEvent(ulong formerHostID, ulong newHostID)
    {
        //this card belongs to the new host, make it blue.
        if(newHostID == friend.Id)
        {
            //highlight the card with a different color. He is a VIP after all :)
            Image image = GetComponent<Image>();
            float alpha = image.color.a;
            image.color = new Color(0, 0, 255, alpha);
        }
        else // this card belongs to a client
        {
            //if I AM the host, i will need kick button over this user
            if (newHostID == SteamClient.SteamId)
                KickButton.SetActive(true);
        }
    }

    async Task GetAvatar()
    {
        var img = await SteamFriends.GetLargeAvatarAsync(friend.Id);
        if (img.HasValue)
            Avatar.texture = MakeTexture(img.Value.Data, img.Value.Width, img.Value.Height);
    }

    public Texture2D MakeTexture(byte[] data, uint width, uint height)
    {
        Texture2D texture = null;
        texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
        texture.LoadRawTextureData(data);
        texture.Apply();

        return texture;
    }

    public void OnKickButtonClick()
    {
        //Kick functionalities exist, but apparently Steamworks does not provide a call to use them.
        //Search for "LobbyKicked_t" in ISteamMatchMaking in the official SteamWorks documentation.
        //So what we do here is sending a packet to the user, politely asking him to fuck off.
        //When the user gets that packet, he just force-leaves the lobby.
        SteamNetworking.SendP2PPacket(friend.Id, P2PPacketWriter.WriteSingleOperation(Operation.Kick));
    }

    public void OnRotateTankButtonClick()
    {
        rotateTank = !rotateTank;

        if (rotateTank)
            StartCoroutine(RotateTank());
        else
            StopAllCoroutines();
    }

    private IEnumerator RotateTank()
    {
        while(rotateTank)
        {
            Tank.transform.Rotate(Tank.transform.up, 20f * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator UpdateTankColor()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(2);
            MeshRenderer[] meshRenderers = Tank.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material.SetFloat("_colorOffset", float.Parse(NetworkManager.CurrentLobby.GetMemberData(friend, "TankColor")));
            }
        }
    }

    public void OnTankColorChanged(float value)
    {
        UpdateTankColor(value);
    }

    private void UpdateTankColor(float value)
    {
        MeshRenderer[] meshRenderers = Tank.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.SetFloat("_colorOffset", value);
        }

        if (confirmColor != null)
            StopCoroutine(confirmColor);
        confirmColor = StartCoroutine(ConfirmColor(value));
    }

    private IEnumerator ConfirmColor(float value)
    {
        yield return new WaitForSecondsRealtime(1);
        //send packet with color
        NetworkManager.CurrentLobby.SetMemberData("TankColor", value.ToString());
    }
}