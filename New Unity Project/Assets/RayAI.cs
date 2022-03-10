using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAI : FighterController
{
    public override void InitializeFighter()
    {
        base.InitializeFighter();
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return _renderer;
    }
}
