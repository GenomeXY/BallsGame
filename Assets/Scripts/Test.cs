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
            Debug.Log("Я лечу, я лечууу!");
            yield return null;
        }
    }

    private IEnumerator TransferTime()
    {
        var coroutine = StartCoroutine(FlyAnimation());
        yield return coroutine;
        Debug.Log("Я прилетел!");
        yield return null;
    }
}
