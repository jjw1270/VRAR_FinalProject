using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Text connectionInfoText;   //정보를 표시할 텍스트
    public GameObject playButton;   //룸 입장 버튼
    public InputField playerNameInput;
    public string playerName = null;
    

    void Start()
    {
        playButton.SetActive(false);
        connectionInfoText.text = "닉네임을 입력하세요";

        if(PlayerPrefs.GetString("playerName") != ""){   //저장된 이름이 있으면
            playerName = PlayerPrefs.GetString("playerName");
            playerNameInput.text = playerName;
            connectionInfoText.text = playerName + "님 환영합니다";
            playButton.SetActive(true);
        }
    }

    void Update(){

    }

    public void PlayGame(){
        playButton.GetComponent<Button>().interactable = false;
        Debug.Log("방참가");
        connectionInfoText.text = "게임을 시작합니다";
        SceneManager.LoadScene("MainScene");
    }

    public void InputName(){
        playerNameInput.Select();
        TouchScreenKeyboard.Open("",TouchScreenKeyboardType.Default,false,false,false,false);
    }

    public void SaveName(){
        playerName = playerNameInput.text;
        if(playerName.Length > 0){
            PlayerPrefs.SetString("playerName", playerName);
            PlayerPrefs.Save();
            connectionInfoText.text = "이름 저장완료";
            playButton.SetActive(true);
        }
    }
}