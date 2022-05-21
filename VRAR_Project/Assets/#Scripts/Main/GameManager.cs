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
    public static int enemyCount = 0;
    GameObject spawnedPlayer;
    public Transform playerSp;
    private float lastTouchTime;
    private bool IsOneClick = false;
    private const float doubleTouchDelay = 0.5f;
    public static bool isGamePaused = false;
    public GameObject pauseCam;
    public static int gameScore = 0;
    public static int lifeCount = 3;
    public GameObject gameOverTitle;
    public Text gameOverScore;
    public GameObject returnToGameBtn;

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
            PlayerMove.isDestroy = false;
            if(lifeCount == 0) {
                //목숨3개!
                //게임오버 UI(일시정지 UI) + 스코어
                Invoke("GameOver", 2f);
            }
            else
                Invoke("DestroyPlayerAndSpawn", 2f);
        }

        //더블 터치 시 일시정지 + UI
        if(Input.GetMouseButtonDown(0)){
            if(!IsOneClick){
                lastTouchTime = Time.time;
                IsOneClick = true;
            }
            else if(IsOneClick && ((Time.time - lastTouchTime) < doubleTouchDelay)){
                //더블터치
                IsOneClick = false;
                Pause();
            }
        }
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
            spawnedPlayer = Instantiate(player, playerSp.position, Quaternion.Euler(new Vector3(0,0,1)));
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

    void GameOver(){
        Time.timeScale = 0f;
        spawnedPlayer.transform.GetChild(0).GetComponent<Camera>().enabled = false;
        spawnedPlayer.GetComponent<PlayerMove>().audioSourceE.Stop();
        pauseCam.SetActive(true);
        gameOverTitle.SetActive(true);
        returnToGameBtn.SetActive(false);
        gameOverScore.gameObject.SetActive(true);
        gameOverScore.text = gameScore.ToString();
    }
}