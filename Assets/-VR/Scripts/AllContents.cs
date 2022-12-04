using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllContents : MonoBehaviour
{
    public GameObject instantiateObect;
    public List<OneAnswerContent> oneAnswerContents;

    public void NewAnswer(int idAns, string mss, int gd)
    {
        GameObject instance = Instantiate(instantiateObect, transform.position, Quaternion.identity, transform) as GameObject;
        OneAnswerContent curr = instance.GetComponent<OneAnswerContent>();
        oneAnswerContents.Add(curr);
        curr.NewAnswer(idAns, mss, gd);
    }

    public void ApplyAnswerInfo(int idA, int rep)
    {
        GetTheAnswer(idA).ShowInfoAnswer(rep);
    }

    OneAnswerContent GetTheAnswer(int id)
    {
        OneAnswerContent rtn = oneAnswerContents[0];
        for (int i = 0; i < oneAnswerContents.Count; i++)
        {
            if (oneAnswerContents[i].id == id)
                return oneAnswerContents[i];
        }
        return rtn;
    }
}
