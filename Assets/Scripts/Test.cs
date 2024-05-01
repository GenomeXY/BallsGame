using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TransferTime());
    }

    private IEnumerator FlyAnimation()
    {
        for (float t = 0; t < 2f; t += Time.deltaTime) 
        {
            Debug.Log("� ����, � ������!");
            yield return null;
        }
    }

    private IEnumerator TransferTime()
    {
        var coroutine = StartCoroutine(FlyAnimation());
        yield return coroutine;
        Debug.Log("� ��������!");
        yield return null;
    }
}
