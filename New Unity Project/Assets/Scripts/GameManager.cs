using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public enum FighterFilter { one, two, both, current, none };
public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    [SerializeField] GameSettings _settings;
    [SerializeField] FighterTab _fighterOne;
    [SerializeField] FighterTab _fighterTwo;
    [SerializeField] GameEventManager _eventManager;
    [SerializeField] GameObject _winScreen;

    [Header("Shuttle Settings")]
    [SerializeField] ShuttleCock _shuttle;
    [SerializeField] Vector3 _shuttleSpawn;
    

    [Header("Aesthetics")]
    [SerializeField] Shaker _cameraShaker;
    [SerializeField] Shaker _stageShaker;
    [SerializeField] StageNet _net;

    [Header("Speedometer")]
    [SerializeField] Transform _speedRotator;

    [Header("Outlines")]
    [SerializeField] Material _redMaterial;
    [SerializeField] Material _blueMaterial;
    [SerializeField] Material _yellowMaterial;

    [Header("Shadows")]
    [SerializeField] CircleShadow _fighterOneShadow;
    [SerializeField] CircleShadow _fighterTwoShadow;
    [SerializeField] CircleShadow _shuttleShadow;

    [Header("UI")]
    [SerializeField] GameObject _debugCanvas;
    [SerializeField] UIFader _screenFader;
    [SerializeField] Animator _UIBars;
    [SerializeField] Animator _UIStage;
    [SerializeField] Animator _UIScore;
    [SerializeField] Animator _UICurrentSet;
    [SerializeField] Animator _UIHealthBars;
    [SerializeField] TextMeshProUGUI _UISetWhite;
    [SerializeField] TextMeshProUGUI _UISetBlack;
    [SerializeField] TextMeshProUGUI _UIScoreText;
    [SerializeField] TextMeshProUGUI _UIScoreF1;
    [SerializeField] TextMeshProUGUI _UIScoreF2;

    [Header("Cameras")]
    [SerializeField] GameObject _stageCamera;
    [SerializeField] GameObject _uiCamera;
    [SerializeField] GameObject _debugCamera;

    [Header("Pause")]
    [SerializeField] Image _pauseArt;
    [SerializeField] Image _pauseArtBack;
    [SerializeField] PauseMenu _pauseController;

    [Header("Audio")]
    AudioSource _source;
    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _endMusic;
    [SerializeField] AudioClip _superSFX;
    [SerializeField] AudioClip _endGameSFX;
    [SerializeField] AudioMixer _musicMixer;
    [SerializeField] AudioMixer _sfxMixer;
    [SerializeField] bool _spinDown;

    [Header("Stats")]
    GameEvent _lastSuperEvent;
    FighterFilter _lastHit = FighterFilter.both;
    public float _serveSpeed = 7.5f;
    public bool needNewRound = false;
    float _rotateTarget;
    int _rally;
    int _successive = 0;
    int _targetRally = 10;
    bool _isPaused;
    bool _slowMusic;
    bool _killSwitch;

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

    public Material GetRedOutline() {
        return _redMaterial;
    }

    public Material GetYellowOutline() {
        return _yellowMaterial;
    }

    public Material GetBlueOutline() {
        return _blueMaterial;
    }


    void InitializeGame() {

        GameObject fOneObject = Instantiate(_settings.GetFighterOneProfile().GetPrefab(), _fighterOne.GetSpawn(), Quaternion.identity);
        GameObject fTwoObject = Instantiate(_settings.GetFighterTwoProfile().GetPrefab(), _fighterTwo.GetSpawn(), Quaternion.identity);

        _fighterOne.SetUpFighter(fOneObject.GetComponent<FighterController>(), _settings.GetFighterOneProfile().GetName());
        _fighterTwo.SetUpFighter(fTwoObject.GetComponent<FighterController>(), _settings.GetFighterTwoProfile().GetName());
        _fighterOne.UpdateIcon(_settings.GetFighterOneProfile().GetFighterIcon());
        _fighterTwo.UpdateIcon(_settings.GetFighterTwoProfile().GetFighterIcon());

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


        _fighterOne.GetController().InitializeFighter();
        _fighterTwo.GetController().InitializeFighter();

        _fighterOne.GetController().transform.position = _fighterOne.GetSpawn();
        _fighterTwo.GetController().transform.position = _fighterTwo.GetSpawn();

        _fighterOne.GetController().SetPallette(false, null);

        var pallete1 = _settings.GetFighterOneProfile().GetPallete(_settings.GetSkinOneID());
        _fighterOne.GetController().SetPallette(_settings.GetSkinOneID() > 0, pallete1);

        var pallete2 = _settings.GetFighterTwoProfile().GetPallete(_settings.GetSkinTwoID());
        _fighterTwo.GetController().SetPallette(_settings.GetSkinTwoID() > 0, pallete2);

        PerformCoinToss();

        _shuttle.SetOwner(_fighterOne.GetController());
        _shuttle.SetOwner(FighterFilter.both);

        _UIScoreF1.text = _settings.GetFighterOneProfile().GetName();
        _UIScoreF2.text = _settings.GetFighterTwoProfile().GetName();

        //ScoreManager.Get().SetPlayerTypes(_settings.GetFighterOneProfile().GetName(), _settings.GetFighterTwoProfile().GetName());
    }

    public bool IsInKO() {
        return KOCoroutine != null;
    }

    public bool IsGameActive() {
        return !_killSwitch;
    }

    public void KillSwitch() {
        _killSwitch = true;
    }

    private void Start() {
        Resume();
        if (GameLogic.Get()._type == GameType.tutorial || GameLogic.Get()._type == GameType.training) {
            _fighterTwo.GetController().gameObject.SetActive(false);
            _debugCanvas.SetActive(true);
        }
        else {
            SetUpGame();
        }
    }

    public bool NoActiveCoroutines() {
        return KOCoroutine == null && EndGameCoroutine == null && stageCoroutine == null && impactCoroutine == null && roundCoroutine == null;
    }

    public void Resume() {
        SetSounds(0.8f, 22000);

        Time.timeScale = 1;
        _pauseController.Disable();

        _uiCamera.SetActive(true);
        _hasHeldPause = true;
    }

    float _pauseTime;
    bool _hasHeldPause;

    float _groundTime;
    void Update() {

        if (_hasHeldPause) {
            _hasHeldPause = GlobalInputManager.Get().GetPauseHeldInput();
        }

        if (!_hasHeldPause && GlobalInputManager.Get().GetPauseHeldInput() && !_isPaused && NoActiveCoroutines()) {
            FighterFilter pauser = FighterFilter.none;
            _pauseTime += Time.fixedDeltaTime * 2;
            if (_pauseTime >= 1) {
                pauser = _fighterOne.GetController().GetFilter();
                if (_fighterTwo.GetController().GetComponent<InputHandler>().GetPause()) {
                    pauser = _fighterTwo.GetController().GetFilter();
                }
                //    print("F" + pauser);
                //}
                //if (_fighterOne.GetController().GetComponent<InputHandler>().GetPause()) {
                //    pauser = _fighterOne.GetController().GetFilter();
                //    print("E" + pauser);
                //}
                //print("F" + pauser);

                _pauseController.Enable();
                _pauseController.SetPauseOwner(pauser);
                _uiCamera.SetActive(false);
                _pauseTime = 0;
                Time.timeScale = 0;
            }
        }
        else {
            _pauseTime = 0;
        }


        _pauseArt.fillAmount = Mathf.Clamp(_pauseTime, 0, 1);
        _pauseArtBack.fillAmount = _pauseArt.fillAmount;

        //_rotateTarget = Mathf.Lerp(_rotateTarget, _shuttle.GetSpeed() / _shuttle.GetMaxJailSpeed(), Time.fixedDeltaTime * 12);
        //var vec = Vector3.Lerp(new Vector3(0, 0, -80f), new Vector3(0, 0, 80f), _rotateTarget);
        //vec.x = 0;
        //_speedRotator.localEulerAngles = vec;

        if (Keyboard.current.f10Key.wasPressedThisFrame) {
            _debugCanvas.SetActive(!_debugCanvas.activeSelf);
            _debugCamera.SetActive(_debugCanvas.activeSelf);
        }

        if(GameLogic.Get()._type == GameType.training) {
            if (GlobalInputManager.Get().GetResetInput()) {
                _shuttle.transform.position = _shuttleSpawn;
                _shuttle.ResetShuttle(true);
            }

            if (GlobalInputManager.Get().GetHitboxInput()) {
                _debugCamera.SetActive(!_debugCamera.activeSelf);
            }

            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                _fighterOne.GetController().SetMeterToFull();
            }
        }

        if (_slowMusic) {
            _music.pitch = 0.5f;
        }

        if (_spinDown) {
            _music.pitch *= 0.99f;
        }
        else {
            if (_music.pitch < 1 && stageCoroutine == null) {
                _music.pitch += Time.fixedDeltaTime;

                if (_music.pitch > 1) {
                    _music.pitch = 1;
                }
            }
        }

        if (_shuttle.CanTimeOut() && NoActiveCoroutines() && !(GameLogic.Get()._type == GameType.training || GameLogic.Get()._type == GameType.tutorial)) {
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
    }

    public void SlowDownMusic() {
        _slowMusic = true;
    }

    public void SpeedUpMuisc() {
        _slowMusic = false;
    }

    void FlashScore() {
        _UIScore.SetTrigger("score");
        var pointOne = string.Format("{0:00}", ScoreManager.Get().GetScores()[ScoreManager.Get().GetCurrentRound() - 1, 0]);
        var pointTwo = string.Format("{0:00}", ScoreManager.Get().GetScores()[ScoreManager.Get().GetCurrentRound() - 1, 1]);
        _UIScoreText.text = $"{pointOne} - {pointTwo}";
    }

    bool _hasFirstTriggered;
    void SetUpGame() {
        if (ScoreManager.Get().gameOver != FighterFilter.both) {
            EndGame();
            return;
        }

        if (_lastSuperEvent != null) {
            _lastSuperEvent.DisableScreen();
        }

        _net.OnIntro();

        _killSwitch = true;
        ResetCoroutines();

        _stageCamera.SetActive(true);
        _music.pitch = 1f;
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

        roundCoroutine = StartCoroutine(RoundSetUp(!_hasFirstTriggered && ScoreManager.Get().gameOver == FighterFilter.both));

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
        _successive = 0;
        _targetRally = 10;
        _shuttle.ResetBounce();
        _shuttle.SetBounciness(1f);
        TimerManager.Get().ResetPointTimer();
        SetLastHitter(FighterFilter.both);

        _hasFirstTriggered = true;

        //_shuttle.transform.position = _shuttleSpawn;
        _shuttle.GetComponent<Rigidbody>().isKinematic = true;
    }

    public Shaker GetCameraShaker() {
        return _cameraShaker;
    }

    public Shaker GetStageShaker() {
        return _stageShaker;
    }

    Coroutine stageCoroutine;
    public void OnSpecial(GameEvent gEvent, FighterFilter filter, FighterController controller) {
        StunFrames(1f, FighterFilter.both);
        _shuttle.FreezeShuttle(1.0f);
        _source.PlayOneShot(_superSFX);


        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        stageCoroutine = StartCoroutine(EventFlash(1, gEvent, filter, controller));
    }

    void ResetCoroutines() {
        if (impactCoroutine != null) {
            StopCoroutine(impactCoroutine);
        }

        if (stageCoroutine != null) {
            StopCoroutine(stageCoroutine);
        }

        if (impactCoroutine != null) {
            StopCoroutine(impactCoroutine);
        }
    }


    Coroutine impactCoroutine;
    public void OnImpactFrame(float time) {
        if (impactCoroutine != null) {
            StopCoroutine(impactCoroutine);
        }

        impactCoroutine = StartCoroutine(ImpactFrameProcess(time));
    }

    public Coroutine roundCoroutine;
    IEnumerator RoundSetUp(bool hideScore) {
        _killSwitch = true;
        _UIHealthBars.SetTrigger("Out");
        yield return new WaitForSeconds(1);

        if (!hideScore) {
            FlashScore();
            yield return new WaitForSeconds(2);
        }

        _UISetWhite.text = "BREAK " + ScoreManager.Get().GetCurrentPoint();
        _UISetBlack.text = "BREAK " + ScoreManager.Get().GetCurrentPoint();
        _UICurrentSet.SetTrigger("Set");
        yield return new WaitForSeconds(1);
        StartCoroutine("ServeTimer");
        yield return new WaitForSeconds(1);
        _net.OnIntroEnd();
        _UIHealthBars.SetTrigger("In");
        _killSwitch = false;

        roundCoroutine = null;
    }

    public bool IsInRoundSetup() {
        return roundCoroutine != null;
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
        }
        if (filter == FighterFilter.two || filter == FighterFilter.both) {
            _fighterTwo.GetController().StunController(timer);
        }
    }

    public ShuttleCock GetShuttle() {
        return _shuttle;
    }

    IEnumerator EventFlash(float time, GameEvent gEvent, FighterFilter filter, FighterController controller) {
        gEvent.SetOrientation(filter);
        _music.pitch = -1f;
        gEvent.EnableScreen();
        _lastSuperEvent = gEvent;
        _stageCamera.SetActive(false);
        yield return new WaitForSeconds(time);
        controller.OnAfterSuperScreen();
        gEvent.DisableScreen();
        _stageCamera.SetActive(true);
        _music.pitch = 1f;
        stageCoroutine = null;
    }

    public bool IsSpecialScreen() {
        return stageCoroutine != null;
    }

    public Coroutine KOCoroutine;
    public void KOEvent(FighterFilter filter) {
        if (KOCoroutine != null) {
            return;
        }

        if(filter == FighterFilter.one) {
            _net.OnKO(_settings.GetFighterOneProfile().GetCircleKOSprite());
        }
        else {
            _net.OnKO(_settings.GetFighterTwoProfile().GetCircleKOSprite());
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
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 1f;
            //Time.fixedDeltaTime = 0.02f;
            yield return new WaitForSecondsRealtime(1f);
        }
        else {
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 1f;
            //Time.fixedDeltaTime = 0.02f;
            yield return new WaitForSecondsRealtime(2f);
        }
        SetUpGame();
        KOCoroutine = null;
    }

    IEnumerator EndGameProcess(FighterFilter winner, FighterFilter loser) {
        _UIStage.SetTrigger("Fade");
        _UIBars.SetBool("Show", true);
        DisableUI();
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
        loseController.PlayLose();
        winController.PlayWin();

        CameraContoller.Get().SetFocus(loseController.GetFocusTransform(), Vector2.zero);
        yield return new WaitForSecondsRealtime(5f);
        CameraContoller.Get().SetFocus(winController.GetFocusTransform(), Vector2.zero);
        yield return new WaitForSecondsRealtime(5f);
        //SceneManager.LoadScene(sceneName: "WinScreenTest");
        CameraContoller.Get().SetFocus(winController.GetFocusTransform(), new Vector2(-2.5f, -0.15f));
        _winScreen.SetActive(true);
    }

    public FighterController GetFighter(FighterFilter filter) {
        if (filter == FighterFilter.one) {
            return GetFighterOne();
        }
        return GetFighterTwo();
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
            var hitMes = new HitMessage(new Vector3(_shuttle.transform.position.x / -4f, _serveSpeed, 0f), new VelocityInfluence(), false, FighterFilter.both, ShotType.lift);
            _shuttle.Shoot(hitMes);
        }
        TimerManager.Get().SetTimerState(true);
    }

    public void IncreaseRally() {
        _rally++;
        SuccessiveHit();
        //_successive = 1;
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
        if(GameLogic.Get()._type == GameType.training) {
            return 1;
        }

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

    public void EnableUI() {
        _uiCamera.SetActive(true);
    }

    public void DisableUI()
    {
        _uiCamera.SetActive(false);
    }

    public void SetSounds(float volumeMult, float lowpass) {
        _musicMixer.SetFloat("lowpass", lowpass);
        _sfxMixer.SetFloat("volume", (-80 + volumeMult * 100));
    }
}
