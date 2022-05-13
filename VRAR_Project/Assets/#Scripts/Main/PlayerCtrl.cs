using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    public Image aim;
    private Color aimColor;
    private PhotonView pv;
    void Start()
    {
        if(!pv.IsMine) return;

        this.gameObject.tag = "Mine";
        aimColor = aim.color;
    }

    void Update()
    {
        if(!pv.IsMine) return;
        rayCtrl();
        
    }

    void rayCtrl(){
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 1000;

        if(Physics.Raycast(transform.position, forward, out hit)){
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("CanHit")){   //레이어 비교
                aim.color = Color.red;
                if(hit.transform.CompareTag("Enemy")){
                    fire(hit.transform);
                }
                else if(hit.transform.CompareTag("블라블라")){
                    //블라블라
                }
                
            }
            else{
            aim.color = aimColor;
            }
        }
    }

    void fire(Transform target){
        
        float time = 0;
        time += Time.deltaTime;


    }

}
