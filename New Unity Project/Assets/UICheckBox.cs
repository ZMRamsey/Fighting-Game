using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICheckBox : UIButton
{
    [Header("CheckBox Settings")]
    [SerializeField] Sprite _checkedSprite;
    [SerializeField] Sprite _uncheckedSprite;

    bool _checked = true;

    public void Check()
    {
        SetSprites(_checkedSprite);
    }

    public void UnCheck()
    {
        SetSprites(_uncheckedSprite);
    }

    public override void OnSubmit()
    {
        base.OnSubmit();
        if (_checked)
        {
            UnCheck();
        }
        else
        {
            Check();
        }
        _checked = !_checked;
    }

    public bool GetChecked()
    {
        return _checked;
    }
}
