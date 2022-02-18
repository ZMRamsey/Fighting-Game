using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum FighterFilter { one, two, both};
public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    [SerializeField] GameSettings _settings;
    [SerializeField] FighterTab _fighterOne;
    [SerializeField] FighterTab _fighterTwo;
    [SerializeField] Shaker _cameraShaker;
    [SerializeField] Shaker _stageShaker;
    [SerializeField] ShuttleCock _shuttle;
    [SerializeField] Vector3 _shuttleSpawn;
    [SerializeField] Transform _speedRotator;
    [SerializeField] GameObject _stageCamera;
    [SerializeField] AudioClip _superSFX;
    [SerializeField] AudioSource _music;
    [SerializeField] UIFader _screenFader;
    [SerializeField] GameObject _debugCanvas;
    [SerializeField] GameEventManager _eventManager;
    GameEvent _lastSuperEvent;
    AudioSource _source;
    float _rotateTarget;
    int _rally;
    int _successive = 1;
    FighterFilter _lastHit = FighterFilter.both;
    public float _serveSpeed = 7.5f;

    [SerializeField] bool _spinDown;

    public static GameManager Get() {
        return _instance;
    }

    public GameEventManager GetEventManager() {
        return _eventManager;
    }

    void Awake()
    {
        _instance = this;
        _source = GetComponent<AudioSource>();

        InitializeGame();
    }

    void InitializeGame() {

        GameObject fOneObject = Instantiate(_settings.GetFighterOneProfile().GetPrefab(), _fighterOne.GetSpawn(), Quaternion.identity);
        GameObject fTwoObject = Instantiate(_settings.GetFighterTwoProfile().GetPrefab(), _fighterTwo.GetSpawn(), Quaternion.identity);

        _fighterOne.SetUpFighter(fOneObject.GetComponent<FighterController>(), _settings.GetFighterOneProfile().GetName());
        _fighterTwo.SetUpFighter(fTwoObject.GetComponent<FighterController>(), _settings.GetFighterOneProfile().GetName());

        if (_settings.GetFighterTwoState() != InputState.player) {
            if (_settings.GetFighterTwoState() == InputState.ai) {
                fTwoObject.AddComponent<AIBrain>();
                fTwoObject.GetComponent<InputHandler>().SetInputState(InputState.ai);
            }
        }
        else {
            fTwoObject.GetComponent<InputHandler>().SetDevice(_settings.GetFighterTwoDevice());
        }

        if (_settings.GetFighterOneState() != InputState.player) {
            if (_settings.GetFighterOneState() == InputState.ai) {
                fOneObject.AddComponent<AIBrain>();
                fOneObject.GetComponent<InputHandler>().SetInputState(InputState.ai);
            }
        }
        else {
            fOneObject.GetComponent<InputHandler>().SetDevice(_settings.GetFighterOneDevice());
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

        if (Keyboard.current.f10Key.wasPressedThisFrame) {
            _debugCanvas.SetActive(!_debugCanvas.activeSelf);
        }

        if (Keyboard.current.f11Key.wasPressedThisFrame) {
            KOEvent();
        }


        if (_spinDown) {
            _music.pitch *= 0.99f;
        }
        else {
            if (_music.pitch < 1 && stageCoroutine == null) {
                _music.pitch += Time.deltaTime;

                if (_music.pitch > 1) {
                    _music.pitch = 1;
                }
            }
        }

        if (_shuttle.GetVelocity().magnitude < 0.005 && _shuttle.transform.position.y < 0.75001 && !_shuttle.IsFrozen() && KOCoroutine == null)
        {
            string scorer = "one";
            if (_shuttle.transform.position.x < 0)
            {
                scorer = "two";
            }
            ScoreManager.Get().UpdateScore(scorer, "GroundOut");
            SetUpGame();
        }

        //if (_fighterOne.GetController().GetFighterAction() == FighterAction.dead || _fighterTwo.GetController().GetFighterAction() == FighterAction.dead)
        //{
        //    _fighterOne.GetController().SetFighterAction(FighterAction.none);
        //    _fighterTwo.GetController().SetFighterAction(FighterAction.none);
        //    KOEvent();
        //}
    }

    void SetUpGame() {
        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        if (_lastSuperEvent != null) {
            _lastSuperEvent.DisableScreen();
        }

        _stageCamera.SetActive(true);
        _spinDown = false;

        _screenFader.SetAlpha(1);

        _shuttle.ResetShuttle();

        _fighterOne.GetController().transform.position = _fighterOne.GetSpawn();
        _fighterTwo.GetController().transform.position = _fighterTwo.GetSpawn();

        _fighterOne.GetController().InitializeFighter();
        _fighterTwo.GetController().InitializeFighter();

        _fighterOne.GetUI().SetBarValue(0.0f);
        _fighterTwo.GetUI().SetBarValue(0.0f);

        _fighterOne.UpdateScore(ScoreManager.Get().GetScores()[ScoreManager.Get().GetCurrentRound()-1,0]);
        _fighterTwo.UpdateScore(ScoreManager.Get().GetScores()[ScoreManager.Get().GetCurrentRound()-1,1]);


        if (ScoreManager.Get().GetLastScorer() == 0)
        {
            _shuttle.transform.position = new Vector3 (_shuttleSpawn.x + 5, _shuttleSpawn.y, _shuttleSpawn.z);
            _shuttle.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.red);
        }
        else
        {
            _shuttle.transform.position = new Vector3(_shuttleSpawn.x - 5, _shuttleSpawn.y, _shuttleSpawn.z);
            _shuttle.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.blue);
        }
        _rally = 0;
        SetLastHitter(FighterFilter.both);
        //_shuttle.transform.position = _shuttleSpawn;
        StartCoroutine("ServeTimer");
    }

    public Shaker GetCameraShaker() {
        return _cameraShaker;
    }

    public Shaker GetStageShaker() {
        return _stageShaker;
    }

    Coroutine stageCoroutine;
    public void OnSpecial(GameEvent gEvent, FighterFilter filter) {
        StunFrames(1f, FighterFilter.both);
        _shuttle.FreezeShuttle(1.0f);
        _source.PlayOneShot(_superSFX);


        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        stageCoroutine = StartCoroutine(StageFlash(1, gEvent, filter));
    }

    Coroutine impactCoroutine;
    public void OnImpactFrame(float time) {
        if (impactCoroutine != null) {
            StopCoroutine(impactCoroutine);
        }

        impactCoroutine = StartCoroutine(ImpactFrameProcess(time));
    }

    IEnumerator ImpactFrameProcess(float time) {
        _eventManager.GetImpactFlash().EnableScreen();
        _stageCamera.SetActive(false);
        yield return new WaitForSeconds(time);
        _eventManager.GetImpactFlash().DisableScreen();
        _stageCamera.SetActive(true);
        stageCoroutine = null;
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

    IEnumerator StageFlash(float time, GameEvent gEvent, FighterFilter filter) {
        gEvent.SetOrientation(filter);
        _music.pitch = -1f;
        gEvent.EnableScreen();
        _lastSuperEvent = gEvent;
        _stageCamera.SetActive(false);
        yield return new WaitForSeconds(time);
        gEvent.DisableScreen();
        _stageCamera.SetActive(true);
        _music.pitch = 1f;
        stageCoroutine = null;
    }

    public Coroutine KOCoroutine;
    public void KOEvent() {
        if(KOCoroutine != null) {
            return;
        }

        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        if (impactCoroutine != null) {
            StopCoroutine(impactCoroutine);
        }

        _eventManager.DisableAll();

        KOCoroutine = StartCoroutine(KOProcess());
    }

    IEnumerator KOProcess() {
        _eventManager.GetDarkness().EnableScreen();
        _stageCamera.SetActive(false);

        _fighterOne.GetController().StunController(1f);
        _fighterTwo.GetController().StunController(1f);

        _shuttle.FreezeShuttle(1.0f);
        _spinDown = true;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(0.2f);
        _eventManager.GetDarkness().DisableScreen();
        _stageCamera.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        yield return new WaitForSecondsRealtime(4f);
        SetUpGame();
        KOCoroutine = null;
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

    IEnumerator ServeTimer()
    {
        yield return new WaitForSeconds(1);
        if (_rally == 0)
        {
            _shuttle.Shoot(new Vector3(_shuttle.transform.position.x / -4f, _serveSpeed, 0f), new Vector3(), false, false, FighterFilter.both);
        }
    }

    public void IncreaseRally()
    {
        _rally++;
        _successive = 1;
    }

    public void SuccessiveHit()
    {
        _successive++;
    }

    public void SetLastHitter(FighterFilter hitter)
    {
        _lastHit = hitter;
    }

    public FighterFilter GetLastHit()
    {
        return _lastHit;
    }

    public int GetCurrentRally()
    {
        return _rally;
    }

    public int GetSuccessive()
    {
        return _successive;
    }
}
