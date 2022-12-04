using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneAnswerContent : MonoBehaviour
{
    public Text myText;
    public int id;
    public bool alreadyDo;
    public int goodR;
    public GameObject[] infoAnswers;
    public string message;
    public void NewAnswer(int idQ, string showAnswer, int goodDr)
    {
        id = idQ;
        myText.text = showAnswer + " = ?";
        message = showAnswer;
        goodR = goodDr;
    }

    public void ShowInfoAnswer(int result)
    {
        int codeT = 0;
        if (result == goodR)
            codeT = 1;
        infoAnswers[codeT].SetActive(true);
        myText.text = message + " = " +result; 
        alreadyDo = true;
    }
}
