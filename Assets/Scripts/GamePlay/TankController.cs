using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private InputHandler inputHandler;
    private Player player;
    private CameraBehaviour cameraTarget;
    public GameObject Crosshair; //set in inspector

    private float transformSendTime;
    public float SendEvery;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        inputHandler.Init();
        player = GetComponent<Player>();
        cameraTarget = GameObject.Find("Camera").GetComponent<CameraBehaviour>();
        cameraTarget.Init(transform, inputHandler);
        Crosshair = GameObject.Find("Crosshair");
        Cursor.visible = false;
    }

    void Update()
    {
        //player is in the pause menu
        if (GameManager.IsPaused) //<- check inputs in the pause menu
        {
            //if pressing the pause key or the interrupt key, trigger the TRY INTERRUPT PAUSE event
            if (inputHandler.pauseButtonPressed || inputHandler.interruptButtonPressed)
            {
                UI_EventSystem.TryInterruptPause();
            }

            return;
        }

        //player is playing
        if (inputHandler.pauseButtonPressed)
        {
            //fire event to set pause
            UI_EventSystem.PauseGame();
            return;
        }

        if (inputHandler.statsButtonPressed)
            UI_EventSystem.ToggleStats();

        Ray ray = cameraTarget.Camera.ScreenPointToRay(inputHandler.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 50f))
        {
            //Debug.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal * 2f, Color.red);
            Quaternion lookRotation = Quaternion.LookRotation(
                (
                new Vector3(hitInfo.point.x, 0, hitInfo.point.z) -
                new Vector3(player.Turret.transform.position.x, 0, player.Turret.transform.position.z)
                ).normalized, Vector3.up);
            player.Turret.transform.rotation = Quaternion.Slerp(player.Turret.transform.rotation, lookRotation, player.TurretRotationSpeed * Time.deltaTime);

            Crosshair.transform.position = hitInfo.point + hitInfo.normal * 0.1f;
            Crosshair.transform.forward = hitInfo.normal;
        }

        Vector3 movementDirection = (cameraTarget.Camera.transform.right * inputHandler.movementHorizontal) +
            (cameraTarget.CameraForward * -inputHandler.movementVertical);

        //if moving
        if (movementDirection != Vector3.zero)
        {
            //1 - Calculate the angle between the tank forward and the movement direction.
            //2 - Calculate the angle between the tank backward and the movement direction.
            //3 - Choose between forward and backward based on the SMALLEST angle.
            //4 - Lerp the chosen side rotation to reach the desired movement direction rotation.
            //5 - If after the rotation the angle is smaller than a given threshold
            //    Move the tank in movement direction * speed * deltatime

            float fAngle = Vector3.Angle(player.Base.transform.forward, movementDirection);
            float bAngle = Vector3.Angle(-player.Base.transform.forward, movementDirection);

            if (fAngle <= bAngle)
            {
                player.Base.transform.forward = Vector3.Lerp(player.Base.transform.forward, movementDirection, player.RotationSpeed * Time.deltaTime);
                if (Vector3.Angle(player.Base.transform.forward, movementDirection) < player.ManovrabilityAngle)
                    transform.position += player.Base.transform.forward * player.MovementSpeed * Time.deltaTime;
            }
            else
            {
                player.Base.transform.forward = Vector3.Lerp(player.Base.transform.forward, -movementDirection, player.RotationSpeed * Time.deltaTime);
                if (Vector3.Angle(player.Base.transform.forward, -movementDirection) < player.ManovrabilityAngle)
                    transform.position -= player.Base.transform.forward * player.MovementSpeed * Time.deltaTime;
            }
        }

        if (inputHandler.nextWeaponButtonPressed)
            player.WeaponsHandler.SelectNextWeapon();
        else if (inputHandler.previousWeaponButtonPressed)
            player.WeaponsHandler.SelectPreviousWeapon();

        if(inputHandler.attackButtonPressed && player.WeaponsHandler.GetCurrentWeapon().CanShoot)
        {
            Vector3 xzDistance = new Vector3(Crosshair.transform.position.x, transform.position.y, Crosshair.transform.position.z) - transform.position;
            player.WeaponsHandler.GetCurrentWeapon().Shoot(xzDistance.magnitude);
        }

        //set a time limit to send transform data
        transformSendTime += Time.deltaTime;
        if(transformSendTime >= SendEvery)
        {
            if(NetworkManager.CurrentLobby.IsOwnedBy(player.SteamData.Id)) //i am the host
            {
                //send to each player my transform
                foreach (ulong friend in GameManager.Players.Keys)
                {
                    SteamNetworking.SendP2PPacket(friend, P2PPacketWriter.WriteTankTransform(transform.position, player.Base.rotation, player.Turret.rotation, SteamClient.SteamId));
                }
            }
            else //i am a client, send to the host my transform, he will redirect it to other players too
                SteamNetworking.SendP2PPacket(NetworkManager.CurrentLobby.Owner.Id, P2PPacketWriter.WriteTankTransform(transform.position, player.Base.rotation, player.Turret.rotation, SteamClient.SteamId));
            
            transformSendTime = 0;
        }
    }
}
