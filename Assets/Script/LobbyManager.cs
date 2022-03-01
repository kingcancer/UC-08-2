using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomInput;
    public GameObject createPanel;
    public GameObject roomPanel;
    public Text roomName;

    public RoomItem roomItemPrefab;
    private List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contentObject;

    private float tempoEntreUpdate = 1.5f;
    private float proxUpdate;

    public List<PlayerItem> PlayerItemList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnclickCreate()
    {
        if (roomInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions(){MaxPlayers = 4});
        }
    }

    public override void OnJoinedRoom()
    {
        createPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Sala: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= proxUpdate)
        {
            UpdateRoomList(roomList);
            proxUpdate = Time.time + tempoEntreUpdate;
        }
        
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        createPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in PlayerItemList)
        {
            Destroy(item.gameObject);
        }
        PlayerItemList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            PlayerItemList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
        
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("Fase");
    }
}
