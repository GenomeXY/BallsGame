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
        //������� �� �������� itemA � itemB ����� ����������
        ActiveItem toItem;
        ActiveItem fromItem;
        //���� ������ ����� �� � ���������� ������� ��� �� 0.02f
        if (Mathf.Abs(itemA.transform.position.y - itemB.transform.position.y) > 0.02f)
        {
            //���� � ���� ��� �
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
            //���� �������� � ������ ��� �������� �
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
        Vector3 startPosition = fromItem.transform.position;
        for (float t = 0; t < 1f; t += Time.deltaTime / 0.08f)
        {
            fromItem.transform.position = Vector3.Lerp(startPosition, toItem.transform.position, t);
            yield return null;
        }
        fromItem.transform.position = toItem.transform.position;
        fromItem.Die();
        toItem.IncreaseLevel();

        ExplodeBall(toItem.transform.position, toItem.Radius + 0.15f);
    }

    public void ExplodeBall(Vector3 position, float radius)
    {
        //�������� � ������ ��� ����������, ������� ������ � ����� �������� radius
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