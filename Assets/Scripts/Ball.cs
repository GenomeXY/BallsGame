using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : ActiveItem
{
    [SerializeField] private BallSettings _ballSettings;
    [SerializeField] private Renderer _ballRenderer;
    [SerializeField] private Transform _visualTransform;
    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        _ballRenderer.material = _ballSettings.BallMaterials[level];

        //Радиус шара увеличивается, когда увеличивается его уровень
        Radius = Mathf.Lerp(0.4f, 0.7f, level / 10f); //при уровне 0 - радиус 0.4f, при уровне 10 - радуис 0.7f
        //Меняем масштаб шара и радиусы коллайдера и триггера
        Vector3 ballScale = Vector3.one * Radius * 2f;
        _visualTransform.localScale = ballScale;
        _collider.radius = Radius;
        _trigger.radius = Radius + 0.1f;

        Projection.Setup(_ballSettings.BallProjectionMaterials[level], _levelText.text, Radius);
              

        if (ScoreManager.Instance.AddScore(ItemType, transform.position, level))
        {
            Die();
        }    
    }

    public override void DoEffect()
    {
        base.DoEffect();
        IncreaseLevel();
    }
}
