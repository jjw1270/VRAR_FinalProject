using System.Collections;
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
    public bool isHit;
    public Slider hpBar;
    private float maxHP = 100f;
    private float curHP = 100f;
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

        if(!pm.isDestroy){
            rayCtrl();
            hpBar.value = curHP / maxHP;
            if(isHit)
                IDamaged();
        }
        
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
                    FireSound();
                    StartCoroutine(fireSoundDelay(hit.transform));
                }
            }
            else{
                fireEffect[0].SetActive(false);
                fireEffect[1].SetActive(false);
                if(isDelay){
                    if(!isLFS){
                        isLFS = true;
                        audioSourceF.Stop();
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
    IEnumerator fireSoundDelay(Transform enemy){
        Debug.Log("발사");
        photonView.RPC("Fire", RpcTarget.Others, enemy);
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }
    void FireSound(){
        isLFS = false;
        audioSourceF.Stop();
        audioSourceF.clip = fireSound;
        audioSourceF.volume = 1.0f;
        audioSourceF.Play();/////////////////////

        fireEffect[0].SetActive(true);
        fireEffect[1].SetActive(true);
    }

    void IDamaged(){
        {
            isHit = false;
            curHP -= 10;
            
            //HP감소, 피격이펙트
        }
    }
    

    [PunRPC]
    void Fire(GameObject enemy){
        enemy.GetComponent<PlayerCtrl>().isHit = true;
    }
}
