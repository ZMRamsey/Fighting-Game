using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float _shakeIntensity = .01f;
    [SerializeField] float _dampen;

    float _shakeDecay = 0.01f;
    [SerializeField] float _shakeDuration = 0;
    [SerializeField] bool _ignoreScaling;

    public bool shake;

    Vector3 originPosition;
    Quaternion originRotation;

    void Awake() {
        originPosition = transform.localPosition;
        originRotation = transform.localRotation;
    }

    void Update() {
        if (shake) {
            _shakeDuration = _shakeIntensity;
        }

        if (_shakeDuration > 0) {
            float dur = Mathf.Clamp(_shakeDuration, 0, 1);
            if (_ignoreScaling) {
                dur = 1;
            }

            Vector3 targetPos = originPosition + Random.insideUnitSphere * dur;

            Quaternion targetRot = new Quaternion(
                originRotation.x + Random.Range(-_shakeIntensity, _shakeIntensity) * .1f * dur,
                originRotation.y + Random.Range(-_shakeIntensity, _shakeIntensity) * .1f * dur,
                originRotation.z + Random.Range(-_shakeIntensity, _shakeIntensity) * .1f * dur,
                originRotation.w + Random.Range(-_shakeIntensity, _shakeIntensity) * .1f * dur);
            _shakeDuration -= Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * _dampen);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * _dampen);
        }
        else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPosition, Time.deltaTime * 4);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originRotation, Time.deltaTime * 4);
        }
    }

    public void SetShake(float duration, float strength, bool ignoreScale) {
        _ignoreScaling = ignoreScale;
        _shakeIntensity = strength;
        _dampen = strength;
        _shakeDuration = duration;

    }
}
