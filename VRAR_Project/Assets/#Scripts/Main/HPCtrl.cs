using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPCtrl : MonoBehaviour
{
    private bool isPlayer;
    PlayerMove pm;
    public bool isHit;
    public Image hpBar;
    public float maxHP = 100f;
    public float curHP = 100f;
    public GameObject explosionEffect;
    public Canvas myHpCanvas;

    void Start()
    {
        //player와 enemy 구분
        if(this.gameObject.CompareTag("Player")){
            isPlayer = true;
            pm = GetComponent<PlayerMove>();
            myHpCanvas = null;
        }
        
    }

    void Update()
    {
        hpBar.fillAmount = curHP / maxHP;
        //enemy HP
        if(!isPlayer){

            if(isHit){
                curHP -= 30f * Time.deltaTime;
            }
            if(curHP <= 0f && curHP > -99f){
                if(curHP == -10f){
                    this.transform.GetChild(2).localScale = new Vector3(20,20,20);
                    this.gameObject.tag = "Destroyed";
                    explosionEffect.SetActive(true);
                    Destroy(this.gameObject, 2f);
                }
                else{
                    if(this.transform.GetChild(0).name == "Bomb"){
                        this.GetComponent<MeshRenderer>().enabled = false;
                        GameManager.gameScore += 100;
                    }
                    else
                        GameManager.gameScore += 500;
                    this.gameObject.tag = "Destroyed";
                    explosionEffect.SetActive(true);
                    Destroy(this.gameObject, 2f);
                }
                curHP = -99f;
                GameManager.enemyCount--;
            }
        }
        //player
        else{
            if(isHit){
                isHit = false;
                curHP -= 5f;
            }
            if(curHP <= 0f && curHP > -99f){
                if(!PlayerMove.isDestroy){
                    curHP = -999f;
                    PlayerMove.isDestroy = true;
                    explosionEffect.SetActive(true);
                    GameManager.lifeCount -= 1;
                    Invoke("playerDestroy", 2f);
                    pm.audioSourceE.Stop();
                }
            }
        }
        
    }
    void LateUpdate(){  //hp바가 나에게 잘 보이게
        if(!isPlayer){
            if(!PlayerMove.isDestroy)
                myHpCanvas.transform.LookAt(Camera.main.transform.position);
        }
    }

    public void playerDestroy(){
        pm.mainCam.transform.GetComponent<AudioListener>().enabled = false;
        pm.mainCam.transform.GetComponent<Camera>().enabled = false;
        //Destroy(pm.player);
    }
}
