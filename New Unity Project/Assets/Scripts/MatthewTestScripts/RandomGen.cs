using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGen : MonoBehaviour
{
    public GameObject[] characters;
    public int objNum;
    public int objCount;

    public void SelectRandomChar()
    {
        objNum = Random.Range(0, 2);
        objCount = 0;
        while (objCount < 2)
        {
            characters[objCount].SetActive(false);
            objCount += 1;
        }
        characters[objNum].SetActive(true);
    }
}
