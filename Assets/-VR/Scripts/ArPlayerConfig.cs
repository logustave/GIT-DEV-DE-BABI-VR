using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ArPlayerConfig : MonoBehaviour
{
    public int currentScore = 2;
    RpcManager rpcManager;
    public float baseTimeForLoosePoint = 15f;
    public float currentTimeForLoose;

    public bool currently = false;
    public int goodResponse;
    public List<int> allResponse;
    public int currentId;

    private void Awake()
    {
        currentScore = 5;
        rpcManager = GetComponent<RpcManager>();
    }
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.A))
            rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("showpannel");
    }
    public void RepondreQuestion(int goodRes, int idRes, string question)
    {
        goodResponse = goodRes;
        allResponse = new List<int>();
        rpcManager.networkingPhoton.questionSelect.secondTxt.text = $"{goodRes} :: {idRes} :: {question}";
        int whenApparear = Random.Range(0, 2);
        List<int> listresult = new List<int> {-2,-5,-5,-8,10,11,12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        if (listresult.Contains(goodResponse))
            listresult.Remove(goodResponse);
        rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("showpannel");
        for (int i = 0; i <= 2; i++)
        {
            if (i == whenApparear)
                allResponse.Add(goodRes);
            else
            {
                int stc = Random.Range(0, listresult.Count);
                listresult.Remove(stc);
                allResponse.Add(stc + goodRes);
            }
        }
        rpcManager.networkingPhoton.questionSelect.infoCalcul.text = "Combien font :\n" + question;
        for (int i = 0; i < rpcManager.networkingPhoton.questionSelect.buttonInputAuizs.Count; i++)
        {
            rpcManager.networkingPhoton.questionSelect.buttonInputAuizs[i].ShowMe(allResponse[i]);
            rpcManager.networkingPhoton.questionSelect.buttonInputAuizs[i].pl = this;
        }
        currentTimeForLoose = baseTimeForLoosePoint;
        currentId = idRes;
        currently = true;
    }
    public void Update()
    {
        if(currently)
            if(currentTimeForLoose > 0)
            {
                rpcManager.networkingPhoton.questionSelect.imgTime.fillAmount = currentTimeForLoose / baseTimeForLoosePoint;
                currentTimeForLoose -= 1 * Time.deltaTime;
            }else
            {
                currently = false;
                baseTimeForLoosePoint = 0;
                LooseForce();
            }
    }
    public void LooseForce()
    {
        currentScore -= 1;
        for (int i = 0; i < rpcManager.networkingPhoton.trainmaner.Length; i++)
        {
            rpcManager.networkingPhoton.trainmaner[i].xx = currentScore;
            rpcManager.networkingPhoton.trainmaner[i].Sous();
        }
        rpcManager.ShareCurrentTrain(currentScore, 0);
        rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("hidepannel");
        if (currentScore > 0)
            rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("no");
        else
        {
            rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("def");
            return;
        }
        rpcManager.ShareResponse(currentId, -20);
        CheckIfDispo();
    }
    public void CheckIfDispo()
    {
        rpcManager.CheckAndGoAnswer();
    }
    public void GiveResponse(int response)
    {
        currently = false;
        if(response == goodResponse)
        {
            currentScore += 1;
            for (int i = 0; i < rpcManager.networkingPhoton.trainmaner.Length; i++)
            {
                rpcManager.networkingPhoton.trainmaner[i].xx = currentScore;
                rpcManager.networkingPhoton.trainmaner[i].Add();
            }
            rpcManager.ShareCurrentTrain(currentScore, 1);
            rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("yes");
        }
        else
        {
            currentScore -= 1;
            for (int i = 0; i < rpcManager.networkingPhoton.trainmaner.Length; i++)
            {
                rpcManager.networkingPhoton.trainmaner[i].xx = currentScore;
                rpcManager.networkingPhoton.trainmaner[i].Sous();
            }
            rpcManager.ShareCurrentTrain(currentScore, 0);
            if (currentScore > 0)
                rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("no");
            else
            {                 
                rpcManager.networkingPhoton.questionSelect.anim.SetTrigger("def");
                return;
            }
        }
        rpcManager.ShareResponse(currentId, response);
        CheckIfDispo();
    }
    
    
}
