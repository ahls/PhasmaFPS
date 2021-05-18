using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class weaponCrate : MonoBehaviour
{
    public GameObject weaponInside;
    public bool withinRange = false;
    public PlayerWeapons playerInRange;
    private const string PICKUP_TEXT = "Press <b> F </b> to pick up ";
    private PhotonView _pv;
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
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
                    playerInRange.WeaponPickupText.text = "";
                    _pv.RPC("RPC_PickedUp", RpcTarget.AllBuffered);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        transform.eulerAngles = transform.eulerAngles + Vector3.up;
    }
    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherPV = other.GetComponent<PhotonView>();
        if (otherPV != null && otherPV.IsMine)
        {
            PlayerWeapons pw = other.GetComponent<PlayerWeapons>();
            playerInRange = pw;
            pw.WeaponPickupText.text = PICKUP_TEXT + weaponInside.name;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (playerInRange == other.GetComponent<PlayerWeapons>())
        {
            playerInRange.WeaponPickupText.text = "";
            playerInRange = null; 
        }
    }

    [PunRPC]
    private void RPC_PickedUp()
    {
        Destroy(gameObject);
    }
}
