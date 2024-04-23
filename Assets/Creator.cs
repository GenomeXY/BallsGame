using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField] private Transform _tube; 
    [SerializeField] private Transform _spawner;
    [SerializeField] private ActiveItem _ballPrefab;

    private ActiveItem _itemInTube;
    private ActiveItem _itemInSpawner;

    void Start()
    {
        CreateItemInTube();
        StartCoroutine(MoveToSpawner());
    }

    //Создаем новый мяч в трубе
    void CreateItemInTube()
    {
        //Назначаем шару случайный уровень и создаем шар в позиции трубы
        int itemLevel = Random.Range(0, 5);
        _itemInTube = Instantiate(_ballPrefab, _tube.position, Quaternion.identity); 
        _itemInTube.SetLevel(itemLevel);
        _itemInTube.SetupToTube(); //отключаем физику у шара
    }

    //Корутина для перемещения шара из трубы к spawner
    private IEnumerator MoveToSpawner()
    {
        _itemInTube.transform.parent = _spawner; //припэринчиваем item к spawner
        for (float t = 0; t < 1f; t+=Time.deltaTime / 0.3f) //за 0,3 сек. плавно двигаем шар 
        {
            _itemInTube.transform.position = Vector3.Lerp(_tube.position, _spawner.position, t);
            yield return null;
        }
        _itemInTube.transform.localPosition = Vector3.zero;//ставим шар ровно в позицию spawner
        _itemInSpawner = _itemInTube;
        _itemInTube = null;
        CreateItemInTube();
    }

    void Update()
    {
        if (_itemInSpawner)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }
        }
    }

    void Drop()
    {
        _itemInSpawner.DropItem();
        //чтобы бросить мяч только один раз обнуляем его
        _itemInSpawner = null;
        if(_itemInTube)
        {
            StartCoroutine(MoveToSpawner());
        }
    }
}
