﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    public Camera mainCam;
    private Vector3 screenCenter;
    public Image aim;
    private Color aimColor;
    private PhotonView pv;
    public AudioSource audioSourceF;
    public AudioClip fireSound, lastFireSound;
    public GameObject[] fireEffect;
    private bool isDelay = false;
    private float delayTime = 1.3f;
    private bool isLFS;
    private PlayerMove pm;
    public Text playerName;
    void Start()
    {
        pm = this.GetComponent<PlayerMove>();

        pv = GameObject.Find("Super_Spitfire").GetComponent<PhotonView>();
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        if(!pv.IsMine) return;

        
        this.gameObject.tag = "Mine";
        screenCenter = new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight / 2);
        aimColor = aim.color;
    }

    void Update()
    {
        if(!pv.IsMine) return;

        if(!pm.isDestroy)
            rayCtrl();
        
    }

    void rayCtrl(){
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * 10000f, Color.red);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 10000f)){
            if(hit.transform.CompareTag("Enemy")){
                aim.color = Color.red;
                pv.RPC("Fire", RpcTarget.Others, hit.transform.gameObject);
                if(!isDelay){
                    isDelay = true;
                    FireSound(hit.transform);
                    StartCoroutine(fireSoundDelay());
                }
            }
            else{
                fireEffect[0].SetActive(false);
                fireEffect[1].SetActive(false);
                if(isDelay){
                    if(!isLFS){
                        isLFS = true;
                        audioSourceF.Stop();
                        //audioSourceF.clip = lastFireSound;
                        audioSourceF.volume = 0.6f;
                        audioSourceF.PlayOneShot(lastFireSound);
                    }
                }
                aim.color = aimColor;
            }
        }
        else{
        }
    }
    IEnumerator fireSoundDelay(){
        Debug.Log("발사");
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
    void FireSound(Transform target){
        isLFS = false;
        //audioSourceF.clip = fireSound;
        audioSourceF.Stop();
        audioSourceF.volume = 1.0f;
        audioSourceF.PlayOneShot(fireSound);

        fireEffect[0].SetActive(true);
        fireEffect[1].SetActive(true);
    }

    [PunRPC]
    void Fire(GameObject enemy){
        //isFire = ture;
    }
}
