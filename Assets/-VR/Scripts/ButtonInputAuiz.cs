using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInputAuiz : MonoBehaviour
{
    public Text showTxt;
    public int value;
    public ArPlayerConfig pl;
    public QuestionSelect questionSelect;
    public void ShowMe(int val)
    {
        GetComponent<Button>().interactable = true;
        value = val;
        showTxt.text = val.ToString();
    }

    public void SelectMe()
    {
        questionSelect.anim.SetTrigger("hidepannel");
        for (int i = 0; i < questionSelect.buttonInputAuizs.Count; i++)
        {
            questionSelect.buttonInputAuizs[i].GetComponent<Button>().interactable = false;
        }
        pl.GiveResponse(value);
    }
}
