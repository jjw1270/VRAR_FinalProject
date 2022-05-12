using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";  //게임 버전
    public Text connectionInfoText;   // 네트워크 정보를 표시할 텍스트
    public GameObject playButton;   //룸 입장 버튼
    public InputField playerNameInput;
    public string playerName = null;
    
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        playButton.SetActive(false);
        connectionInfoText.text = "서버 접속중...";
    }

    //서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        if(PlayerPrefs.GetString("playerName") != ""){   //저장된 이름이 있으면
            playerName = PlayerPrefs.GetString("playerName");
            playerNameInput.text = playerName;
            PhotonNetwork.LocalPlayer.NickName = playerName;
            playButton.SetActive(true);
        }
        else{

        }
        connectionInfoText.text = "접속 성공";
    }

    //서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸 입장버튼 비활성화
        playButton.SetActive(false);
        connectionInfoText.text = "서버와 연결되지 않음. 재접속 시도중..";
        //서버와 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    //랜덤 룸 참가 실패시 자동실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        //접속 상태 표시
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성중..";
        //최대 6명을 수용 가능한 빈 룸을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });
    }

    //룸에 참가 완료시 자동실행
    public override void OnJoinedRoom()
    {
        Debug.Log("방참가");
        base.OnJoinedRoom();
        //접속 상태 표시
        connectionInfoText.text = "방 참가 성공";
        //모든 룸 참가자들이 Main 씬을 로드하게 함
        PhotonNetwork.LoadLevel("MainScene");
    }

    public void Connect(){
        //중복접속시도 막음
        playButton.GetComponent<Button>().interactable = false;
        playButton.GetComponent<BoxCollider>().enabled = false;
        //서버에 접속중이면
        if(PhotonNetwork.IsConnected){
            //룸 접속 실행
            connectionInfoText.text = "방에 입장중..";
            PhotonNetwork.JoinRandomRoom();
        }
        else{
            //서버에 접속중이 아니면 접속 시도
            connectionInfoText.text = "서버와 연결되지 않음. 재접속 시도중..";
            //서버와 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void InputName(){
        playerNameInput.Select();
    }

    public void SaveName(){
        playerName = playerNameInput.text;
        if(playerName.Length > 0){
            PlayerPrefs.SetString("playerName", playerName);
            PlayerPrefs.Save();
            PhotonNetwork.LocalPlayer.NickName = playerName;
            connectionInfoText.text = "이름 저장완료";
            playButton.SetActive(true);
        }
    }
}