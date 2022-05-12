using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfRot : MonoBehaviour
{
    public GameObject prof;
    private float speed = 1500f;

    // Update is called once per frame
    void Update()
    {
        prof.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
