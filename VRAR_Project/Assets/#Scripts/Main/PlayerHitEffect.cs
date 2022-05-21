using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitEffect : MonoBehaviour
{
    HPCtrl myHp;
    public GameObject hitEffect;
    public GameObject lowHitEffect;
    float tmpHP;
    void Start()
    {
        myHp = this.GetComponent<HPCtrl>();
        tmpHP = myHp.curHP;
    }

    void Update()
    {
        if(tmpHP != myHp.curHP){
            tmpHP = myHp.curHP;
            hitEffect.SetActive(true);
            Invoke("stopEffect", 1.5f);
        }
        if(tmpHP < 40f){
            //연기
            lowHitEffect.SetActive(true);
        }
    }

    public void stopEffect(){
        hitEffect.SetActive(false);
    }
}
