using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    private List<IDataPersistence> dataPersistenceObjects;

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
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.optionsData = new OptionsData();
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref optionsData);
        }

        Debug.Log("Saved Music Volume = " + optionsData._musicVol);
        Debug.Log("Saved SFX Volume = " + optionsData._sfxVol);
    }

    public void LoadGame()
    {
        if(this.optionsData == null)
        {
            Debug.Log("No data was found. Initialising to default values");
            NewGame();
        }

        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(optionsData);
        }

        Debug.Log("Loaded Music Volume = " + optionsData._musicVol);
        Debug.Log("Loaded SFX Volume = " + optionsData._sfxVol);
    }

    private void OnApplicationQuit()
    {
            SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType < IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
