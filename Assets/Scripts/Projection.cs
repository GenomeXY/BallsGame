using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projection : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Transform _visualTransform;

    public void Setup(Material material, string numberText, float radius)
    {
        _renderer.material = material;
        _levelText.text = numberText;
        _visualTransform.localScale = Vector3.one * radius * 2;
    }

    public void SetupLevelText(string numberText)
    {
        _levelText.text = numberText;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
