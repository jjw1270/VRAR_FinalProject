using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayClick : MonoBehaviour
{
    public Image aim;
    private Color aimColor;
    void Start()
    {
        aimColor = aim.color;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 1000;

        if(Physics.Raycast(transform.position, forward, out hit)){
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