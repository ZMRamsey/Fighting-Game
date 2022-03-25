using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class FighterTab
{
    [SerializeField] FighterController _controller;
    [SerializeField] FighterUI _fighterUI;
    [SerializeField] Vector3 _spawn;
    [SerializeField] TextMeshProUGUI _fighterName;
    [SerializeField] TextMeshProUGUI _fighterScore;
    [SerializeField] Animator _scoreAnim;
    [SerializeField] Image _icon;

    public FighterUI GetUI() {
        return _fighterUI;
    }

    public void SetUpFighter(FighterController controller, string name) {
        _controller = controller;
        _fighterName.text = name;
    }

    public FighterController GetController() {
        return _controller;
    }

    public Vector3 GetSpawn() {
        return _spawn;
    }

    public void UpdateScore(int score) {
        _fighterScore.text = score.ToString();
    }

    public void UpdateRallyScore(int rally) {
        _scoreAnim.SetTrigger("Score");
        _scoreAnim.GetComponent<TextMeshProUGUI>().text = rally.ToString();
    }

    public void UpdateIcon(Sprite sprite) {
        _icon.sprite = sprite;
    }
}
