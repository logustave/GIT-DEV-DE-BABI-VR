using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScren : MonoBehaviour
{
    public NetworkingPhoton networkingPhoton;

    public Text leftValue;
    public int leftV;
    public Text rightValue;
    public int rightV;
    public Text operation;

    public bool choose = false;

    public Color chooseButtonColor;
    public Color baseButtonColor;
    public Button[] leftAndRight;

    public Color addCo,sousCo;

    public void Start()
    {
        RecolorColorButton(leftAndRight[0]);
    }
    public void SwitchCurrentInput(bool whatChoose)
    {
        choose = whatChoose;
    }

    public void AnswerConfirm()
    {
        if (leftValue.text == null || rightValue.text == null)
            return;
        for (int i = 0; i < leftAndRight.Length; i++)
        {
            leftAndRight[i].image.color = baseButtonColor;
        }

        int currentOpCode = 0;
        int response = leftV - rightV;
        if (operation.text == "+")
        {
            currentOpCode = 1;
            response = leftV + rightV;
        }

       

        networkingPhoton.vrRpcManager.ShareAnswerRpc(currentOpCode, leftV,rightV,response);
        choose = false;
        leftValue.text = "";
        leftV = 0;
        rightV = 0;
        rightValue.text = "";

    }
    public void ChooseLeftOrRight(bool choosen)
    {
        choose = choosen;
    }

    public void RecolorColorButton(Button btn)
    {
        for (int i = 0; i < leftAndRight.Length; i++)
        {
            leftAndRight[i].image.color = baseButtonColor;
        }
        btn.image.color = chooseButtonColor;
    }

    public void ChangeQuestion(int num)
    {
        if (!choose)
        {
            leftValue.text = num.ToString();
            leftV = num;
        }
        else
        {
            rightValue.text = num.ToString();
            rightV = num;
        }
    }

    public void ChangeOperation(int codeOp)
    {
        if (codeOp == 0)
        {
            operation.text = "-";
            operation.GetComponentInParent<Image>().color = sousCo;
        }
        else
        { 
            operation.text = "+";
            operation.GetComponentInParent<Image>().color = addCo;
        }
    }
}
