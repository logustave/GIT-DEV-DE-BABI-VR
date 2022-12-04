using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RpcManager : MonoBehaviour
{
    public PhotonView view;
    public ArPlayerConfig arPlayerConfig;
    public NetworkingPhoton networkingPhoton;
    void awake()
    {
        if (networkingPhoton == null)
        {
            networkingPhoton = GameObject.FindObjectOfType<NetworkingPhoton>();
        }
        if (view == null)
            view = GetComponent<PhotonView>();
        if (arPlayerConfig == null)
        {
            try
            {
                arPlayerConfig = GetComponent<ArPlayerConfig>();
            }
            catch { Debug.Log("that's the instructor"); }
        }
    }

    private void Start()
    {
        if (networkingPhoton == null)
        {
            networkingPhoton = GameObject.FindObjectOfType<NetworkingPhoton>();
        }
        if (PlayerPrefs.GetInt("institutor") == 1)
        {
            if (!view.IsMine)
            {
                GoodDronePlacement();
                networkingPhoton.camAr.gameObject.SetActive(false);
            }
        }
        else
        {
            if (view.IsMine)
            {
                networkingPhoton.drone.gameObject.SetActive(false);
                transform.parent = networkingPhoton.camAr.transform;
                transform.localPosition = new Vector3();
                transform.localRotation = new Quaternion();
            }
        }
    }
    void GoodDronePlacement()
    {
        networkingPhoton.drone.transform.parent = transform;
        networkingPhoton.drone.transform.localPosition = new Vector3();
        networkingPhoton.drone.transform.localRotation = new Quaternion();
    }
    public void ShareAnswerRpc(int oper, int leftVal, int rightVal, int responsey)
    {
        view.RPC("RpcAnswer", RpcTarget.All, view.ViewID, oper, leftVal, rightVal, responsey);
    }

    public void ShareResponse(int cuResponse, int resp)
    {
        view.RPC("RpcResponse", RpcTarget.All, view.ViewID, cuResponse, resp);
    }

    public void ShareCurrentTrain(int currentLife, int type)
    {
        view.RPC("RpccTrain", RpcTarget.All, currentLife, type);
    }

    [PunRPC]
    public void RpcAnswer(int viewId, int op, int lv, int rv, int resp)
    {
        string operation = "-";
        if (op == 1)
            operation = "+";

        string answer = $"{lv} {operation} {rv}";
        networkingPhoton.answerList.Add(answer);
        networkingPhoton.goodResponse.Add(resp.ToString());
        networkingPhoton.allContents.NewAnswer(networkingPhoton.currentResponse, answer, resp);
        networkingPhoton.currentResponse += 1;

        PhotonView[] photonviews = GameObject.FindObjectsOfType<PhotonView>();

        for (int i = 0; i < photonviews.Length; i++)
        {
            if (photonviews[i].ViewID == view.ViewID)
            {
                if (!view.IsMine)
                {
                    CheckAndGoAnswer();
                    networkingPhoton.questionSelect.debugTxt.text = answer + ";;";
                }
            }
        }
    }
    public void CheckAndGoAnswer()
    {
        ArPlayerConfig ar = GameObject.FindObjectOfType<ArPlayerConfig>();

        if (ar.currently == false)
        {
            OneAnswerContent free = GetFreeoneAnswerContent();
            if (free != null)
                ar.RepondreQuestion(free.goodR, free.id, free.message);
        }

    }
    OneAnswerContent GetFreeoneAnswerContent()
    {
        OneAnswerContent rtn = null;
        for (int i = 0; i < networkingPhoton.allContents.oneAnswerContents.Count; i++)
        {
            if (!networkingPhoton.allContents.oneAnswerContents[i].alreadyDo)
                return networkingPhoton.allContents.oneAnswerContents[i];
        }
        return rtn;
    }
    [PunRPC]
    public void RpcResponse(int viewId, int opId, int resp)
    {
        PhotonView[] photonviews = GameObject.FindObjectsOfType<PhotonView>();

        for (int i = 0; i < photonviews.Length; i++)
        {
            if (photonviews[i].ViewID == view.ViewID)
            {
                if (!view.IsMine)
                {
                    networkingPhoton.allContents.ApplyAnswerInfo(opId, resp);
                }
            }
        }
    }
    [PunRPC]
    public void RpccTrain(int currentLife, int typ)
    {
        try
        {
            PhotonView[] photonviews = GameObject.FindObjectsOfType<PhotonView>();
            for (int i = 0; i < photonviews.Length; i++)
            {
                if (photonviews[i].ViewID == view.ViewID)
                {
                    if (!view.IsMine)
                    {
                        if (typ == 1)
                        {
                            for (int j = 0; j < networkingPhoton.trainmaner.Length; j++)
                            {
                                networkingPhoton.trainmaner[j].xx = currentLife;
                                networkingPhoton.trainmaner[j].Add();
                            }
                        }
                        else
                        {
                            for (int j = 0; j < networkingPhoton.trainmaner.Length; j++)
                            {
                                networkingPhoton.trainmaner[j].xx = currentLife;
                                networkingPhoton.trainmaner[j].Sous();
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
}
