using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class HitPoints : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float currentHP { set; get; }
    public ParticleSystem ps;
    public bool isPlayer { get; set; } = false;
    public Text _healthDisplay;
    private PhotonView _pv;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region setups
    public void init()
    {
        currentHP = maxHP;
        _pv = GetComponent<PhotonView>();
    }
    public void init(float newHealth)
    {
        maxHP = newHealth;
        init();
    }
    #endregion

    public void takeDamageCaller(float dmg)
    {
        _pv.RPC("takeDamage", RpcTarget.AllBuffered, dmg);
    }
    [PunRPC]
    private void takeDamage(float dmg)
    {
        currentHP -= dmg;
        if (isPlayer && _pv.IsMine)
        {
            _healthDisplay.text = ((int)currentHP).ToString();
        }

        if (currentHP <= 0)
        {
            onDeath();
        }
    }
    private void onDeath()
    {
        if (ps != null)
        {
            ps.Play();
            ps.transform.parent = transform.parent;
        }
        gameObject.SetActive(false);
    }
}
