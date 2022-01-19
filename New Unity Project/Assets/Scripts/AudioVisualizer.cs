using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] float _updateSteps = 0.1f;
    [SerializeField] float _smooth;
    int _sampleLength = 1024;
    float[] _clipSamples;

    float _updateTime = 0f;
    float _clipLoudness;

    [SerializeField] float _sizeMult = 1;

    [SerializeField] Vector2 _xSize = new Vector2(1, 500);
    [SerializeField] Vector2 _ySize = new Vector2(1, 500);


    private void Awake() {
        _clipSamples = new float[_sampleLength];
    }

    private void Update() {
        _updateTime += Time.deltaTime;

        if (_updateTime >= _updateSteps) {
            _updateTime = 0f;
            _audio.clip.GetData(_clipSamples, _audio.timeSamples);
            _clipLoudness = 0f;
            foreach (var sample in _clipSamples) {
                _clipLoudness += Mathf.Abs(sample);
            }
            _clipLoudness /= _sampleLength; 

            _clipLoudness *= _sizeMult;
            var xClipLoudness = _clipLoudness;
            xClipLoudness = Mathf.Clamp(_clipLoudness, _xSize.x, _xSize.y);
            _clipLoudness = Mathf.Clamp(_clipLoudness, _ySize.x, _ySize.y);
            Vector3 scale = new Vector3(xClipLoudness, _clipLoudness, _clipLoudness);

            if(transform.localScale.magnitude < scale.magnitude) {
                transform.localScale = scale;
            }

            transform.localScale = Vector3.MoveTowards(transform.localScale, scale, Time.deltaTime * _smooth);
        }
    }
}
