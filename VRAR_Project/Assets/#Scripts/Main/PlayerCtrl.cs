using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public Camera mainCam;
    private Vector3 screenCenter;
    public Image aim;
    private Color aimColor;
    public AudioSource audioSourceF;
    public AudioClip fireSound;
    public GameObject[] fireEffect;
    private bool isDelay = false;
    private float delayTime = 1.3f;
    private PlayerMove pm;
    private HPCtrl enemyHp;
    public Text Score;
    public Text Life;

    // public void setCam(){
    //     mainCam.enabled = true;
    //     mainCam.transform.GetComponent<AudioListener>().enabled = true;
    // }
    void Start()
    {
        // mainCam.enabled = false;
        // mainCam.transform.GetComponent<AudioListener>().enabled = false;
        // Invoke("setCam",0.5f);

        pm = this.GetComponent<PlayerMove>();
        this.gameObject.tag = "Player";
        screenCenter = new Vector3(mainCam.pixelWidth / 2, mainCam.pixelHeight / 2);
        aimColor = aim.color;
    }

    void Update()
    {
        Score.text = GameManager.gameScore.ToString();
        Life.text = GameManager.lifeCount.ToString();
        if(GameManager.isGamePaused) return;

        if(!PlayerMove.isDestroy){
            rayCtrl();
        }
    }

    void rayCtrl(){
        Ray ray = mainCam.ScreenPointToRay(screenCenter);
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * 5000f, Color.red);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 5000f, 1<<10)){
            if(hit.transform.CompareTag("Enemy")){
                enemyHp = hit.transform.GetComponent<HPCtrl>();
                enemyHp.isHit = true;
                aim.color = Color.red;
                if(!isDelay){
                    isDelay = true;
                    StartCoroutine(fireSoundDelay(hit.transform));
                }
            }
        }
        else{
            if(enemyHp!=null)
                enemyHp.isHit = false;
            enemyHp = null;
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
}
