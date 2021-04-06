using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimTargetBehaviour : MonoBehaviour
{
    [SerializeField] Camera cam;
    // Update is called once per frame
    void Update()
    {
        transform.position = cam.transform.position + cam.transform.forward * 20;
    }
}
