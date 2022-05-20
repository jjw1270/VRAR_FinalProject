﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomCtrl : MonoBehaviour
{
    Transform target;
    public Renderer changeColorObj;
    private Color objColor;
    private bool isOn;
    public GameObject ExplosionEffect;

    void Start()
    {
        objColor = changeColorObj.GetComponent<Renderer>().material.color;
        changeColorObj = GetComponent<Renderer>();
        InvokeRepeating("UpdateTarget", 0f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 100f);
    }

    private void UpdateTarget(){
        Collider [] cols = Physics.OverlapSphere(transform.position, 100f, 1<<9);
        
        if(cols.Length>0){
            Debug.Log("나를 발견");
            target = cols[0].gameObject.transform;
            CancelInvoke("UpdateTarget");
            changeColorObj.material.color = Color.red;
            Invoke("Explosion", 3f);
        }
    }

    void Explosion(){
        //폭발
        Instantiate(ExplosionEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
