using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PUN_LobbyManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPanelPrefab;
    [SerializeField] Transform _playersPanel;
    private PhotonView _pv;
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        GameObject newPanel =  PhotonNetwork.Instantiate(Path.Combine("Prefabs","Networking",_playerPanelPrefab.name),Vector3.zero,Quaternion.identity,0);
        _pv.RPC("AddToPanel", RpcTarget.MasterClient, newPanel);
        
    }

    [PunRPC]
    private void AddToPanel(GameObject newPanel)
    {
        newPanel.transform.SetParent(_playersPanel);
    }
}
