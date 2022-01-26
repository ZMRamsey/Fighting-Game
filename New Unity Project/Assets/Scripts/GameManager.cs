using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FighterFilter { one, two, both};
public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    [SerializeField] Shaker _cameraShaker;
    [SerializeField] FighterController _fighterOne;
    [SerializeField] FighterController _fighterTwo;
    [SerializeField] ShuttleCock _shuttle;
    [SerializeField] Vector3 _fighterOneSpawn;
    [SerializeField] Vector3 _fighterTwoSpawn;
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

        _fighterOne.transform.position = _fighterOneSpawn;
        _fighterTwo.transform.position = _fighterTwoSpawn;
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
            _fighterOne.StunController(timer);
        }
        if (filter == FighterFilter.two || filter == FighterFilter.both) {
            _fighterTwo.StunController(timer);
        }
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
}
