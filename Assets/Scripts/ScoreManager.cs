using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Level Level;
    // �������
    public ScoreElement[] ScoreElementPrefabs;
    // ��������� ��������
    public ScoreElement[] ScoreElements;
    public Transform ItemScoreParent;
    [SerializeField] private Camera _camera;

    public static ScoreManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ScoreElements = new ScoreElement[Level.Tasks.Length];
        // ���������� �� ���� ������� ������
        for (int taskIndex = 0; taskIndex < Level.Tasks.Length; taskIndex++)
        {
            // ������
            Task task = Level.Tasks[taskIndex];
            // ��� �������, ������� ���� �������
            ItemType itemType = task.ItemType;
            // ���� ������, ������� ������������� ����� ��������
            for (int i = 0; i < ScoreElementPrefabs.Length; i++)
            {
                if (itemType == ScoreElementPrefabs[i].ItemType)
                {
                    // ������� ����� ScoreElement
                    ScoreElement newScoreElement = Instantiate(ScoreElementPrefabs[i], ItemScoreParent);
                    newScoreElement.Setup(task.Number); //task.Level, this
                    // ��������� ScoreElement � ������
                    ScoreElements[taskIndex] = newScoreElement;
                }
            }
        }
    }
}
