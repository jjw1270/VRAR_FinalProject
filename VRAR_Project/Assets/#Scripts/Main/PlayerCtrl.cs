using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public Camera mainCam;
    private Vector3 screenCenter;
    public GameObject player;
    public Image aim;
    private Color aimColor;
    public AudioSource audioSourceF;
    public AudioClip fireSound;
    public GameObject[] fireEffect;
    private bool isDelay = false;
    private float delayTime = 1.3f;
    private PlayerMove pm;
    public bool isHit;
    public Image hpBar;
    private float maxHP = 100f;
    private float curHP = 100f;
    void Start()
    {
        pm = this.GetComponent<PlayerMove>();

        this.gameObject.tag = "Player";
        screenCenter = new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight / 2);
        aimColor = aim.color;
    }

    void Update()
    {
        if(GameManager.isGamePaused) return;

        if(!PlayerMove.isDestroy){
            rayCtrl();
            hpBar.fillAmount = curHP / maxHP;
            if(isHit)
                IDamaged();
            if(curHP <= 0f){
                pm.destroy();
            }
        }
    }

    void rayCtrl(){
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * 5000f, Color.red);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 5000f, 1<<10)){
            Debug.Log(hit.collider.gameObject.tag);
            if(hit.transform.CompareTag("Enemy")){
                Debug.Log("???");
                aim.color = Color.red;
                if(!isDelay){
                    isDelay = true;
                    StartCoroutine(fireSoundDelay(hit.transform));
                }
            }
        }
        else{
                fireEffect[0].SetActive(false);
                fireEffect[1].SetActive(false);
                if(isDelay){
                    StopAllCoroutines();
                    isDelay = false;
                }
                audioSourceF.Stop();
                aim.color = aimColor;
            }

    }
    IEnumerator fireSoundDelay(Transform enemy){
        fireEffect[0].SetActive(true);
        fireEffect[1].SetActive(true);
        audioSourceF.clip = fireSound;
        audioSourceF.Play();
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }

    void IDamaged(){
        {
            isHit = false;
            curHP -= 10;
            
            //HP감소, 피격이펙트
        }
    }
}
