using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : PassiveItem
{
    [SerializeField] private GameObject _dieEffect; //частицы при разрушении камн€
    [Range(0, 2)]
    [SerializeField] private int _level = 2;
    [SerializeField] private Transform _visualTransform;
    [SerializeField] private Stone _stonePrefab;

    public override void OnAffect()
    {
        base.OnAffect();
        if (_level > 0)
        {
            //—оздаем два камн€
            for (int i = 0; i < 2; i++)
            {
                CreateChildRock(_level - 1);
            }
        }
        Die();
    }

    void CreateChildRock(int level)
    {
        Stone newRock = Instantiate(_stonePrefab, transform.position, Quaternion.identity);
        newRock.SetLevel(level);
    }

    //”станавливаем параметры камн€ в зависимости от уровн€
    public void SetLevel(int level)
    {
        _level = level;
        float scale = 1f;
        if (level == 2)
        {
            scale = 1f;
        }
        else if (level == 1)
        {
            scale = .7f;
        }
        else if (level == 0)
        {
            scale = .45f;
        }
        _visualTransform.localScale = Vector3.one * scale;
    }

    void Die()
    {
        Instantiate(_dieEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
