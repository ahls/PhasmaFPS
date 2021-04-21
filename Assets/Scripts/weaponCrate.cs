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
        PlayerWeapons PW= other.GetComponent<PlayerWeapons>();
        if (PW != null && !PW.slotFull)
        {
            GameObject tempWeapon = Instantiate(weaponPrefab);
            // other.GetComponent<PlayerWeapons>().updateWeapon(tempWeapon.GetComponent<weaponData>());
            if (other.GetComponent<PlayerWeapons>().pickupWeapon(tempWeapon.GetComponent<weaponData>()))
            {//destroy only if the weapon is picked up
                Destroy(gameObject);
            }
        }
    }
}
