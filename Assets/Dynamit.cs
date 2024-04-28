using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamit : ActiveItem
{
    [Header("Dynamit")]
    [SerializeField] private float _affectRadius = 1.5f; //������ ��������
    [SerializeField] private float _forceValue = 1000f; //���� ��������

    [SerializeField] private GameObject _affectArea; 
    [SerializeField] private GameObject _effectPrefab;

    protected override void Start()
    {
        base.Start();
        _affectArea.SetActive(false); //��������� ����-������ ��������
    }

    [ContextMenu("Explode")]
    public void Explode() //�������� �������
    {
        StartCoroutine(AffectProcess());
    }

    private IEnumerator AffectProcess()
    {
        _affectArea.SetActive(true); //�������� ����-������ ��������, ����� �� ������ ���� ���������
        _animator.enabled = true;
        yield return new WaitForSeconds(1f);
        Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnValidate() //����������, ����� �� ���-�� ������ � ���������� (����� ��������� ���� ��������� � ����������)
    {
        _affectArea.transform.localScale = Vector3.one * _affectRadius * 2f;
    }
}
