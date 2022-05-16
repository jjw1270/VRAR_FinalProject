using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    PhotonView pv;
    public GameObject player;
    public GameObject mainCam;
    float moveSpeed = 0.05f;
    float cur_angle;
    float prev_angle;
    float delta_angle;
    private bool isBoundary;
    public GameObject warningText;
    public Text warningCount;
    private bool isWarning;
    float time = 10f;
    public GameObject explosionEffect;
    public bool isDestroy;
    public GameObject prop;
    private float propSpeed = 500f;
    public AudioSource audioSourceE;
    public AudioClip explosionSound;

    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        audioSourceE.Play();
        // if(!pv.IsMine)
        //     mainCam.GetComponent<Camera>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDestroy)
            prop.transform.Rotate(Vector3.up * propSpeed);
        //if(!pv.IsMine) return;
        moveSpeed += 0.2f * Time.deltaTime;
        if(moveSpeed >= 1f)
            moveSpeed = 1f;
        MoveLookAt();

        if(isWarning){
            warningText.SetActive(true);
            time -= Time.deltaTime;
            warningCount.text = (Mathf.Ceil(time)).ToString();
            if(time <= 0){
                warningCount.text = "0";
                Debug.Log("boom");
                destroy();
            }
        }
        else{
            time = 10f;
            warningText.SetActive(false);
            warningCount.text = "";
        }
    }

    void MoveLookAt(){
        if(isDestroy) {
            this.transform.position = (this.transform.position);
            return;
        }

        this.transform.Translate(mainCam.transform.forward * moveSpeed);

        cur_angle = mainCam.transform.eulerAngles.y;
        delta_angle = cur_angle - prev_angle;
        prev_angle = cur_angle;

        if(delta_angle < -1){
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, 
                Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 60), Time.deltaTime);
        }
        else if(delta_angle > 1){
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, 
                Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, -60), Time.deltaTime);
        }
        else {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, 
                Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0), Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Boundary")){
            //맵의 경계에 부딪히면 10초안에 돌아가라고 경고, 10초 지나면 사망
            Debug.Log("Boundary");
            if(!isWarning){
                isWarning = true;
            }
            else{
                isWarning = false;
            }
        }
        else if(other.transform.CompareTag("MapColli")){
            //지형과 부딪히면 폭파됨
            Debug.Log("MAP");
            destroy();
        }
    }
    void OnCollisionEnter(Collision other) {
        Debug.Log("HIT");
        if(other.transform.CompareTag("MapColli")){
            //지형과 부딪히면 폭파됨
            Debug.Log("MAP");
            destroy();
        }
    }

    void destroy(){
        if(!isDestroy){
            isDestroy = true;
            Destroy(player);
            Instantiate(explosionEffect, player.transform.position, player.transform.rotation);
            audioSourceE.Stop();
            audioSourceE.PlayOneShot(explosionSound);
        }
    }
}