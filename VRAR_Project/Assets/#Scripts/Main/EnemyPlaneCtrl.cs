using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneCtrl : MonoBehaviour
{
    private Transform[] waypoint;
    private float speed = 1f;
    public float damping = 3.0f;
    private int nextIndex;
    Transform target;
    public AudioSource audioSource;
    public GameObject[] fireEffect;
    private bool isDelay = false;
    private float delayTime = 1.3f;
    void Start()
    {
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
            this.transform.Translate(dir.normalized * speed * Time.deltaTime);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * damping);
        }
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 300f);
    }

    IEnumerator fireSoundDelay(Transform player){
        Debug.Log("플레이어맞음");
        fireEffect[0].SetActive(true);
        fireEffect[1].SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }

    void MoveWayPoint(){
        Vector3 direction = waypoint[nextIndex].position - this.transform.position;
        Quaternion goalRotation = Quaternion.LookRotation(direction);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, goalRotation, Time.deltaTime * damping);
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime * 100f);
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
            if((nextIndex+1) >= waypoint.Length)
                nextIndex = 1;
            else if((nextIndex+2) >= waypoint.Length)
                nextIndex += 1;
            else if(nextIndex==1)
                nextIndex = Random.Range(2, waypoint.Length-2);
            else
                nextIndex = Random.Range(nextIndex+1, nextIndex+2);
        }
    }
}
