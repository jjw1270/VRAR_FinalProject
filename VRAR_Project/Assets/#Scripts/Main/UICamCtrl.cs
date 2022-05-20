using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamCtrl : MonoBehaviour
{
    public Image aim;
    private Vector3 screenCenter;
    public Camera pauseCam;
    private Color aimColor;
    void Start()
    {
        aimColor = aim.color;
        screenCenter = new Vector3(pauseCam.pixelWidth / 2, pauseCam.pixelHeight / 2);
    }

    void Update()
    {
        pauseCam.transform.position = new Vector3(0,0,0.08f);
        Ray ray = pauseCam.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 500f,1<<5)){
            if(hit.collider.CompareTag("Button")){
                aim.color = Color.red;
                if(Input.GetMouseButtonDown(0)){
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
        else{
            aim.color = aimColor;
        }
    }
}