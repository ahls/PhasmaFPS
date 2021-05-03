using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerPanelBehaviour : MonoBehaviour
{
    private int _numReadyPlayers = 0;
    private PhotonView _pv;
    [SerializeField] GameObject _readyButton;
    [SerializeField] Text _playerName;
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            _readyButton.GetComponent<Button>().interactable = (true);
            _playerName.text = PhotonNetwork.NickName;  
        }
    }

    public void OnHitReady()
    {
        _readyButton.transform.GetChild(0).GetComponent<Text>().text = "READY!";
        _pv.RPC("ReadyStates", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void ReadyStates()
    {
        _numReadyPlayers++;
        if (_numReadyPlayers == 2)
        {
            PhotonNetwork.LoadLevel("Arena");
        }
    }


}
