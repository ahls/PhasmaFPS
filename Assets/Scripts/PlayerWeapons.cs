using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] Camera cam;
    private PlayerMovement _playermovement;
    private PhotonView _pv;

    //shooting
    [SerializeField] GameObject ImpactPrefab;
    [SerializeField] GameObject HitImpactPrefab;
    private ParticleSystem fireflare;
    private Transform firelocation;
    private float fireTimer;
    private AudioSource audioSource;


    //weapon related
    [HideInInspector] public weaponData _weaponData { set; get; }
    public Dictionary<int,weaponData> weapons = new Dictionary<int, weaponData>();
    private int currentWeaponIndex = 3;
    private float reloadingTimer;
    private bool isReloading = false;
    [SerializeField] GameObject DefaultWeapon;
    [SerializeField] Text _loadedAmmo, _totalAmmo;
    [SerializeField] GameObject cratePrefab;

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
        _pv = GetComponent<PhotonView>();
        GameObject tempKnife = Instantiate(DefaultWeapon);

        weapons[3] = tempKnife.GetComponent<weaponData>();
        equipWeapon(3);
        if(_pv.IsMine)
        {
            cam.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pv.IsMine) return;
        fireTimer -= Time.deltaTime;
        if (_weaponData != null)
        {
            fireCheck();
        }
        swap();
        aimDownCheck();
        dropWeaponCheck();
        reloadCheck();
    }

    #region Equipping related
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
        _pv.RPC("HoldWeapon", RpcTarget.AllBuffered);
        //HoldWeapon();
        updateAmmoInfo();
    }
    [PunRPC]
    private void HoldWeapon()
    {
        _weaponData.gameObject.SetActive(true);
        _weaponData.transform.parent = leftHand.parent;
        _weaponData.transform.position = leftHand.parent.position;
        _weaponData.transform.rotation = leftHand.parent.rotation;
        leftHand.position = _weaponData.leftGrip.position;
        leftHand.rotation = _weaponData.leftGrip.rotation;
        rightHand.position = _weaponData.rightGrip.position;
        rightHand.rotation = _weaponData.rightGrip.rotation;
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
                isReloading = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentWeaponIndex != 2)
            {
                equipWeapon(2);
                isReloading = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentWeaponIndex != 3)
            {
                equipWeapon(3);
                isReloading = false;
            }
        }
    }

    public bool pickupWeapon(weaponData pickedWeapon)
    {
        if (weapons.Count < 3)
        {// if there is a empty spot for a new weapon
            if (!weapons.ContainsKey(1))
            {//if 0th slot is empty
                weapons[1] = pickedWeapon;

                equipWeapon(1);
                
            }
            else if (!weapons.ContainsKey(2))
            {//if 1st slot is empty
                weapons[2] = pickedWeapon;

                equipWeapon(2);
                
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// funciton that is called on Update to check for dropping waepon
    /// </summary>
    private void dropWeaponCheck()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (currentWeaponIndex == 3)
            {//you are holding a default weapon, which cannot be dropped
                return;
            }
            _weaponData = null;
            if (currentWeaponIndex == 1)
            {
                spawnWeapon(weapons[1]);
                weapons.Remove(1);
                if (weapons.ContainsKey(2))
                {
                    equipWeapon(2);
                }
                else
                {
                    equipWeapon(3);
                }
            }
            else if (currentWeaponIndex == 2)
            {
                spawnWeapon(weapons[2]);
                weapons.Remove(2);
                if (weapons.ContainsKey(1))
                {
                    equipWeapon(1);
                }
                else
                {
                    equipWeapon(3);
                }
            }
        }
    }
    /// <summary>
    /// helper funciton that creates the weapon on the world
    /// </summary>
    /// <param name="droppingWeapon"></param>
    private void spawnWeapon(weaponData droppingWeapon)
    {
        if(droppingWeapon.loadedAmmo == 0 && droppingWeapon.currentAmmo == 0)
        {
            Destroy(droppingWeapon.gameObject);
        }
        else
        {
            GameObject newCrate = Instantiate(cratePrefab, transform.position, Quaternion.identity);
            droppingWeapon.transform.SetParent(newCrate.transform);
            droppingWeapon.transform.position = newCrate.transform.position;
            droppingWeapon.transform.localEulerAngles = new Vector3(-90, 0, 0);
            newCrate.GetComponent<weaponCrate>().weaponInside = droppingWeapon.gameObject;
            RaycastHit floor;
            if(Physics.Raycast(newCrate.transform.position, Vector3.down, out floor))
            {
                newCrate.transform.position = floor.point + Vector3.up;
            }
            
        }
    }

    #endregion



    #region Shooting related
    private void fireCheck()
    {
        if (Input.GetMouseButton(0)) // if left clicked
        {
            if (!isReloading)
            {
                if (_weaponData.loadedAmmo > 0)
                {
                    fire();
                }
                else if (_weaponData.currentAmmo > 0)
                {
                    reloadingTimer = Time.time + _weaponData.reloadTime;
                    isReloading = true;
                }
            }
        }
    }
    private void fire()
    {
        if (fireTimer > 0) return;
        fireTimer = 60 / _weaponData.fireRate; // cooldown is (60seconds / RPM)
        for (int i = 0; i < _weaponData.numPelletes; i++)
        {
            Quaternion bulletSpread = Quaternion.Euler(new Vector3(
                                                   Random.Range(-_weaponData.bulletSpread, _weaponData.bulletSpread),
                                                   Random.Range(-_weaponData.bulletSpread, _weaponData.bulletSpread),
                                                   0));
            RaycastHit aimHit;
            if (Physics.Raycast(cam.transform.position + cam.transform.forward * 2, bulletSpread * cam.transform.forward, out aimHit, _weaponData.range))
            {
                RaycastHit hit;

                if (Physics.Raycast(firelocation.position, aimHit.point - firelocation.position, out hit, _weaponData.range))
                {
                    GameObject tempImpact = Instantiate(ImpactPrefab, hit.point, Quaternion.LookRotation(transform.position - hit.point));
                    Destroy(tempImpact, 0.3f);
                    HitPoints HP = hit.transform.GetComponent<HitPoints>();
                    if (HP != null)
                    {
                        HP.takeDamageCaller(_weaponData.damage);
                        audioManager.i.playSound(SoundTypes.playerHit);
                        if(HP.gameObject.tag == "Player")
                        {

                            Destroy(Instantiate(HitImpactPrefab, hit.point, Quaternion.LookRotation(transform.position - hit.point)), 0.5f);
                        }
                    }
                }
            }
        }
        _weaponData.loadedAmmo--;
        _loadedAmmo.text = _weaponData.loadedAmmo.ToString();
        _pv.RPC("GunFireEffect", RpcTarget.AllBuffered);
        _playermovement.AddRecoil(_weaponData.RecoilX_min, _weaponData.RecoilY_min, _weaponData.RecoilX_max, _weaponData.RecoilY_max);
        
    }
    [PunRPC]
    private void GunFireEffect()
    {
        fireflare.Play();
        audioSource.PlayOneShot(audioSource.clip,audioSource.volume);
    }


    private void reloadCheck()
    {

        if (Input.GetKeyDown(KeyCode.R) && 
            _weaponData.currentAmmo > 0 &&
            _weaponData.loadedAmmo != _weaponData.magazineSize &&
            !isReloading)
        {
            reloadingTimer = Time.time + _weaponData.reloadTime;
            isReloading = true;
            audioManager.i.playSound(SoundTypes.reloadStart);
        }
        if(isReloading)
        {
            if(Time.time > reloadingTimer)
            {
                reloadWeapon();
            }
        }
    }
    private void reloadWeapon()
    {
        int newMag = Mathf.Min(_weaponData.magazineSize, _weaponData.currentAmmo+_weaponData.loadedAmmo);
        _weaponData.currentAmmo -= newMag - _weaponData.loadedAmmo;
        _weaponData.loadedAmmo = newMag;
        updateAmmoInfo();
        isReloading = false;
        audioManager.i.playSound(SoundTypes.reloadEnd);
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
    #endregion

    private void updateAmmoInfo()
    {  
        _loadedAmmo.text = _weaponData.loadedAmmo.ToString();
        _totalAmmo.text = _weaponData.currentAmmo.ToString();
    }
    public void init(GameObject canvas)
    {
        CanvasConnector cc = canvas.GetComponent<CanvasConnector>();

        _loadedAmmo = cc.CurrentAmmo;
        _totalAmmo = cc.TotalAmmo;
        GetComponent<HitPoints>().isPlayer = true;
        GetComponent<HitPoints>()._healthDisplay = cc.Health;

    }
}
