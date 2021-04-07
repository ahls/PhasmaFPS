using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCrate : MonoBehaviour
{
    public GameObject weaponPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("being picked up");
        if(other.tag == "Player")
        {
            GameObject tempWeapon = Instantiate(weaponPrefab);
            other.GetComponent<PlayerWeapons>().updateWeapon(tempWeapon.GetComponent<weaponData>());
            Destroy(gameObject);
        }
    }
}
