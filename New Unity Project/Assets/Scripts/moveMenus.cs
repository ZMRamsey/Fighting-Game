using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMenus : MonoBehaviour
{

    public GameObject obj;
    public MenuRacketRotator rot;


    Vector3 diff = new Vector3(1.0f,0f,0f);


    Vector3 optionsMenuPos = new Vector3(2074.669f, 745.2428f, 1.832399f);
    Vector3 mainMenuPos = new Vector3(645.894f, 745.2428f, 0f);
    Vector3 defPos = new Vector3(645.894f, 745.2428f, 0f);

    Vector3 testPos = new Vector3(700.0f, 700.0f, 1.832399f);

    public GameObject mainObj;
    public GameObject optionsObj;

    public bool _movingToOptions = false;
    public bool _movingToMain = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("X: " + obj.transform.position.x);
        //Debug.Log("Y: " + obj.transform.position.y);

        if (_movingToOptions && !_movingToMain)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, optionsMenuPos, 1600f * Time.deltaTime);
            rot._rotatorConstant = 180.0f;
        }
        else if (_movingToMain && !_movingToOptions)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, mainMenuPos, 1600f * Time.deltaTime);
            rot._rotatorConstant = 0.0f;
        }
        else if (!_movingToOptions && !_movingToMain)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, defPos, 1600f * Time.deltaTime);
            rot._rotatorConstant = 0.0f;
        }

        if (obj.transform.position == optionsMenuPos)
        {
            optionsObj.GetComponent<optionsRotatorCircle>().enabled = true;
        }
        else if (obj.transform.position == mainMenuPos)
        {
            mainObj.GetComponent<rotaterCircle>().enabled = true;
        }
        else if (obj.transform.position == defPos)
        {
            mainObj.GetComponent<rotaterCircle>().enabled = true;
        }
        else
        {
            mainObj.GetComponent<rotaterCircle>().enabled = false;
            optionsObj.GetComponent<optionsRotatorCircle>().enabled = false;
        }
    }
}
