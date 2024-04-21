using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    public int Level; //������� ��������� ��������
    public float Radius;
    [SerializeField] private TextMeshProUGUI _levelText; //����� �����

    [SerializeField] private Transform _visualTransform;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private SphereCollider _trigger;

    [ContextMenu("IncreaseLevel")]
    public void IncreaseLevel()
    {
        Level++;
        SetLevel(Level);
    }

    public void SetLevel(int level)
    {
        Level = level;
        //��������� ����� �� ���� - ((������� ������� + 1) � ������� 2)
        //0 ��. - 2, 1 ��. - 4, 2 ��. - 8, 3 ��. - 16, 4 ��. - 32, 5 ��. - 64 
        int number = (int)Mathf.Pow(2, level + 1);
        string numberString = number.ToString();
        _levelText.text = numberString;

        //������ ���� �������������, ����� ������������� ��� �������
        Radius = Mathf.Lerp(0.4f, 0.7f, level / 10f); //��� ������ 0 - ������ 0.4f, ��� ������ 10 - ������ 0.7f
        //������ ������� ���� � ������� ���������� � ��������
        Vector3 ballScale = Vector3.one * Radius * 2f;
        _visualTransform.localScale = ballScale;
        _collider.radius = Radius;
        _trigger.radius = Radius + 0.1f;
    }

}
