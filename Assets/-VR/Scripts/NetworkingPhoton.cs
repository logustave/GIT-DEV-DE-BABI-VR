using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkingPhoton : MonoBehaviourPunCallbacks
{
    public RpcManager vrRpcManager;
    public GameObject vrPlayer;
    public Camera vrCam;
    public GameObject vrInstance;

    public GameObject arInstance;
    public GameObject drone;

    public QuestionSelect questionSelect;
    public GameObject camAr;
    public GameObject educationPlace;

    public TrainManager[] trainmaner;

    public Transform spawnPoint;
    public Text debugInfo;
    public bool isInstitutor = true;
    public byte version = 1;
    public byte PlayerTtl = 200;

    public AllContents allContents;
    public List<string> answerList;
    public List<string> goodResponse;
    public List<int> responseValue;
    public int currentResponse;

    void Awake()
    {
        if (isInstitutor)
        {
            PlayerPrefs.SetInt("institutor", 1);
            vrPlayer.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("institutor", 0);
            vrPlayer.SetActive(false);
        }
    }
    private void Start()
    {
        PhotonConnect();
    }
    void Update()
    {
        if (debugInfo != null)
            debugInfo.text = PhotonNetwork.NetworkClientState.ToString() + "::" + PhotonNetwork.CountOfPlayersInRooms;
    }

    void PhotonConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.version.ToString();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "].");
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("devdebabi2", roomOptions, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonConnect();
        Debug.Log("error connect : " + message);
    }

    public override void OnJoinedRoom()
    {
        if (PlayerPrefs.GetInt("institutor") == 1)
        {
            GameObject instance = PhotonNetwork.Instantiate(vrInstance.name, spawnPoint.position, spawnPoint.rotation) as GameObject;
            if (vrCam != null)
                vrCam.enabled = true;
            if (educationPlace != null)
                educationPlace.SetActive(true);
            vrRpcManager = instance.GetComponent<RpcManager>();
            instance.GetComponent<RpcManager>().networkingPhoton = this;
        }
        else
        {
            GameObject instance = PhotonNetwork.Instantiate(arInstance.name, Vector3.zero, Quaternion.identity) as GameObject;
            instance.GetComponent<RpcManager>().networkingPhoton = this;
            questionSelect.gameObject.SetActive(true);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("connected:" + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("is_disconnect:" + otherPlayer.NickName);
    }
}
