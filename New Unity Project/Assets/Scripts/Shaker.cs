using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float shake_intensity = .01f;
    [SerializeField] float _dampen;

    float shake_decay = 0.01f;
    [SerializeField] float shake_duration = 0;
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
            shake_duration = shake_intensity;
        }

        if (shake_duration > 0) {
            float dur = Mathf.Clamp(shake_duration, 0, 1);
            if (_ignoreScaling) {
                dur = 1;
            }

            Vector3 targetPos = originPosition + Random.insideUnitSphere * dur;

            Quaternion targetRot = new Quaternion(
                originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .1f * dur,
                originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .1f * dur,
                originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .1f * dur,
                originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .1f * dur);
            shake_duration -= Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * _dampen);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * _dampen);
        }
        else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPosition, Time.deltaTime * 4);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originRotation, Time.deltaTime * 4);
        }
    }

    public void SetShake(float duration) {
        shake_duration = duration;

    }
}
