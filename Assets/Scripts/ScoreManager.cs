using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Level Level;
    // Префабы
    public ScoreElement[] ScoreElementPrefabs;
    // Созданные элементы
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
        // Проходимся по всем задачам уровня
        for (int taskIndex = 0; taskIndex < Level.Tasks.Length; taskIndex++)
        {
            // Задача
            Task task = Level.Tasks[taskIndex];
            // Тип объекта, который надо собрать
            ItemType itemType = task.ItemType;
            // Ищем префаб, который соответствует этому элементу
            for (int i = 0; i < ScoreElementPrefabs.Length; i++)
            {
                if (itemType == ScoreElementPrefabs[i].ItemType)
                {
                    // Создаем новый ScoreElement
                    ScoreElement newScoreElement = Instantiate(ScoreElementPrefabs[i], ItemScoreParent);
                    newScoreElement.Setup(task.Number); //task.Level, this
                    // Добавляем ScoreElement в массив
                    ScoreElements[taskIndex] = newScoreElement;
                }
            }
        }
    }
}
