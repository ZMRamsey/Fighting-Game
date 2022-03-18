using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    static GameLogic _logic;
    [SerializeField] GameObject _loadScreenCanvas;
    [SerializeField] GameObject _versusScreenCanvas;
    GameObject _screen;
    bool _hasLoaded;
    AsyncOperation _scene;

    public static GameLogic Get() {
        return _logic;
    }

    [SerializeField] GameSettings _settings;

    void Awake() {
        if (!_logic) {
            _logic = this;
        }
        else {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

    public GameSettings GetSettings() {
        return _settings;
    }

    void Update() {
        if(GlobalInputManager.Get().GetSubmitInput() && _hasLoaded) {
            if (_screen == _versusScreenCanvas) {
                _screen.GetComponent<LoadingScreen>().EndAnimation();
            }
            else {
                FinishLoad();
            }
            _hasLoaded = false;
        }

        //if (GlobalInputManager.Get().GetPauseInput()) {
        //    LoadScene("Base");
        //}
    }

    public async void LoadScene(string sceneName) {
        _screen = _loadScreenCanvas;

        if (sceneName == "Base") {
            _screen = _versusScreenCanvas;
        }

        _screen.SetActive(true);

        _scene = SceneManager.LoadSceneAsync(sceneName);
        _scene.allowSceneActivation = false;

        do {
            
        } while (_scene.progress < 0.9f);

        _screen.GetComponent<LoadingScreen>().OnLoaded();
        _hasLoaded = true;
    }

    public void FinishLoad() {
        if(_scene != null) {
            _scene.allowSceneActivation = true;
            _screen.SetActive(false);
        }
    }

    
}
