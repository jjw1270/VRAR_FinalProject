using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayClick : MonoBehaviour
{
    public Image aim;
    private Vector3 screenCenter;
    public Camera mainCam;
    private Color aimColor;
    void Start()
    {
        aimColor = aim.color;
        screenCenter = new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000f)){
            aim.color = Color.red;
            if(Input.GetMouseButtonDown(0)){
                if(hit.collider.CompareTag("Button")){
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
        else{
            aim.color = aimColor;
        }
    }
}