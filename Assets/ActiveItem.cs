using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    public int Level; //������� ��������� ��������
    public float Radius;
    [SerializeField] protected TextMeshProUGUI _levelText; //����� �����

    [SerializeField] private Transform _visualTransform;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private SphereCollider _trigger;

    public Rigidbody Rigidbody;
    public bool IsDead;

    [SerializeField] private Animator _animator;

    public Projection Projection;

    private void Start()
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
        //��������� ����� �� ���� - (2 � ������� (������� ������� + 1))
        //0 ��. - 2, 1 ��. - 4, 2 ��. - 8, 3 ��. - 16, 4 ��. - 32, 5 ��. - 64 � �.�.
        int number = (int)Mathf.Pow(2, level + 1);
        string numberString = number.ToString();
        _levelText.text = numberString;

        //������ ���� �������������, ����� ������������� ��� �������
        Radius = Mathf.Lerp(0.4f, 0.7f, level / 10f); //��� ������ 0 - ������ 0.4f, ��� ������ 10 - ������ 0.7f
        //������ ������� ���� � ������� ���������� � ��������
        Vector3 ballScale = Vector3.one * Radius * 2f;
        _visualTransform.localScale = ballScale;
        _collider.radius = Radius;
        _trigger.radius = Radius + 0.1f;        
    }

    private void EnableTrigger()
    {
        _trigger.enabled = true;
    }

    //������������� Item � ����� �������
    public void SetupToTube()
    {
        //��������� ������
        _trigger.enabled = false;
        _collider.enabled = false;
        Rigidbody.isKinematic = true;
        Rigidbody.interpolation = RigidbodyInterpolation.None;
    }

    public void DropItem()
    {
        //������ ������ ����������
        _trigger.enabled = true;
        _collider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        //������������� �� Spawn
        transform.parent = null;
        //������ �������� ����, ����� ����� �������
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
}
