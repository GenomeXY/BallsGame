using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreElement : MonoBehaviour
{
    public ItemType ItemType; 
    [SerializeField] public int CurrentScore; // ������� �������� �������
    [SerializeField] private TextMeshProUGUI _text; 
    [SerializeField] public Transform IconTransform; // ������ �� ��������
    [SerializeField] private AnimationCurve _scaleCurve; // ������������ ������
    [SerializeField] public int Level; // ��� �����
    public GameObject FlyingIconPrefab; 

    // �������� ���� Item � �����
    [ContextMenu("AddOne")]

    public void AddOne() // ���������� ����� ������� ������� ������������� ����
    {
        CurrentScore--;
        if (CurrentScore < 0 )
        {
            CurrentScore = 0;
        }
        _text.text = CurrentScore.ToString();
        // ��������� �������� ��������� �����
        StartCoroutine(AddAnimation());
        //ScoreManager.Instance.CheckWin();
    }

    public void Setup(int number) // ������������� ���� ����� �������� ������
    {
        CurrentScore = number;
        _text.text = number.ToString();
    }

    IEnumerator AddAnimation() // ��������� ������ �� ������
    {
        for (float t = 0; t < 1f; t += Time.deltaTime * 1.8f)
        {
            float scale = _scaleCurve.Evaluate(t);
            IconTransform.localScale = Vector3.one * scale;
            yield return null;
        }
        IconTransform.localScale = Vector3.one;
    }
}
