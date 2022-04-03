using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool _loadInToCharacterSelect;
    public GameType _type;

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
        _loadInToCharacterSelect = false;
    }

    public GameSettings GetSettings() {
        return _settings;
    }

    void Update() {
        if (GlobalInputManager.Get().GetSubmitInput() && _hasLoaded) {
            if (_screen == _versusScreenCanvas) {
                _screen.GetComponent<LoadingScreen>().EndAnimation();
            }
            else {
                FinishLoad();
            }
            _hasLoaded = false;
        }

        if(MainMenuSystem.Get() && _loadInToCharacterSelect) {
            CharacterSelectSystem.Get().SetGameType(_type);
            MainMenuSystem.Get().SkipToCharacterSelect();
            _loadInToCharacterSelect = false;
        }
    }

    AsyncOperation _scene;

    public void LoadScene(string sceneName, string from) {
        _screen = _loadScreenCanvas;

        if (sceneName == "Base") {
            _screen = _versusScreenCanvas;
        }


        _screen.SetActive(true);
        _screen.GetComponent<LoadingScreen>().Rebind();

        _scene = SceneManager.LoadSceneAsync(sceneName);
        _scene.allowSceneActivation = false;
        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress() {
        while (_scene.progress < 0.9f) {
            yield return null;
        }

        _screen.GetComponent<LoadingScreen>().OnLoaded();
        _hasLoaded = true;
    }


    public void FinishLoad() {
        if (_scene != null) {
            _scene.allowSceneActivation = true;
            _screen.SetActive(false);
        }
    }


}
