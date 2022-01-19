using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGen : MonoBehaviour
{
    public Sprite spriteDeselected;
    public Sprite spriteSelected;

    public GameObject[] characters;
    public GameObject[] buttons;
    public int objNum;
    public int objCount;

    public void SelectRandomChar()
    {
        objNum = Random.Range(0, 2);
        objCount = 0;
        while (objCount < 2)
        {
            characters[objCount].SetActive(false);

            buttons[objCount].GetComponent<Image>().sprite = spriteDeselected;
            objCount += 1;
        }
        characters[objNum].SetActive(true);
        buttons[objNum].GetComponent<Image>().sprite = spriteSelected;
    }
}
