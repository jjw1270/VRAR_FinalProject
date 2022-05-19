using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneCtrl : MonoBehaviour
{
    private Transform[] waypoint;
    public float speed = 0.4f;
    public float damping = 3.0f;
    private Transform enemyTransform;
    private int nextIndex;
    Transform target;
    public AudioSource audioSource;
    public GameObject[] fireEffect;
    private bool isDelay = false;
    private float delayTime = 1.3f;
    void Start()
    {
        enemyTransform = this.GetComponent<Transform>();
        waypoint = GameObject.Find("EnemyMovePath").transform.GetComponentsInChildren<Transform>();
        nextIndex = Random.Range(1, waypoint.Length);

        InvokeRepeating("UpdateTarget", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            MoveWayPoint();
        else{
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 500f)){
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.blue);
                if(hit.transform.CompareTag("Player")){
                    if(!isDelay){
                        isDelay = true;
                        StartCoroutine(fireSoundDelay(hit.transform));
                    }
                    Debug.Log("플레이어맞음");
                }
            }
            else{
                fireEffect[0].SetActive(false);
                fireEffect[1].SetActive(false);
                if(isDelay){
                StopAllCoroutines();
                isDelay = false;
                }
            }
            Vector3 dir = target.position - this.transform.position;
            this.transform.Translate(dir.normalized * speed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * damping);
        }
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 300f);
    }

    IEnumerator fireSoundDelay(Transform player){
        fireEffect[0].SetActive(true);
        fireEffect[1].SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }

    void MoveWayPoint(){
        Vector3 direction = waypoint[nextIndex].position - enemyTransform.position;
        Quaternion goalRotation = Quaternion.LookRotation(direction);

        enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, goalRotation, Time.deltaTime * damping);
        enemyTransform.Translate(Vector3.forward * speed);
    }

    private void UpdateTarget(){
        Collider [] cols = Physics.OverlapSphere(transform.position, 300f, 1<<9);
        
        if(cols.Length>0){
            Debug.Log("나를 발견");
            target = cols[0].gameObject.transform;
        }
        else{
            Debug.Log("나를 놓침");
            target = null;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("WayPath")){
            //nextIndex = (++nextIndex >= waypoint.Length) ? 1 : nextIndex;
            if((nextIndex+1) >= waypoint.Length)
                nextIndex = 1;
            else if((nextIndex+2) >= waypoint.Length)
                nextIndex += 1;
            else
                nextIndex = Random.Range(nextIndex+1, nextIndex+2);
        }
    }
}
