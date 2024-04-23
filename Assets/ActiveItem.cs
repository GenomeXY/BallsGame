using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    public int Level; //уровень активного элемента
    public float Radius;
    [SerializeField] private TextMeshProUGUI _levelText; //текст цифры

    [SerializeField] private Transform _visualTransform;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private SphereCollider _trigger;

    public Rigidbody Rigidbody;

    [ContextMenu("IncreaseLevel")]
    public void IncreaseLevel()
    {
        Level++;
        SetLevel(Level);
    }

    public virtual void SetLevel(int level)
    {
        Level = level;
        //Обновляем число на шаре - (2 в степени (уровень объекта + 1))
        //0 ур. - 2, 1 ур. - 4, 2 ур. - 8, 3 ур. - 16, 4 ур. - 32, 5 ур. - 64 и т.д.
        int number = (int)Mathf.Pow(2, level + 1);
        string numberString = number.ToString();
        _levelText.text = numberString;

        //Радиус шара увеличивается, когда увеличивается его уровень
        Radius = Mathf.Lerp(0.4f, 0.7f, level / 10f); //при уровне 0 - радиус 0.4f, при уровне 10 - радуис 0.7f
        //Меняем масштаб шара и радиусы коллайдера и триггера
        Vector3 ballScale = Vector3.one * Radius * 2f;
        _visualTransform.localScale = ballScale;
        _collider.radius = Radius;
        _trigger.radius = Radius + 0.1f;
    }

    //Устанавливаем Item в трубу наверху
    public void SetupToTube()
    {
        //Выключаем физику
        _trigger.enabled = false;
        _collider.enabled = false;
        Rigidbody.isKinematic = true;
        Rigidbody.interpolation = RigidbodyInterpolation.None;
    }

    public void DropItem()
    {
        //Делаем объект физическим
        _trigger.enabled = true;
        _collider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        //Отперенчиваем от Spawn
        transform.parent = null;
        //Задаем скорость вниз, чтобы летел быстрее
        Rigidbody.velocity = Vector3.down * 0.8f;
    }
}
