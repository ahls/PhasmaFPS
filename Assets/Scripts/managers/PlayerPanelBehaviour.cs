using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerPanelBehaviour : MonoBehaviour
{
    static private int _numReadyPlayers = 0;
    private PhotonView _pv;
    private Transform _playersPanel;
    [SerializeField] GameObject _readyButton;
    [SerializeField] Text _playerName;
    // Start is called before the first frame update
    void Start()
    {
        _playersPanel = GameObject.Find("Players panel").transform;
        transform.SetParent(_playersPanel);
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            _readyButton.GetComponent<Button>().interactable = (true);
        }
            _playerName.text = _pv.Owner.NickName;  
    }

    public void OnHitReady()
    {
        _pv.RPC("ReadyStates", RpcTarget.AllBuffered);
    }


    [PunRPC]
    private void ReadyStates()
    {
        _readyButton.transform.GetChild(0).GetComponent<Text>().text = "READY!";
        _numReadyPlayers++;
        if (PhotonNetwork.IsMasterClient && _numReadyPlayers == 2)
        {
            Debug.Log("Arena is being loaded");
            PhotonNetwork.LoadLevel("Arena");
        }
    }


}
