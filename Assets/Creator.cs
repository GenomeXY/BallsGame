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

    [SerializeField] private Transform _rayTransform;
    [SerializeField] private LayerMask _layerMask;
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
        for (float t = 0; t < 1f; t+=Time.deltaTime / 0.4f) //за 0,4 сек. плавно двигаем шар 
        {
            _itemInTube.transform.position = Vector3.Lerp(_tube.position, _spawner.position, t);
            yield return null;
        }
        _itemInTube.transform.localPosition = Vector3.zero; //ставим шар ровно в позицию spawner
        _itemInSpawner = _itemInTube;
        _rayTransform.gameObject.SetActive(true); //включаем луч 
        _itemInSpawner.Projection.Show(); //включаем проекцию
        _itemInTube = null;
        CreateItemInTube();
    }

    void LateUpdate()
    {
        if (_itemInSpawner)
        {
            Ray ray = new Ray(_spawner.position, Vector3.down);
            RaycastHit hit;
            if (Physics.SphereCast(ray, _itemInSpawner.Radius, out hit, 100, _layerMask, QueryTriggerInteraction.Ignore)) //пользуемся 10 сигнатурой метода, включаем игнорирование триггеров шаров
            {
                _rayTransform.localScale = new Vector3(_itemInSpawner.Radius * 2, hit.distance, 1f); //hit.distance - это расстояние, которое прошел луч перед тем как попал в коллайдер, оно же и равно масштабу луча, который нам нужен
                _itemInSpawner.Projection.SetPosition(_spawner.position + Vector3.down * hit.distance); //установка позиции проекции - от спауна вниз на расстояние, которое прошел луч
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }
        }
    }

    void Drop()
    {
        _itemInSpawner.DropItem();
        _itemInSpawner.Projection.Hide(); //выключаем проекцию
        //чтобы бросить мяч только один раз обнуляем его
        _itemInSpawner = null;
        _rayTransform.gameObject.SetActive(false); //выключаем луч при сбросе объекта        
        if (_itemInTube)
        {
            StartCoroutine(MoveToSpawner());
        }
    }
}
