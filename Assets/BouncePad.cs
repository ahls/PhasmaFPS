using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{

    private PlayerMovement player;
    public float height = 20;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            player = col.transform.GetComponent<PlayerMovement>();
            player.jump(height);
        }
    }
}
