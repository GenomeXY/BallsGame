using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamit : ActiveItem
{
    [Header("Dynamit")]
    [SerializeField] private float _affectRadius = 2.5f; //радиус действия
    [SerializeField] private float _forceValue = 500f; //сила действия

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

        Collider[] colliders = Physics.OverlapSphere(transform.position, _affectRadius); // возвращает массив всех коллайдеров, которые пересекла сфера определенного радиуса
        for (int i = 0; i < colliders.Length; i++)
        {
            // Применяем силу ко всем Rigidbody в радиусе
            Rigidbody rigidbody = colliders[i].attachedRigidbody;
            if (rigidbody)
            {
                Vector3 fromTo = (rigidbody.transform.position - transform.position).normalized;
                rigidbody.AddForce(fromTo * _forceValue + Vector3.up * _forceValue * 0.5f); //применяем силу от динамита + ещё немного вверх
            }
            // Производим эффект на каждом PassiveItem
            PassiveItem passiveItem = colliders[i].GetComponentInParent<PassiveItem>();
            if (passiveItem)
            {
                passiveItem.OnAffect();
            }
        }

        Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnValidate() //вызывается, когда мы что-то меняем в инспекторе (чтобы покрутить зону поражения в инспекторе)
    {
        _affectArea.transform.localScale = Vector3.one * _affectRadius * 2f;
    }
}
