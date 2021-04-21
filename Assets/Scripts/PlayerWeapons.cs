using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] Camera cam;
    private PlayerMovement _playermovement;

    //shooting
    [SerializeField] GameObject ImpactPrefab;
    private ParticleSystem fireflare;
    private Transform firelocation;
    private float fireTimer;
    private AudioSource audioSource;


    //weapon related
    public bool slotFull = false;
    public weaponData _weaponData { set; get; }
    public Dictionary<int,weaponData> weapons = new Dictionary<int, weaponData>();
    private int currentWeaponIndex = 3;
    [SerializeField] GameObject KnifePrefab;


    //riggings
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] private Rig aimRig;
    [SerializeField] Transform aimTarget;
    private float lerpSpeed = 0.2f;



    // Start is called before the first frame update
    void Start()
    {
        _playermovement = GetComponent<PlayerMovement>();
        GameObject tempKnife = Instantiate(KnifePrefab);

        weapons[3] = tempKnife.GetComponent<weaponData>();
        equipWeapon(3);
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
        swap();
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
        if (_weaponData != null)
        {
            _weaponData.gameObject.SetActive(false);
        }
        _weaponData = weapons[index];
        _weaponData.gameObject.SetActive(true);
        _weaponData.transform.parent = leftHand.parent;
        _weaponData.transform.position = leftHand.parent.position;
        _weaponData.transform.rotation = leftHand.parent.rotation;
        leftHand.position = _weaponData.leftGrip.position;
        rightHand.position = _weaponData.rightGrip.position;
        firelocation = _weaponData.fireLocation;
        fireflare = _weaponData.fireLocation.GetComponent<ParticleSystem>();
        audioSource = firelocation.parent.GetComponent<AudioSource>();
        audioSource.clip = _weaponData.shotSound;
    }
    private void swap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentWeaponIndex != 1)
            {
                equipWeapon(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentWeaponIndex != 2)
            {
                equipWeapon(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return;
        }
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
        audioSource.PlayOneShot(audioSource.clip,audioSource.volume);
        _playermovement.AddRecoil(_weaponData.RecoilX_min, _weaponData.RecoilY_min, _weaponData.RecoilX_max, _weaponData.RecoilY_max);
        
    }


    public bool pickupWeapon(weaponData pickedWeapon)
    {
        if(weapons.Count <2)
        {// if there is a empty spot for a new weapon
            if(!weapons.ContainsKey(1))
            {//if 0th slot is empty
                weapons[1] = pickedWeapon;
                if(weapons.Count == 2)
                {
                    equipWeapon(1);
                }
            }
            else if(!weapons.ContainsKey(2))
            {//if 1st slot is empty
                weapons[2] = pickedWeapon; 
                if (weapons.Count == 2)
                {
                    equipWeapon(2);
                }
            }
            if(weapons.Count == 3)
            {
                slotFull = true;
            }
            return true;
        }
        return false;
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
