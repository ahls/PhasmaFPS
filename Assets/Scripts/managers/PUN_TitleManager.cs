using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PUN_TitleManager :  MonoBehaviourPunCallbacks
{
    private string _gameVersion = "0.01";
    [SerializeField] private InputField _createIF;
    [SerializeField] private InputField _createPN;
    [SerializeField] private InputField _joinIF;
    [SerializeField] private InputField _joinPN;
    [SerializeField] private GameObject _errorPopup;
    [SerializeField] private Text _errorTxt;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = _gameVersion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorTxt.text = ("Room you tried to create failed due to the following reason:\n" + message);
        _errorPopup.SetActive(true);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _errorTxt.text = ("Room you tried to join failed due to the following reason:\n" + message);
        _errorPopup.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("Lobby");
    }



    #region buttons events
    public void OnCreateButton()
    {
        PhotonNetwork.CreateRoom(_createIF.text, new RoomOptions { MaxPlayers = 2 });
        PhotonNetwork.NickName = _createPN.text;
    }
    public void OnJoinButton()
    {
        PhotonNetwork.JoinRoom(_joinIF.text);
        PhotonNetwork.NickName = _joinPN.text;
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
    #endregion
}
