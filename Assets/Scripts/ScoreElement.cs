using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreElement : MonoBehaviour
{
    public ItemType ItemType; 
    [SerializeField] public int CurrentScore; // сколько осталось собрать
    [SerializeField] private TextMeshProUGUI _text; 
    [SerializeField] public Transform IconTransform; // ссылка на картинку
    [SerializeField] private AnimationCurve _scaleCurve; // анимационная кривая
    [SerializeField] public int Level; // для шаров
    public GameObject FlyingIconPrefab; 

    // Добавить один Item к счету
    [ContextMenu("AddOne")]

    public void AddOne() // вызывается когда собрали элемент определенного типа
    {
        CurrentScore--;
        if (CurrentScore < 0 )
        {
            CurrentScore = 0;
        }
        _text.text = CurrentScore.ToString();
        // Запустить анимацию изменения счета
        StartCoroutine(AddAnimation());
        //ScoreManager.Instance.CheckWin();
    }

    public void Setup(int number) // устанавливает счет после создания иконки
    {
        CurrentScore = number;
        _text.text = number.ToString();
    }

    IEnumerator AddAnimation() // колебания иконки по кривой
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
