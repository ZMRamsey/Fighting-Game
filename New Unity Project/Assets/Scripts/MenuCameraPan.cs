using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraPan : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    Vector3 optionsMenuPos = new Vector3(-816.0f, 310.5f, 0f);
    Vector3 mainMenuPos = new Vector3(620.8455f,310.5f,0f);
    Vector3 defPos = new Vector3(620.8455f, 310.5f, 0f);
    Vector3 diff = new Vector3(-1.0f, 0f, 0f);

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
        //Debug.Log("Moving to Options: " + _movingToOptions);
        //Debug.Log("Moving to Main: " + _movingToMain);
        //Debug.Log("X: " + cam.transform.position.x);
        //if (_movingToOptions && !_movingToMain)
        //{
        //    cam.transform.position = Vector3.MoveTowards(cam.transform.position, optionsMenuPos, 800f * Time.deltaTime);
        //}
        //else if (_movingToMain && !_movingToOptions)
        //{
        //    cam.transform.position = Vector3.MoveTowards(cam.transform.position, mainMenuPos, 800f * Time.deltaTime);
        //}
        //else if(!_movingToOptions && !_movingToMain)
        //{
        //    cam.transform.position = Vector3.MoveTowards(cam.transform.position, defPos, 800f * Time.deltaTime);
        //}

        //if(cam.transform.position == optionsMenuPos)
        //{
        //    optionsObj.GetComponent<optionsRotatorCircle>().enabled = true;
        //}
        //else if(cam.transform.position == mainMenuPos)
        //{
        //    mainObj.GetComponent<rotaterCircle>().enabled = true;
        //}
        //else if (cam.transform.position == defPos)
        //{
        //    mainObj.GetComponent<rotaterCircle>().enabled = true;
        //}
        //else
        //{
        //    mainObj.GetComponent<rotaterCircle>().enabled = false;
        //    optionsObj.GetComponent<optionsRotatorCircle>().enabled = false;
        //}
        //cam.transform.position = Vector3.MoveTowards(cam.transform.position, optionsMenuPos, 800f * Time.deltaTime);
        cam.transform.position += diff;
    }
}
