using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FighterFilter { one, two, both, current, none };
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
    [SerializeField] AudioSource _endMusic;
    [SerializeField] UIFader _screenFader;
    [SerializeField] GameObject _debugCanvas;
    [SerializeField] GameObject _debugCamera;
    [SerializeField] GameEventManager _eventManager;
    [SerializeField] Animator _UIBars;
    [SerializeField] Animator _UIStage;
    [SerializeField] Image _pauseArt;
    [SerializeField] Image _pauseArtBack;
    [SerializeField] GameObject _pausePanel;
    GameEvent _lastSuperEvent;
    AudioSource _source;
    float _rotateTarget;
    int _rally;
    int _successive = 0;
    int _targetRally = 10;
    FighterFilter _lastHit = FighterFilter.both;
    public float _serveSpeed = 7.5f;
    public bool needNewRound = false;
    bool _isPaused;

    [SerializeField] bool _spinDown;

    [Header("Audio")]
    [SerializeField] AudioClip _endGameSFX;

    public static GameManager Get() {
        return _instance;
    }

    public GameEventManager GetEventManager() {
        return _eventManager;
    }

    void Awake() {
        _instance = this;
        _source = GetComponent<AudioSource>();

        InitializeGame();
    }

    void InitializeGame() {

        GameObject fOneObject = Instantiate(_settings.GetFighterOneProfile().GetPrefab(), _fighterOne.GetSpawn(), Quaternion.identity);
        GameObject fTwoObject = Instantiate(_settings.GetFighterTwoProfile().GetPrefab(), _fighterTwo.GetSpawn(), Quaternion.identity);

        _fighterOne.SetUpFighter(fOneObject.GetComponent<FighterController>(), _settings.GetFighterOneProfile().GetName());
        _fighterTwo.SetUpFighter(fTwoObject.GetComponent<FighterController>(), _settings.GetFighterTwoProfile().GetName());

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

        PerformCoinToss();

        _shuttle.SetOwner(_fighterOne.GetController());
        _shuttle.SetOwner(FighterFilter.both);

        //ScoreManager.Get().SetPlayerTypes(_settings.GetFighterOneProfile().GetName(), _settings.GetFighterTwoProfile().GetName());


    }

    private void Start() {
        SetUpGame();
    }

    float _pauseTime;
    void Update() {
        if (_isPaused) {
            if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame) {
                Time.timeScale = 1;
                _isPaused = false;
            }
        }

        if (((Gamepad.current != null && Gamepad.current.startButton.isPressed) || Keyboard.current.escapeKey.isPressed) && !_isPaused && KOCoroutine == null && EndGameCoroutine == null && stageCoroutine == null && impactCoroutine == null) {
            _pauseTime += Time.deltaTime * 2;
            if (_pauseTime >= 1) {
                _isPaused = true;
                _pauseTime = 0;
                Time.timeScale = 0;
            }
        }
        else {
            _pauseTime = 0;
        }


        _pauseArt.fillAmount = Mathf.Clamp(_pauseTime, 0, 1);
        _pauseArtBack.fillAmount = _pauseArt.fillAmount;
        _pausePanel.SetActive(_isPaused);

        _rotateTarget = Mathf.Lerp(_rotateTarget, _shuttle.GetSpeedPercent(), Time.deltaTime * 12);
        _speedRotator.eulerAngles = -Vector3.Slerp(new Vector3(0, 0, -75f), new Vector3(0, 0, 75f), _rotateTarget);

        if (Keyboard.current.rKey.wasPressedThisFrame) {
            SetUpGame();
        }

        if (Keyboard.current.f10Key.wasPressedThisFrame) {
            _debugCanvas.SetActive(!_debugCanvas.activeSelf);
            _debugCamera.SetActive(_debugCanvas.activeSelf);
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

        if (_shuttle.GetVelocity().magnitude < 0.005 && _shuttle.transform.position.y < 0.75001 && !_shuttle.IsFrozen() && KOCoroutine == null && EndGameCoroutine == null) {
            string scorer = "one";
            if (_shuttle.transform.position.x < 0) {
                scorer = "two";
            }
            ScoreManager.Get().UpdateScore(scorer, "GroundOut");
            SetUpGame();
        }

        if (NewRound() && KOCoroutine == null && EndGameCoroutine == null) {
            SetUpGame();
            NewRoundNeeded(false);
        }

        //if (_fighterOne.GetController().GetFighterAction() == FighterAction.dead || _fighterTwo.GetController().GetFighterAction() == FighterAction.dead)
        //{
        //    _fighterOne.GetController().SetFighterAction(FighterAction.none);
        //    _fighterTwo.GetController().SetFighterAction(FighterAction.none);
        //    KOEvent();
        //}
    }

    void SetUpGame() {

        if (ScoreManager.Get().gameOver != FighterFilter.both) {
            EndGame();
            return;
        }

        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        if (_lastSuperEvent != null) {
            _lastSuperEvent.DisableScreen();
        }

        _stageCamera.SetActive(true);
        _spinDown = false;

        _screenFader.SetAlpha(1);
        _shuttle.ResetShuttle(true);

        _fighterOne.GetController().transform.position = _fighterOne.GetSpawn();
        _fighterTwo.GetController().transform.position = _fighterTwo.GetSpawn();

        _fighterOne.GetController().ResetFighter();
        _fighterTwo.GetController().ResetFighter();

        _fighterOne.GetUI().SetBarValue(0.0f);
        _fighterTwo.GetUI().SetBarValue(0.0f);

        _fighterOne.UpdateScore(ScoreManager.Get().GetScores()[ScoreManager.Get().GetCurrentRound() - 1, 0]);
        _fighterTwo.UpdateScore(ScoreManager.Get().GetScores()[ScoreManager.Get().GetCurrentRound() - 1, 1]);


        if (ScoreManager.Get().GetLastScorer() == 0) {
            _shuttle.transform.position = new Vector3(_shuttleSpawn.x + 5, _shuttleSpawn.y, _shuttleSpawn.z);
            _shuttle.SetOwner(_fighterOne.GetController());
            //_shuttle.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.red);
        }
        else {
            _shuttle.transform.position = new Vector3(_shuttleSpawn.x - 5, _shuttleSpawn.y, _shuttleSpawn.z);
            _shuttle.SetOwner(_fighterTwo.GetController());
            //_shuttle.GetComponentInChildren<SpriteRenderer>().material.SetColor("OutlineColor", Color.blue);
        }
        _shuttle.SetOwner(FighterFilter.both);

        _rally = 0;
        _targetRally = 10;
        _shuttle.resetBounces();
        TimerManager.Get().ResetPointTimer();
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
        impactCoroutine = null;
    }

    public void StunFrames(float timer, FighterFilter filter) {
        if (filter == FighterFilter.one || filter == FighterFilter.both) {
            _fighterOne.GetController().StunController(timer);
            print("one");
        }
        if (filter == FighterFilter.two || filter == FighterFilter.both) {
            _fighterTwo.GetController().StunController(timer);
            print("two");
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
        if (KOCoroutine != null) {
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
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(0.2f);
        _eventManager.GetDarkness().DisableScreen();
        _stageCamera.SetActive(true);

        if (ScoreManager.Get().gameOver != FighterFilter.both) {
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
            //Time.fixedDeltaTime = 0.02f;
            yield return new WaitForSecondsRealtime(1f);
        }
        else {
            yield return new WaitForSecondsRealtime(4f);
            Time.timeScale = 1f;
            //Time.fixedDeltaTime = 0.02f;
            yield return new WaitForSecondsRealtime(4f);
        }
        SetUpGame();
        KOCoroutine = null;
    }

    IEnumerator EndGameProcess(FighterFilter winner, FighterFilter loser) {
        _UIStage.SetTrigger("Fade");
        _UIBars.SetBool("Show", true);
        _spinDown = true;

        var winController = _fighterOne.GetController();
        var loseController = _fighterOne.GetController();

        if (winner == FighterFilter.two) {
            winController = _fighterTwo.GetController();
        }

        if (loser == FighterFilter.two) {
            loseController = _fighterTwo.GetController();
        }

        winController.RemoveControl();
        loseController.RemoveControl();

        _source.PlayOneShot(_endGameSFX);

        yield return new WaitForSecondsRealtime(_endGameSFX.length / 2);
        _endMusic.Play();
        yield return new WaitForSecondsRealtime(0.5f);
        winController.PlayWin();
        loseController.PlayLose();

        CameraContoller.Get().SetFocus(winController.transform);

        yield return new WaitForSecondsRealtime(5f);
        CameraContoller.Get().SetFocus(loseController.transform);
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(sceneName: "WinScreenTest");
    }

    public FighterController GetFighterOne() {
        return _fighterOne.GetController();
    }

    public FighterController GetFighterTwo() {
        return _fighterTwo.GetController();
    }

    public FighterTab GetFighterTab(FighterFilter filter) {
        if (filter == FighterFilter.one) {
            return _fighterOne;
        }

        return _fighterTwo;
    }

    IEnumerator ServeTimer() {
        TimerManager.Get().SetTimerState(false);
        _rally = 0;
        _successive = 0;
        yield return new WaitForSeconds(1);
        if (_rally == 0 && _successive == 0) {
            _shuttle.ResetShuttle(true);
            var hitMes = new HitMessage(new Vector3(_shuttle.transform.position.x / -4f, _serveSpeed, 0f), new VelocityInfluence(), false, FighterFilter.both);
            _shuttle.Shoot(hitMes);
        }
        TimerManager.Get().SetTimerState(true);
    }

    public void IncreaseRally() {
        _rally++;
        _successive = 1;

        //if (_rally == _targetRally)
        //{
        //    _shuttle.increaseBounces();
        //    _targetRally += 10;
        //}
    }

    public void SuccessiveHit() {
        _successive++;
    }

    public void ResetSuccessive() {
        _successive = 0;
    }

    public void SetLastHitter(FighterFilter hitter) {
        _lastHit = hitter;
    }

    public FighterFilter GetLastHit() {
        return _lastHit;
    }

    public int GetCurrentRally() {
        return _rally;
    }

    public int GetSuccessive() {
        return _successive;
    }

    public Coroutine EndGameCoroutine;
    public void EndGame() {
        string p1Name = _settings.GetFighterOneProfile().GetName();
        string p2Name = _settings.GetFighterTwoProfile().GetName();

        //StatPrinter printer = new StatPrinter();

        //printer.RecordGame(0.0f, ScoreManager.Get().GetScores(), 1, 2, p1Name, p2Name);

        //Save data

        //FindObjectOfType<ResultsHolderScript>().SetData(p1Name, p2Name, ScoreManager.Get().gameOver.ToString());

        //Open end screen
        //SceneManager.LoadScene(sceneName: "WinScreenTest");

        if (EndGameCoroutine == null) {
            FighterFilter winner = ScoreManager.Get().DecideGameWinner();
            EndGameCoroutine = StartCoroutine(EndGameProcess(winner, ScoreManager.Get().GetLoser(winner)));
        }

    }

    public bool NewRound() {
        return needNewRound;
    }

    public void NewRoundNeeded(bool needed) {
        needNewRound = needed;
    }

    public GameSettings GetGameSettings() {
        return _settings;
    }

    public void PerformCoinToss() {
        int rand = Random.Range(1, 100);
        ScoreManager.Get().SetLastScorer(rand % 2);
    }

}
