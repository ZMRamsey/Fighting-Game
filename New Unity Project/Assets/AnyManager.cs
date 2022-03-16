using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyManager : MonoBehaviour
{

    public static AnyManager anyManager;

    bool gameStart;
    private void Awake()
    {
        if (!gameStart)
        {
            anyManager = this;
            gameStart = true;
        }
    }

    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }

    IEnumerator Unload(int scene)
    {
        yield return null;

        SceneManager.UnloadSceneAsync(scene);
    }
}
