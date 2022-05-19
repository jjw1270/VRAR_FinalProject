using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform[] points;
    public GameObject[] enemy;
    private int enemyCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount < 20){
            createEnemy();
        }
    }

    void createEnemy(){
        int sp = Random.Range(0, points.Length);
        Debug.Log(sp);
        BoxCollider randomSp = points[sp].GetComponent<BoxCollider>();
        Vector3 originSpPosition = points[sp].position;
        //콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_x = randomSp.bounds.size.x;
        float range_z = randomSp.bounds.size.z;

        range_x = Random.Range((range_x / 2) * -1, range_x / 2);
        range_z = Random.Range((range_z / 2) * -1, range_z / 2);
        Vector3 randomPosition = new Vector3(range_x, Random.Range(0,400), range_z);
        originSpPosition += randomPosition;

        Instantiate(enemy[Random.Range(0,enemy.Length)], originSpPosition, Quaternion.identity);
        enemyCount++;
    }
}
