using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    private Transform playerTransform;
    public Camera mainCam;
    private Transform camTransform;
    private CharacterController charCtrl;
    private float moveSpeed = 2f;

    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        if(!pv.IsMine) return;

        GameObject.Find("GunInUI").layer = 9;  //내 카메라에서만 보에게끔
        playerTransform = this.GetComponent<Transform>();
        camTransform = mainCam.GetComponent<Transform>();
        charCtrl = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLookAt();
    }

    void MoveLookAt(){
        //메인카메라가 바라보는 방향입니다.
        Vector3 dir = mainCam.transform.localRotation * Vector3.forward;
        //카메라가 바라보는 방향으로 팩맨도 바라보게 합니다.
        transform.localRotation = mainCam.transform.localRotation;
        //transform.localRotation = new Quaternion(0, transform.localRotation.y, 0, transform.localRotation.w);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
        //바라보는 시점 방향으로 이동합니다.
        transform.Translate(dir * moveSpeed * Time.deltaTime );
        transform.position = new Vector3(transform.position.x, 1.2f, transform.position.z);
    }
}