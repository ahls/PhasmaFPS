using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponData : MonoBehaviour
{

    public float fireRate;
    public float range;
    public float bulletSpread;
    public float recoilAmount;
    public float damage;
    public int maxAmmo;
    public int magazineSize;

    public weaponData(float fr, float rng, float spread, float recoil, float dmg, int mxAm, int magSize)
    {
        fireRate = fr;
        range = rng;
        bulletSpread = spread;
        recoilAmount = recoil;
        damage = dmg;
        maxAmmo = mxAm;
        magazineSize = magSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
