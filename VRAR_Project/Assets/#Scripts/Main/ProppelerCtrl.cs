using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProppelerCtrl : MonoBehaviour
{
    public GameObject proppeler;
    private float propSpeed = 500f;
    void Start()
    {

    }

    void Update()
    {
            proppeler.transform.Rotate(Vector3.up * propSpeed);
    }
}
