using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCrate : MonoBehaviour
{
    public GameObject weaponInside;
    public bool withinRange = false;
    public PlayerWeapons playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerInRange != null && playerInRange.weapons.Count < 3)
            {
                // other.GetComponent<PlayerWeapons>().updateWeapon(tempWeapon.GetComponent<weaponData>());
                if (playerInRange.pickupWeapon(weaponInside.GetComponent<weaponData>()))
                {//destroy only if the weapon is picked up
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        playerInRange = other.GetComponent<PlayerWeapons>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (playerInRange == other.GetComponent<PlayerWeapons>())
            playerInRange = null;
    }
}
