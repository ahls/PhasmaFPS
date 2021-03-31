using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    [SerializeField] private float maxHP;
    public float currentHP { set; get; }
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
    {ad
        currentHP = maxHP;
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

        if(currentHP <= 0)
        {
            onDeath();
        }
    }
    private void onDeath()
    {
        gameObject.SetActive(false);
    }
}
