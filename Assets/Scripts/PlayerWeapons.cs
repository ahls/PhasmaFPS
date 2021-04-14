﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class PlayerWeapons : MonoBehaviour
{
    public weaponData _weaponData { set; get; }
    private float fireTimer;
    [SerializeField] Camera cam;
    [SerializeField] GameObject ImpactPrefab;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] private Rig aimRig;
    [SerializeField] Transform aimTarget;
    private ParticleSystem fireflare;
    private Transform firelocation;
    private float lerpSpeed = 0.1f;
    private PlayerMovement _playermovement;
    

    public Dictionary<int,weaponData> weapons = new Dictionary<int, weaponData>();
    private int currentWeaponIndex;
    // Start is called before the first frame update
    void Start()
    {
        _playermovement = GetComponent<PlayerMovement>();
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
        aimDownCheck();
        dropWeaponCheck();
    }


    public void equipWeapon(int index)
    {
        if(!weapons.ContainsKey(index))
        {
            return;
        }
        currentWeaponIndex = index;
        
        _weaponData = weapons[index];
        _weaponData.transform.parent = leftHand.parent;
        _weaponData.transform.position = leftHand.parent.position;
        _weaponData.transform.rotation = leftHand.parent.rotation;
        leftHand.position = _weaponData.leftGrip.position;
        rightHand.position = _weaponData.rightGrip.position;
        firelocation = _weaponData.fireLocation;
        fireflare = _weaponData.fireLocation.GetComponent<ParticleSystem>();
    }


    private void fire()
    {
        fireTimer = 60 / _weaponData.fireRate; // cooldown is (60seconds / RPM)
        RaycastHit aimHit;
        if (Physics.Raycast(cam.transform.position+cam.transform.forward*2,cam.transform.forward,out aimHit,_weaponData.range))
        {
            RaycastHit hit;
            for (int i = 0; i < _weaponData.numPelletes; i++)
            {
                Vector3 spreadOffset = new Vector3(Random.Range(-_weaponData.bulletSpread, _weaponData.bulletSpread),
                                                   Random.Range(-_weaponData.bulletSpread, _weaponData.bulletSpread),
                                                   Random.Range(-_weaponData.bulletSpread, _weaponData.bulletSpread));
                if (Physics.Raycast(firelocation.position, aimHit.point - firelocation.position + spreadOffset, out hit, _weaponData.range))
                {
                    GameObject tempImpact = Instantiate(ImpactPrefab, hit.point, Quaternion.LookRotation(transform.position - hit.point));
                    Destroy(tempImpact, 0.3f);
                    HitPoints HP = hit.transform.GetComponent<HitPoints>();
                    if (HP != null)
                    {
                        HP.takeDamage(_weaponData.damage);
                    }
                }
            }
        }
        fireflare.Play();
        _playermovement.AddRecoil(_weaponData.RecoilX_min, _weaponData.RecoilY_min, _weaponData.RecoilX_max, _weaponData.RecoilY_max);
        
    }


    public void pickupWeapon(weaponData pickedWeapon)
    {
        if(weapons.Count <2)
        {// if there is a empty spot for a new weapon
            if(!weapons.ContainsKey(1))
            {//if 0th slot is empty
                weapons[1] = pickedWeapon;
                if(weapons.Count == 1)
                {
                    equipWeapon(1);
                }
            }
            else if(!weapons.ContainsKey(2))
            {//if 1st slot is empty
                weapons[2] = pickedWeapon; 
                if (weapons.Count == 1)
                {
                    equipWeapon(2);
                }
            }
        }
    }
    private void dropWeaponCheck()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(currentWeaponIndex ==3)
            {//you are holding a melee weapon, which cannot be dropped
                return;
            }
            else if(currentWeaponIndex ==1)
            {
                if(weapons.ContainsKey(2))
                {
                    equipWeapon(2);
                }
                else
                {
                    equipWeapon(3);
                }
                spawnWeapon(weapons[1]);
                weapons.Remove(1);
            }
            else if(currentWeaponIndex == 2)
            {
                if (weapons.ContainsKey(1))
                {
                    equipWeapon(1);
                }
                else
                {
                    equipWeapon(3);
                }
                spawnWeapon(weapons[2]);
                weapons.Remove(2);
            }
        }
    }

    private void spawnWeapon(weaponData droppingWeapon)
    {

    }

    private void aimDownCheck()
    {
        if (Input.GetMouseButton(1))
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, lerpSpeed);
        }
        else
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, lerpSpeed);
        }
    }
}
