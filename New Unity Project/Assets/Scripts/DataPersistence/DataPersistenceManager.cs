using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;

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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
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

        dataHandler.Save(optionsData);
    }

    public void LoadGame()
    {
        this.optionsData = dataHandler.Load();
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
