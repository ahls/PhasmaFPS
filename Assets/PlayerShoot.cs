using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public weaponData _weaponData { set; get; }
    private float fireTimer;
    [SerializeField] Camera cam;
    [SerializeField] Transform bulletImpact;
    private ParticleSystem bulletImpactPS;
    // Start is called before the first frame update
    void Start()
    {
        _weaponData = new weaponData(300, 100, 0, 0, 10, 0, 0); // temp lines for testing
        bulletImpactPS = bulletImpact.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if(Input.GetMouseButton(0)) // if left clicked
        {
            if(fireTimer<= 0)
            {
                fire();
            }
        }
        
    }

    private void fire()
    {
        fireTimer = 60 / _weaponData.fireRate; // cooldown is (60seconds / RPM)
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, _weaponData.range))
        {
            bulletImpact.position = hit.point;
            bulletImpact.rotation = Quaternion.LookRotation(transform.position-bulletImpact.position);
            Debug.Log(bulletImpact.rotation);
            HitPoints HP = hit.transform.GetComponent<HitPoints>();
            if(HP != null)
            {
                HP.takeDamage(_weaponData.damage);
            }
            bulletImpactPS.Play(); // adjust this part to show different particle effect depending on which surface is hit
        }
    }
}
