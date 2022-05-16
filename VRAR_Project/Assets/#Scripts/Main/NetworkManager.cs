using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Transform[] points;
    void Awake() {
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnJoinedRoom()   /////////중복!
    {
        base.OnJoinedRoom();
        Debug.Log("방 입장");
        CreatePlayer();
    }

    void CreatePlayer(){
        int sp = Random.Range(0, points.Length);
        BoxCollider randomSp = points[sp].GetComponent<BoxCollider>();
        Vector3 originSpPosition = points[sp].position;
        //콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_x = randomSp.bounds.size.x;
        float range_y = randomSp.bounds.size.y;
        float range_z = randomSp.bounds.size.z;

        range_x = Random.Range((range_x / 2) * -1, range_x / 2);
        range_y = Random.Range((range_y / 2) * -1, range_y / 2);
        range_z = Random.Range((range_z / 2) * -1, range_z / 2);

        PhotonNetwork.Instantiate("Player", new Vector3(range_x, range_y, range_z), Quaternion.identity);
    }
}
