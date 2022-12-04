using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public GameObject[] lesWagons;
    public int xx;

    public GameObject particule_Disable;

    void Start()
    {
        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Add();
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Sous();
        //}
    }

    public void Add()
    {
        for (int i = 0; i < xx; i++)
        {
            lesWagons[i].SetActive(true);
        }
    }

    public void Sous()
    {
     //   GameObject g = Instantiate(particule_Disable, lesWagons[xx].transform.position, lesWagons[xx].transform.rotation);
      //  g.transform.parent = null;
        lesWagons[xx].SetActive(false);
    }
}
