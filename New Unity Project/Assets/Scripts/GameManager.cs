using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FighterFilter { one, two, both};
public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    [SerializeField] GameSettings _settings;
    [SerializeField] FighterTab _fighterOne;
    [SerializeField] FighterTab _fighterTwo;
    [SerializeField] Shaker _cameraShaker;
    [SerializeField] ShuttleCock _shuttle;
    [SerializeField] Vector3 _shuttleSpawn;
    [SerializeField] Transform _speedRotator;
    [SerializeField] GameObject _canvasObject;
    [SerializeField] GameObject _stageCamera;
    [SerializeField] AudioClip _superSFX;
    [SerializeField] AudioSource _music;
    [SerializeField] UIFader _screenFader;
    AudioSource _source;
    float _rotateTarget;

    public static GameManager Get() {
        return _instance;
    }

    void Awake()
    {
        _instance = this;
        _source = GetComponent<AudioSource>();

        InitializeGame();
    }

    void InitializeGame() {
        GameObject fOneObject = Instantiate(_settings.GetFighterOneProfile().GetPrefab(), _fighterOne.GetSpawn(), Quaternion.identity);
        GameObject fTwoObject = Instantiate(_settings.GetFighterTwoProfile().GetPrefab(), _fighterOne.GetSpawn(), Quaternion.identity);

        _fighterOne.SetControler(fOneObject.GetComponent<FighterController>());
        _fighterTwo.SetControler(fTwoObject.GetComponent<FighterController>());

        if (_settings.GetFighterTwoState() != InputState.player) {
            if (_settings.GetFighterTwoState() == InputState.ai) {
                fTwoObject.AddComponent<AIBrain>();
            }
            fTwoObject.GetComponent<InputHandler>().SetInputState(InputState.ai);
        }

        _fighterOne.GetController().SetFilter(FighterFilter.one);
        _fighterTwo.GetController().SetFilter(FighterFilter.two);

        _fighterOne.GetController().transform.position = _fighterOne.GetSpawn();
        _fighterTwo.GetController().transform.position = _fighterTwo.GetSpawn();
    }

    private void Start() {
        SetUpGame();
    }


    void Update()
    {
        _rotateTarget = Mathf.Lerp(_rotateTarget, _shuttle.GetSpeedPercent(), Time.deltaTime * 12);
        _speedRotator.eulerAngles = -Vector3.Lerp(new Vector3(0,0, -75f), new Vector3(0, 0, 75f), _rotateTarget);

        if (Keyboard.current.rKey.wasPressedThisFrame) {
            SetUpGame();
        }
    }

    void SetUpGame() {
        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        _canvasObject.SetActive(false);
        _stageCamera.SetActive(true);
        _music.pitch = 1f;

        _screenFader.SetAlpha(1);

        _shuttle.ResetShuttle();

        _fighterOne.GetController().transform.position = _fighterOne.GetSpawn();
        _fighterTwo.GetController().transform.position = _fighterTwo.GetSpawn();
        _shuttle.transform.position = _shuttleSpawn;
    }

    public Shaker GetCameraShaker() {
        return _cameraShaker;
    }

    Coroutine stageCoroutine;
    public void OnSpecial() {
        StunFrames(1f, FighterFilter.both);
        _shuttle.FreezeShuttle();
        _source.PlayOneShot(_superSFX);


        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        stageCoroutine = StartCoroutine(StageFlash(1));
    }

    public void StunFrames(float timer, FighterFilter filter) {
        if (filter == FighterFilter.one || filter == FighterFilter.both) {
            _fighterOne.GetController().StunController(timer);
        }
        if (filter == FighterFilter.two || filter == FighterFilter.both) {
            _fighterTwo.GetController().StunController(timer);
        }
    }

    public ShuttleCock GetShuttle() {
        return _shuttle;
    }

    IEnumerator StageFlash(float time) {
        _music.pitch = 0.8f;
        _canvasObject.SetActive(true);
        _stageCamera.SetActive(false);
        yield return new WaitForSeconds(time);
        _canvasObject.SetActive(false);
        _stageCamera.SetActive(true);
        _music.pitch = 1f;
    }

    public FighterController GetFighterOne() {
        return _fighterOne.GetController();
    }

    public FighterController GetFighterTwo() {
        return _fighterTwo.GetController();
    }

    public FighterTab GetFighterTab(FighterFilter filter) {
        if(filter == FighterFilter.one) {
            return _fighterOne;
        }

        return _fighterTwo;
    }
}
