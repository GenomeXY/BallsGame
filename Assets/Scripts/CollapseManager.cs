using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseManager : MonoBehaviour
{
    public static CollapseManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Collapse(ActiveItem itemA, ActiveItem itemB)
    {
        StartCoroutine(CollapseProcess(itemA, itemB));
    }

    public IEnumerator CollapseProcess(ActiveItem itemA, ActiveItem itemB)
    {
        itemA.Disable();
        Vector3 startPosition = itemA.transform.position;
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.08f)
        {
            itemA.transform.position = Vector3.Lerp(startPosition, itemB.transform.position, t);
            yield return null;
        }
        itemA.transform.position = itemB.transform.position;
        itemA.Die();
        itemB.IncreaseLevel();

        ExplodeBall(itemB.transform.position, itemB.Radius + 0.15f);
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
