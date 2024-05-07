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

        //������ ���� �������������, ����� ������������� ��� �������
        Radius = Mathf.Lerp(0.4f, 0.7f, level / 10f); //��� ������ 0 - ������ 0.4f, ��� ������ 10 - ������ 0.7f
        //������ ������� ���� � ������� ���������� � ��������
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
