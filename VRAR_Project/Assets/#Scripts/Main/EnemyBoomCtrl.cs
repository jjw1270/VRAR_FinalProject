using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomCtrl : MonoBehaviour
{
    public Renderer changeColorObj;
    private Color objColor;
    private bool isOn = false;
    HPCtrl myHp;

    void Start()
    {
        objColor = changeColorObj.GetComponent<Renderer>().material.color;
        changeColorObj = GetComponent<Renderer>();
        myHp = this.GetComponent<HPCtrl>();
        InvokeRepeating("UpdateTarget", 0f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(myHp.curHP <= 0f){
            CancelInvoke("UpdateTarget");
            return;
        }
        if(isOn)
            this.transform.localScale += Vector3.one * 10f * Time.deltaTime;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 300f);
    }

    private void UpdateTarget(){
        Collider [] cols = Physics.OverlapSphere(transform.position, 300f, 1<<9);
        
        if(cols.Length>0){
            Debug.Log("폭탄작동..");
            isOn = true;
            CancelInvoke("UpdateTarget");
            changeColorObj.material.color = Color.red;
            Invoke("Explosion", 5f);
        }
    }

    void Explosion(){
        //폭발
        myHp.curHP = -10f;
    }
}
