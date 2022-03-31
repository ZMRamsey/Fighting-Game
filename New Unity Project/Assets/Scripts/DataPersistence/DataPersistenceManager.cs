using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{

    public static DataPersistenceManager instance { get; private set; }

    private OptionsData optionsData;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more than one data manager in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        this.optionsData = new OptionsData();
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {
        if(this.optionsData == null)
        {
            Debug.Log("No data was found. Initialising to default values");
            NewGame();
        }
    }

    private void OnApplicationQuit()
    {
            SaveGame();
    }
}
