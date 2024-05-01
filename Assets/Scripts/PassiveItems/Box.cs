using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : PassiveItem
{
    [Range(0f, 2f)] 
    public int Health = 1; //количество жизней Box
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private GameObject _breakEffectPrefab;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        SetHealth(Health);
    }

    public override void OnAffect()
    {
        base.OnAffect();
        Health -= 1;
        Instantiate(_breakEffectPrefab, transform.position, Quaternion.Euler(-90f, 0, 0));
        _animator.SetTrigger("Shake");
        if (Health < 0)
        {
            Die();
        }
        else
        {
            SetHealth(Health);
        }
    }

    void SetHealth(int value) //включает-выключает уровни коробки в зависимости от кол-ва жизней;
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].SetActive(i <= value);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}

