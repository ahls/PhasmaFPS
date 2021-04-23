﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage;
    public float radius;
    public float force;


    public void Explode()
    {
        Collider[] hitArray = Physics.OverlapSphere(transform.position, radius);
        foreach(var col in hitArray)
        {
            if (col.tag == "Player")
            {
                HitPoints tempHP = col.GetComponent<HitPoints>();
                if (tempHP != null)
                {
                    tempHP.takeDamage(damage);
                }

                PlayerMovement player = col.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.AddImpact((col.transform.position - this.transform.position).normalized, force);
                }
            }
        }
    }

    void OnDisable()
    {
        Explode();
    }
}
