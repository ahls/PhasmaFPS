using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class PlayerWeapons : MonoBehaviour
{
    public weaponData _weaponData { set; get; }
    private float fireTimer;
    [SerializeField] Camera cam;
    [SerializeField] Transform bulletImpact;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] private Rig aimRig;
    [SerializeField] Transform aimTarget;
    private ParticleSystem bulletImpactPS;
    private Transform firelocation;
    private float lerpSpeed = 0.1f;

    public GameObject DebugItem;
    // Start is called before the first frame update
    void Start()
    {
        bulletImpactPS = bulletImpact.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (_weaponData != null)
        {
            if (Input.GetMouseButton(0)) // if left clicked
            {
                if (fireTimer <= 0)
                {
                    fire();
                }
            }
        }
        if(Input.GetMouseButton(1))
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, lerpSpeed);
        }
        else
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, lerpSpeed);
        }

    }


    public void updateWeapon(weaponData newWeapon)
    {
        _weaponData = newWeapon;
        newWeapon.transform.parent = leftHand.parent;
        newWeapon.transform.position = leftHand.parent.position;
        newWeapon.transform.rotation = leftHand.parent.rotation;
        leftHand.position = newWeapon.leftGrip.position;
        rightHand.position = newWeapon.rightGrip.position;
        firelocation = newWeapon.fireLocation;
    }


    private void fire()
    {
        fireTimer = 60 / _weaponData.fireRate; // cooldown is (60seconds / RPM)
        RaycastHit aimHit;
        if(Physics.Raycast(cam.transform.position+cam.transform.forward*2,cam.transform.forward,out aimHit,_weaponData.range))
        {
            Instantiate(DebugItem, aimHit.point, Quaternion.identity);
            RaycastHit hit;
            if (Physics.Raycast(firelocation.position, aimHit.point - firelocation.position, out hit, _weaponData.range))
            {
                bulletImpact.position = hit.point;
                bulletImpact.rotation = Quaternion.LookRotation(transform.position - bulletImpact.position);
                Debug.Log(bulletImpact.rotation);
                HitPoints HP = hit.transform.GetComponent<HitPoints>();
                if (HP != null)
                {
                    HP.takeDamage(_weaponData.damage);
                }
                bulletImpactPS.Play(); // adjust this part to show different particle effect depending on which surface is hit
            }
        }

        
    }
}
