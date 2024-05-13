using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // „увствительность
    [SerializeField] private float _sencentivity = 25f;
    // ћаксимальное рассто€ние на которое может смещатьс€ Spawner
    [SerializeField] private float _maxXposition = 2.5f;

    private float _xPosition;
    // ѕозици€ мыши по ’ в предыдущем кадре
    private float _oldMouseX;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _oldMouseX = Input.mousePosition.x; //запоминаем координату мыши по ’ при нажатии кнопки мыши
        }

        if (Input.GetMouseButton(0))
        {
            float delta = Input.mousePosition.x - _oldMouseX; //на сколько пикселей сместилась мышь по сравнению с предыдущим кадром
            _oldMouseX = Input.mousePosition.x;
            _xPosition += delta * _sencentivity / Screen.width; //Screen.width - учитываем ширину экрана в пиксел€х
            _xPosition = Mathf.Clamp(_xPosition, -_maxXposition, _maxXposition);
            transform.position = new Vector3(_xPosition, transform.position.y, transform.position.z);
        }
    }

    // вариант дл€ движени€ Spawner просто за курсоромм мышки (без зажати€ клавиши)

    //private void Start()
    //{
    //    _oldMouseX = Input.mousePosition.x;
    //}

    //private void Update()
    //{
    //    float delta = Input.mousePosition.x - _oldMouseX; //на сколько пикселей сместилась мышь по сравнению с предыдущим кадром
    //    _oldMouseX = Input.mousePosition.x;
    //    _xPosition += delta * _sencentivity / Screen.width; //Screen.width - учитываем ширину экрана в пиксел€х
    //    _xPosition = Mathf.Clamp(_xPosition, -_maxXposition, _maxXposition);
    //    transform.position = new Vector3(_xPosition, transform.position.y, transform.position.z);
    //}
}
