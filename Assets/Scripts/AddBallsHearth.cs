using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBallsHearth : ActiveItem
{
    [Header("AddBallsHearth")]
    [SerializeField] private GameObject _effectPrefab;
    protected override void Start()
    {
        base.Start();
    }

    public override void SetLevel(int level)
    {
        base.SetLevel(level);

        Projection.SetupLevelText(_levelText.text);
    }
    private IEnumerator AffectProcess()
    {
        _animator.enabled = true;
        yield return new WaitForSeconds(1f);

        Creator.Instance.BallsLeft += Int32.Parse(_levelText.text);
        Creator.Instance.UpdateBallsLeftText();   

        Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override void DoEffect()
    {
        base.DoEffect();
        StartCoroutine(AffectProcess());
    }
}
