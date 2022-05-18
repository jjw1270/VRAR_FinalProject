using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProppelerCtrl : MonoBehaviour
{
    private PlayerMove pm;
    private float propSpeed = 500f;
    void Start()
    {
        pm = transform.root.GetComponent<PlayerMove>();
    }

    void Update()
    {
        if(!PlayerMove.isDestroy)
            this.transform.Rotate(Vector3.up * propSpeed);
    }
}
