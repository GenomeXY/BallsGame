using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int _ballsLeft;
    [SerializeField] private TextMeshProUGUI _numberOfBallsText;
    void Start()
    {
        _ballsLeft = Level.Instance.NumberOfBalls; // ��������� ���������� �����, ������� �������� � ������
        UpdateBallsLeftText();

        CreateItemInTube();
        StartCoroutine(MoveToSpawner());
    }

    public void UpdateBallsLeftText()
    {
        _numberOfBallsText.text = _ballsLeft.ToString();
    }

    //������� ����� ��� � �����
    void CreateItemInTube()
    {
        if (_ballsLeft == 0)
        {
            Debug.Log("Balls Ended");
            return;
        }
        //��������� ���� ��������� ������� � ������� ��� � ������� �����
        int itemLevel = Random.Range(0, 5);
        _itemInTube = Instantiate(_ballPrefab, _tube.position, Quaternion.identity); 
        _itemInTube.SetLevel(itemLevel);
        _itemInTube.SetupToTube(); //��������� ������ � ����
        _ballsLeft--;
        UpdateBallsLeftText(); 
    }

    //�������� ��� ����������� ���� �� ����� � spawner
    private IEnumerator MoveToSpawner()
    {
        _itemInTube.transform.parent = _spawner; //�������������� item � spawner
        for (float t = 0; t < 1f; t+=Time.deltaTime / 0.4f) //�� 0,4 ���. ������ ������� ��� 
        {
            _itemInTube.transform.position = Vector3.Lerp(_tube.position, _spawner.position, t);
            yield return null;
        }
        _itemInTube.transform.localPosition = Vector3.zero; //������ ��� ����� � ������� spawner
        _itemInSpawner = _itemInTube;
        _rayTransform.gameObject.SetActive(true); //�������� ��� 
        _itemInSpawner.Projection.Show(); //�������� ��������
        _itemInTube = null;
        CreateItemInTube();
    }

    void LateUpdate()
    {
        if (_itemInSpawner)
        {
            Ray ray = new Ray(_spawner.position, Vector3.down);
            RaycastHit hit;
            if (Physics.SphereCast(ray, _itemInSpawner.Radius, out hit, 100, _layerMask, QueryTriggerInteraction.Ignore)) //���������� 10 ���������� ������, �������� ������������� ��������� �����
            {
                _rayTransform.localScale = new Vector3(_itemInSpawner.Radius * 2, hit.distance, 1f); //hit.distance - ��� ����������, ������� ������ ��� ����� ��� ��� ����� � ���������, ��� �� � ����� �������� ����, ������� ��� �����
                _itemInSpawner.Projection.SetPosition(_spawner.position + Vector3.down * hit.distance); //��������� ������� �������� - �� ������ ���� �� ����������, ������� ������ ���
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }
        }
    }

    private Coroutine _waitForLose;
    void Drop()
    {
        _itemInSpawner.DropItem();
        _itemInSpawner.Projection.Hide(); //��������� ��������
        //����� ������� ��� ������ ���� ��� �������� ���
        _itemInSpawner = null;
        _rayTransform.gameObject.SetActive(false); //��������� ��� ��� ������ �������        
        if (_itemInTube)
        {
            StartCoroutine(MoveToSpawner());
        }
        else
        {
            _waitForLose = StartCoroutine(WaitForLose());
            CollapseManager.Instance.OnCollapse.AddListener(ResetLoseTimer);
        }
    }        
    IEnumerator WaitForLose()
    {
        for (float t = 0f; t < 5f; t += Time.deltaTime)
        {
            yield return null;
        }
        // Lose
        Debug.Log("Lose");
    }

    private void ResetLoseTimer() // ������������� � �������� ��������, �������� ������ ������ ��� ��� ����������� �����
    {
        if (_waitForLose != null)
        {
            StopCoroutine(_waitForLose);
            _waitForLose = StartCoroutine(WaitForLose());
        }        
    }
}
