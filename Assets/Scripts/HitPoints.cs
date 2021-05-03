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
    [SerializeField] private bool isPlayer;
    [SerializeField] private Text _healthDisplay;
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



    public void takeDamage(float dmg)
    {
        currentHP -= dmg;
        if(isPlayer && _pv.IsMine)
        {
            _healthDisplay.text = currentHP.ToString();
        }

        if(currentHP <= 0)
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
