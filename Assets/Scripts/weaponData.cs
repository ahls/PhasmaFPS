using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class weaponData : MonoBehaviour
{

    public float fireRate;
    public float range;
    public float bulletSpread;
    public float reloadTime;
    public float damage;
    public int maxAmmo;
    public int currentAmmo;
    public int magazineSize;
    public int loadedAmmo;
   

    public AudioClip shotSound;
    public float RecoilY_min, RecoilY_max;
    public float RecoilX_min, RecoilX_max;
    public int numPelletes;
    public Transform leftGrip;
    public Transform rightGrip;
    public Transform fireLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
