using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject player;
    GameObject spawnedPlayer;

    private float lastTouchTime;
    private const float doubleTouchDelay = 0.5f;
    public static bool isGamePaused = false;
    public GameObject pauseCam;
    public int lifeCount = 3;

    void Start()
    {
        lastTouchTime = Time.time;
        CreatePlayer();
    }

    void Update(){
        if(lifeCount == 0) {
            //목숨3개!
            //게임오버 UI(일시정지 UI) + 스코어
        }

        if(PlayerMove.isDestroy){
            PlayerMove.isDestroy = false;
            Invoke("DestroyPlayer", 3f);
        }

        //더블 터치 시 일시정지 + UI
        if(Input.touchCount == 1){
            Touch touch = Input.GetTouch(0);
            switch(touch.phase){
                case TouchPhase.Began:
                    if(Time.time - lastTouchTime < doubleTouchDelay){  //더블터치판정
                        if(isGamePaused)
                            Resume();
                        else
                            Pause();
                    }
                    break;
                case TouchPhase.Ended:
                    lastTouchTime = Time.time;
                    break;
            }
        }
    }

    void CreatePlayer(){
        int sp = Random.Range(0, points.Length);
        BoxCollider randomSp = points[sp].GetComponent<BoxCollider>();
        Vector3 originSpPosition = points[sp].position;
        //콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_x = randomSp.bounds.size.x;
        float range_z = randomSp.bounds.size.z;

        range_x = Random.Range((range_x / 2) * -1, range_x / 2);
        range_z = Random.Range((range_z / 2) * -1, range_z / 2);

        spawnedPlayer = Instantiate(player, new Vector3(range_x, 1100, range_z), Quaternion.identity);
    }

    public void DestroyPlayer(){
        lifeCount-=1;
        Destroy(spawnedPlayer);
        CreatePlayer();
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
