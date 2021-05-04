using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class IngameManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject GUIprefab;

    // Start is called before the first frame update
    void Start()
    {
            GameObject playerUnit = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Networking", "Player"), spawnLocations[Random.Range(0,4)].position, Quaternion.identity, 0);
            GameObject gui = Instantiate(GUIprefab);
            playerUnit.GetComponent<PlayerWeapons>().init(gui);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
