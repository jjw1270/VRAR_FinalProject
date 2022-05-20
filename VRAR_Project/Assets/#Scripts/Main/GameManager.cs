using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject player;
    public GameObject[] enemy;
    private int enemyCount = 0;
    GameObject spawnedPlayer;

    private float lastTouchTime;
    private const float doubleTouchDelay = 0.5f;
    public static bool isGamePaused = false;
    public GameObject pauseCam;
    public int lifeCount = 3;

    void Start()
    {
        lastTouchTime = Time.time;
        CreateObj(1);
    }

    void Update(){

        if(enemyCount < 20){
            CreateObj(2);
        }

        if(PlayerMove.isDestroy){
            lifeCount--;
            PlayerMove.isDestroy = false;
            if(lifeCount == 0) {
            //목숨3개!
            //게임오버 UI(일시정지 UI) + 스코어
            }
            else
                Invoke("DestroyPlayerAndSpawn", 3f);
        }

        //더블 터치 시 일시정지 + UI
        if(Input.touchCount == 1){
            Touch touch = Input.GetTouch(0);
            switch(touch.phase){
                case TouchPhase.Began:
                    if(Time.time - lastTouchTime < doubleTouchDelay){  //더블터치판정
                        if(!isGamePaused)
                            Pause();
                    }
                    break;
                case TouchPhase.Ended:
                    lastTouchTime = Time.time;
                    break;
            }
        }
        if(Input.GetKeyDown(KeyCode.P))
            Pause();
    }

    void CreateObj(int obj){
        BoxCollider Sp = spawnPoint.GetComponent<BoxCollider>();
        Vector3 originSpPosition = spawnPoint.position;
        //콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_x = Sp.bounds.size.x;
        float range_z = Sp.bounds.size.z;

        range_x = Random.Range((range_x / 2) * -1, range_x / 2);
        range_z = Random.Range((range_z / 2) * -1, range_z / 2);
        Vector3 randomPosition = new Vector3(range_x, Random.Range(0,500), range_z);
        originSpPosition += randomPosition;

        if(obj == 1)
            spawnedPlayer = Instantiate(player, originSpPosition, Quaternion.identity);
        else{
            Instantiate(enemy[Random.Range(0,enemy.Length)], originSpPosition, Quaternion.identity);
            enemyCount++;
        }
    }

    public void DestroyPlayerAndSpawn(){
        Destroy(spawnedPlayer);
        CreateObj(1);
    }

    public void Resume(){
        Time.timeScale = 1f;
        pauseCam.SetActive(false);
        spawnedPlayer.GetComponent<PlayerMove>().audioSourceE.Play();
        spawnedPlayer.transform.GetChild(0).GetComponent<Camera>().enabled = true;
        isGamePaused = false;
    }
    void Pause(){
        Time.timeScale = 0f;
        spawnedPlayer.transform.GetChild(0).GetComponent<Camera>().enabled = false;
        spawnedPlayer.GetComponent<PlayerMove>().audioSourceE.Stop();
        pauseCam.SetActive(true);
        isGamePaused = true;
    }
    public void GotoLobby(){
        SceneManager.LoadScene("LobbyScene");
    }
    public void Exit(){
        Application.Quit();
    }
}
