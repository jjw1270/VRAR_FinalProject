using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCam;
    private void LateUpdate() {
        transform.LookAt(mainCam.transform.position);
    }
}
