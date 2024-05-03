using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveItem : Item
{
    public int Level; //уровень активного элемента
    public float Radius;
    [SerializeField] protected TextMeshProUGUI _levelText; //текст цифры      
    [SerializeField] protected SphereCollider _collider;
    [SerializeField] protected SphereCollider _trigger;

    public Rigidbody Rigidbody;
    public bool IsDead;

    [SerializeField] protected Animator _animator;

    public Projection Projection;

    protected virtual void Start()
    {
        Projection.Hide();
    }

    [ContextMenu("IncreaseLevel")]
    public void IncreaseLevel()
    {
        Level++;
        SetLevel(Level);
        _animator.SetTrigger("IncreaseBallLevel");

        _trigger.enabled = false;
        Invoke(nameof(EnableTrigger), 0.08f);
    }

    public virtual void SetLevel(int level)
    {
        Level = level;
        //Обновляем число на шаре - (2 в степени (уровень объекта + 1))
        //0 ур. - 2, 1 ур. - 4, 2 ур. - 8, 3 ур. - 16, 4 ур. - 32, 5 ур. - 64 и т.д.
        int number = (int)Mathf.Pow(2, level + 1);
        string numberString = number.ToString();
        _levelText.text = numberString;       
    }

    private void EnableTrigger()
    {
        _trigger.enabled = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if(IsDead) return;

        if (other.attachedRigidbody)
        {
            ActiveItem otherItem = other.attachedRigidbody.GetComponent<ActiveItem>();
            if (otherItem)
            {
                if (!otherItem.IsDead && Level == otherItem.Level)
                {
                    CollapseManager.Instance.Collapse(this, otherItem);
                }
            }
        }        
    }

    public void Disable()
    {
        _trigger.enabled = false;
        Rigidbody.isKinematic = true;
        _collider.enabled = false;
        IsDead = true;
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    public virtual void DoEffect ()
    {

    }
}
