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
                    newScoreElement.Setup(task); //task.Level, this
                    // Добавляем ScoreElement в массив
                    ScoreElements[taskIndex] = newScoreElement;
                }
            }
        }
    }

    public bool AddScore(ItemType itemType, Vector3 position, int level = 0) // вызывается при уничтожении объекта 
    {
        for (int i = 0; i < ScoreElements.Length; i++)
        {
            //if (ScoreElements[i].ItemType != itemType) continue;
            //if (ScoreElements[i].CurrentScore == 0) continue;
            //if (ScoreElements[i].Level != level) continue;
            //StartCoroutine(AddScoreAnimation(ScoreElements[i], position)); //все это тоже самое, что и код ниже

            if (ScoreElements[i].ItemType == itemType)
            {
                if (ScoreElements[i].CurrentScore !=0)
                {
                    if (ScoreElements[i].Level == level)
                    {
                        StartCoroutine(AddScoreAnimation(ScoreElements[i], position));
                        return true;
                    }
                }
            }
        }
        return false;
    }

    IEnumerator AddScoreAnimation(ScoreElement scoreElement, Vector3 position) //для создания иконки-элемента и её перелета-движения в UI
    {
        GameObject icon = Instantiate(scoreElement.FlyingIconPrefab, position, Quaternion.identity);
        // Строим кривую Безье по 4 точкам
        Vector3 a = position; 
        Vector3 b = position + Vector3.back * 6.5f + Vector3.down * 5f;
        Vector3 screenPosition = new Vector3(scoreElement.IconTransform.position.x, scoreElement.IconTransform.position.y, -_camera.transform.position.z);
        Vector3 d = _camera.ScreenToWorldPoint(screenPosition); // точка прилета (фиинальная точка)
        Vector3 c = d + Vector3.back * 6f;

        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            icon.transform.position = Bezier.GetPoint(a, b, c, d, t);
            yield return null;
        }
        Destroy(icon.gameObject);
        scoreElement.AddOne();

        // Проверить условие победы
        CheckWin();
    }

    public void CheckWin() // проверка условия победы
    {
        for (int i = 0; i < ScoreElements.Length; i++)
        {
            if (ScoreElements[i].CurrentScore != 0)
            {
                return;
            }
        }
        Debug.Log("Win");
        GameManager.Instance.Win();
    }
}
