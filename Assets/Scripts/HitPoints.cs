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


    public Image HitDisplay;
    private float HitAlpha;
    private const float HitScreenLerp = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (HitDisplay != null && HitAlpha > 0f)
        {
            HitAlpha = Mathf.Lerp(HitAlpha, 0, HitScreenLerp);
            HitDisplay.color = new Color(1f, 1f, 1f, HitAlpha);
        }
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
            _healthDisplay.text = ((int)Mathf.Ceil(currentHP)).ToString();
        }
        if(HitDisplay != null)
        {
            HitAlpha += Mathf.Min(0.5f,dmg/10);
            HitAlpha = Mathf.Min(HitAlpha, 1f);
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
