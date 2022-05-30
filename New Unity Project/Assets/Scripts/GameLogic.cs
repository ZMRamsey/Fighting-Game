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
    [SerializeField] GameObject _arcadeScreenCanvas;
    GameObject _screen;
    bool _hasLoaded;
    public bool _loadInToCharacterSelect;
    public GameType _type;
    public int _arcadeRoute = 0;
    public bool _devLocked = true;

    public bool _fullScreen = true;
    public Vector2 _resolution = new Vector2(1920,1080);

    public static GameLogic Get() {
        return _logic;
    }

    [SerializeField] GameSettings _settings;

    void Awake() {
        if (!_logic) {
            _logic = this;
            Screen.SetResolution((int)_resolution.x, (int)_resolution.y, _fullScreen);
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

        if (GlobalInputManager.Get().GetLockInput() && _devLocked)
        {
            _devLocked = false;
            Debug.Log("Unlocked");
        }
    }

    AsyncOperation _scene;

    public void LoadScene(string sceneName, string from, bool useVersus) {
        _screen = _type == GameType.arcade ? _arcadeScreenCanvas : _loadScreenCanvas;

        if (useVersus) {
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

    public void SetResolution(Vector2 res)
    {
        _resolution = res;
    }

    public Vector2 GetResolution()
    {
        return _resolution;
    }

    public void SetFullScreen(bool fullscreen)
    {
        _fullScreen = fullscreen;
    }

    public bool GetFullScreen()
    {
        return _fullScreen;
    }

    public void ArcadeStoreUser()
    {

    }

    public int GetArcadePoint()
    {
        //_arcadeRoute++;
        //if(_arcadeRoute > 6)
        //{
        //    _arcadeRoute = 1;
        //}
        //return _arcadeRoute -1;
        return _arcadeRoute;
    }

    public void IncreaseArcadeCount()
    {
        _arcadeRoute++;
    }

    public void ResetArcade()
    {
        _arcadeRoute = 0;
    }
}
