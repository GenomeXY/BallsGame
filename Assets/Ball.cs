using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : ActiveItem
{
    [SerializeField] private BallSettings _ballSettings;
    [SerializeField] private Renderer _ballRenderer;

    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        _ballRenderer.material = _ballSettings.BallMaterials[level];

        Projection.Setup(_ballSettings.BallProjectionMaterials[level], _levelText.text, Radius);
    }
}
