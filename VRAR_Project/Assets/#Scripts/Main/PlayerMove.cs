using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCam;
    float moveSpeed = 100.0f;
    float cur_angle;
    float prev_angle;
    float delta_angle;
    private bool isBoundary;
    public GameObject warningText;
    public Text warningCount;
    public GameObject warningPanel;
    private bool isWarning;
    float time = 10f;
    public GameObject explosionEffect;
    public static bool isDestroy = false;
    HPCtrl playerHP;
    GameManager gm;
    
    public AudioSource audioSourceE;
    public AudioClip explosionSound;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerHP = this.gameObject.GetComponent<HPCtrl>();
    }

    void Update()
    {
        if(GameManager.isGamePaused) return;
        
        //맵 밖으로 나갔을 때 경고
        if(isWarning){
            warningText.SetActive(true);
            warningPanel.SetActive(true);
            time -= Time.deltaTime;
            warningCount.text = (Mathf.Ceil(time)).ToString();
            if(time <= 0){
                warningCount.text = "0";
                Debug.Log("맵밖사망");
                playerHP.curHP = -1f;
            }
        }
        else{
            time = 10f;
            warningText.SetActive(false);
            warningPanel.SetActive(false);
            warningCount.text = "";
        }
    }
    private void LateUpdate() {
        if(GameManager.isGamePaused) return;
        //플레이어 이동
        MoveLookAt();
    }

    void MoveLookAt(){
        if(isDestroy) {
            this.transform.Translate(Vector3.zero * Time.deltaTime);
            return;
        }

        this.transform.Translate(mainCam.transform.forward * moveSpeed * Time.deltaTime);

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
            //맵의 경계에 부딪치면 10초안에 돌아가라고 경고, 10초 지나면 사망
            Debug.Log("Boundary");
            if(!isWarning){
                isWarning = true;
            }
            else{
                isWarning = false;
            }
        }
        else if(other.transform.CompareTag("Enemy")){
            //적과 부딪치면 폭파됨
            if(other.transform.GetChild(0).name == "Bomb"){
                Debug.Log("부딪힘");
                playerHP.curHP = -1f;
            }
        }
    }
}