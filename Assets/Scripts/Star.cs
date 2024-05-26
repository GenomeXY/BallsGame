using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : ActiveItem
{
    [Header("Star")]
    [SerializeField] private float _affectRadius = 2.2f; //радиус действия

    [SerializeField] private GameObject _affectArea;
    [SerializeField] private GameObject _effectPrefab;

    protected override void Start()
    {
        base.Start();
        _affectArea.SetActive(false); //выключаем круг-радуис действия
    }

    public override void SetLevel(int level)
    {
        base.SetLevel(level);

        Projection.SetupLevelText(_levelText.text);
    }

    private IEnumerator AffectProcess()
    {
        _affectArea.SetActive(true); //включает круг-радуис действия, чтобы мы видели зону действия
        _animator.enabled = true;
        yield return new WaitForSeconds(1f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _affectRadius); // возвращает массив всех коллайдеров, которые пересекла сфера определенного радиуса
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].attachedRigidbody)
            {
                ActiveItem item = colliders[i].attachedRigidbody.GetComponent<ActiveItem>();
                if (item && colliders[i].isTrigger)
                {
                    item.IncreaseLevel();
                }
            }
        }

        Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnValidate() //вызывается, когда мы что-то меняем в инспекторе (чтобы покрутить зону действия в инспекторе)
    {
        _affectArea.transform.localScale = Vector3.one * _affectRadius * 2f;
    }

    public override void DoEffect()
    {
        base.DoEffect();
        StartCoroutine(AffectProcess());
    }
}
