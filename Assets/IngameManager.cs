using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class IngameManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject GUIprefab;
    private PhotonView _pv;

    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            GameObject playerUnit = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Networking", "Player"), spawnLocations[_pv.OwnerActorNr].position, Quaternion.identity, 0);
            GameObject gui = Instantiate(GUIprefab);
            playerUnit.GetComponent<PlayerWeapons>().init(gui);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
