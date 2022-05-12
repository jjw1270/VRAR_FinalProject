using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    public GameObject player;
    public GameObject mainCam;
    float moveSpeed = 1f;
    float cur_angle;
    float prev_angle;
    float delta_angle;

    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!pv.IsMine) return;
        MoveLookAt();
    }

    void MoveLookAt(){
        this.transform.Translate(mainCam.transform.forward * moveSpeed);

        cur_angle = mainCam.transform.eulerAngles.y;
        delta_angle = cur_angle - prev_angle;
        prev_angle = cur_angle;

        if(delta_angle < 0){
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, 
                Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 45), Time.deltaTime);
        }
        else if(delta_angle > 0){
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, 
                Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, -45), Time.deltaTime);
        }
        else {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, 
                Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0), Time.deltaTime);
        }
    }
}