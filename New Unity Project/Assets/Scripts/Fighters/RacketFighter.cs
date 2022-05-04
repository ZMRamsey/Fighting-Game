using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacketFighter : FighterController
{
    [SerializeField] GameObject _subWooferPrefab;
    [SerializeField] GameObject _mrHandyPrefab;
    [SerializeField] CanvasGroup _raketUI;
    [SerializeField] Image _raketMeter;
    float _buildMeter;
    GameObject _subWooferObject;
    GameObject _mrHandyObject;
    Material _pallete;

    public override void OnSuperMechanic() {
        base.OnSuperMechanic();
        _subWooferObject.GetComponent<SubWoofer>().ResetShuttle(false);
        GameManager.Get().OnSpecial(GameManager.Get().GetEventManager().GetRacketSuper(), _filter, this);
    }

    public override void OnSuperEnd(bool instant) {
        if (instant && _subWooferObject != null) {
            _subWooferObject.SetActive(false);
        }
    }

    public override void SetPallette(bool active, Material mat) {
        _pallete = mat;
        _mrHandyObject.GetComponent<MrHandy>().SetPallete(_pallete);

        if (_rgbRenderer != null) {
            _rgbRenderer.enabled = active;

            if (active) {
                _rgbRenderer.material = mat;
            }
        }
    }

    public float GetBuildMeter() {
        return _buildMeter;
    }

    public override void OnSuperEvent() {
        _subWooferObject.GetComponent<SubWoofer>().ResetShuttle(false);
        _subWooferObject.SetActive(true);
        _subWooferObject.transform.position = transform.position;

        var dir = _settings.GetSuperMove().GetHitDirection();
        if (_renderer.flipX) {
            dir.x *= -1;
        }

        _subWooferObject.GetComponent<Rigidbody>().velocity = dir * 3;
    }

    public override void OnFighterUpdate() {

        var isBuilding = !GameManager.Get().IsInKO() && _inputHandler.GetCrouch() && _myState == FighterState.inControl && (_mrHandyObject && !_mrHandyObject.activeSelf) && _canAttack && GetGrounded() && _buildMeter < 1;

        _animator.SetBool("charge", isBuilding);

        if (isBuilding) {
            _raketUI.alpha = 1;
            _buildMeter += Time.deltaTime * 0.15f;
            _buildMeter = Mathf.Clamp(_buildMeter, 0, 1);
            _raketMeter.fillAmount = _buildMeter;
        }

        if (!isBuilding && _raketUI.alpha != 0) {
            _raketUI.alpha -= Time.deltaTime * 4;
        }


        if (_inputHandler.GetCrouch() && _inputHandler.GetChip() && !_mrHandyObject.activeSelf && _buildMeter >= 1 && _myState == FighterState.inControl) {
            _buildMeter = 0;
            _mrHandyObject.transform.position = transform.position;
            _mrHandyObject.GetComponent<MrHandy>().ResetHandy();
            _mrHandyObject.SetActive(true);
        }
    }

    public override void InitializeFighter() {
        base.InitializeFighter();

        _subWooferObject = Instantiate(_subWooferPrefab, transform.position, Quaternion.identity);
        _subWooferObject.SetActive(false);

        _mrHandyObject = Instantiate(_mrHandyPrefab, transform.position, Quaternion.identity);
        _mrHandyObject.GetComponent<MrHandy>().OnSpawn(_filter);
        _mrHandyObject.SetActive(false);
    }

    public override void ResetFighter() {
        base.ResetFighter();
        _raketUI.alpha = 0;
        _buildMeter = 0;
        _raketMeter.fillAmount = 0;
        if (_mrHandyObject && _mrHandyObject.activeSelf) {
            _mrHandyObject.GetComponent<MrHandy>().OnDeath(true);
        }
    }  
}
