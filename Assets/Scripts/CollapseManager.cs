using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollapseManager : MonoBehaviour
{
    public UnityEvent OnCollapse;

    public static CollapseManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Collapse(ActiveItem itemA, ActiveItem itemB)
    {
        //Правило по которому itemA и itemB будут выбираться
        ActiveItem toItem;
        ActiveItem fromItem;
        //Если высота шаров по у отличается большее чем на 0.02f
        if (Mathf.Abs(itemA.transform.position.y - itemB.transform.position.y) > 0.02f)
        {
            //Если А выше чем В
            if (itemA.transform.position.y > itemB.transform.position.y)
            {
                fromItem = itemA;
                toItem = itemB;
            }
            else
            {
                fromItem = itemB;
                toItem = itemA;
            }
        }
        else
        {
            //Если скорость А больше чем скорость В
            if (itemA.Rigidbody.velocity.magnitude > itemB.Rigidbody.velocity.magnitude)
            {
                fromItem = itemA;
                toItem = itemB;
            }
            else
            {
                fromItem = itemB;
                toItem = itemA;
            }
        }

        StartCoroutine(CollapseProcess(fromItem, toItem));
    }

    public IEnumerator CollapseProcess(ActiveItem fromItem, ActiveItem toItem)
    {
        fromItem.Disable();

        // Если два шара, то въезжают друг в друга
        if (fromItem.ItemType == ItemType.Ball || toItem.ItemType == ItemType.Ball)
        {
            Vector3 startPosition = fromItem.transform.position;
            for (float t = 0; t < 1f; t += Time.deltaTime / 0.08f)
            {
                fromItem.transform.position = Vector3.Lerp(startPosition, toItem.transform.position, t);
                yield return null;
            }
            fromItem.transform.position = toItem.transform.position;
        }     

        if (fromItem.ItemType == ItemType.Ball && toItem.ItemType == ItemType.Ball)
        {
            fromItem.Die();
            toItem.DoEffect();
            ExplodeBall(toItem.transform.position, toItem.Radius + 0.15f);
        }
        else
        {
            // Если объект мяч - уничтожаем
            if (fromItem.ItemType == ItemType.Ball)
            {
                fromItem.Die();
            }
            else
            {
                // Если не мяч, воспроизводим эффект
                fromItem.DoEffect();
            }
            if (toItem.ItemType == ItemType.Ball)
            {
                toItem.Die();
            }
            else
            {
                toItem.DoEffect();
            }
        }

        OnCollapse.Invoke();
    }

    public void ExplodeBall(Vector3 position, float radius)
    {
        //Собираем в массив все коллайдеры, которые попали в сферу радиусом radius
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            PassiveItem passiveItem = colliders[i].GetComponentInParent<PassiveItem>();
            //if (colliders[i].attachedRigidbody)
            //{
            //    passiveItem = colliders[i].attachedRigidbody.GetComponent<PassiveItem>();
            //}
            if (passiveItem)
            {
                passiveItem.OnAffect();
            }
        }
    }
}
