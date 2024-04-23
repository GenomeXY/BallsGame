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

    //������� ����� ��� � �����
    void CreateItemInTube()
    {
        //��������� ���� ��������� ������� � ������� ��� � ������� �����
        int itemLevel = Random.Range(0, 5);
        _itemInTube = Instantiate(_ballPrefab, _tube.position, Quaternion.identity); 
        _itemInTube.SetLevel(itemLevel);
        _itemInTube.SetupToTube(); //��������� ������ � ����
    }

    //�������� ��� ����������� ���� �� ����� � spawner
    private IEnumerator MoveToSpawner()
    {
        _itemInTube.transform.parent = _spawner; //�������������� item � spawner
        for (float t = 0; t < 1f; t+=Time.deltaTime / 0.3f) //�� 0,3 ���. ������ ������� ��� 
        {
            _itemInTube.transform.position = Vector3.Lerp(_tube.position, _spawner.position, t);
            yield return null;
        }
        _itemInTube.transform.localPosition = Vector3.zero;//������ ��� ����� � ������� spawner
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
        //����� ������� ��� ������ ���� ��� �������� ���
        _itemInSpawner = null;
        if(_itemInTube)
        {
            StartCoroutine(MoveToSpawner());
        }
    }
}
