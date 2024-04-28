using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamit : ActiveItem
{
    [Header("Dynamit")]
    [SerializeField] private float _affectRadius = 1.5f; //радиус действия
    [SerializeField] private float _forceValue = 1000f; //сила действия

    [SerializeField] private GameObject _affectArea; 
    [SerializeField] private GameObject _effectPrefab;

    protected override void Start()
    {
        base.Start();
        _affectArea.SetActive(false); //выключаем круг-радуис действия
    }

    [ContextMenu("Explode")]
    public void Explode() //взрывает динамит
    {
        StartCoroutine(AffectProcess());
    }

    private IEnumerator AffectProcess()
    {
        _affectArea.SetActive(true); //включает круг-радуис действия, чтобы мы видели зону поражения
        _animator.enabled = true;
        yield return new WaitForSeconds(1f);
        Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnValidate() //вызывается, когда мы что-то меняем в инспекторе (чтобы покрутить зону поражения в инспекторе)
    {
        _affectArea.transform.localScale = Vector3.one * _affectRadius * 2f;
    }
}
