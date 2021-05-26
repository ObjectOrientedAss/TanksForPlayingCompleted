using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FriendCard : MonoBehaviour
{
    public Friend friend { get; private set; } //the user data of this card
    public RawImage Avatar; //set in inspector
    public TextMeshProUGUI SteamName; //set in inspector

    public void Init(Friend friend)
    {
        this.friend = friend;
        SteamName.text = friend.Name;

        _ = GetAvatar();
        SteamFriends.RequestUserInformation(friend.Id, false);
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

    public void OnInviteButtonClick()
    {
        //Invite this friend
        NetworkManager.CurrentLobby.InviteFriend(friend.Id);
        //friend.InviteToGame("Come play with me, you dumb!"); //invita la persona a giocare e non nella lobby
        //Debug.Log("Sent lobby invite to " + friend.Name);
    }
}
