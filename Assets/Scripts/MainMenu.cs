using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private Button _startButton;
    private void Start()
    {
        _coinsText.text = Progress.Instance.Coins.ToString();
        _levelText.text = "Level " + (Progress.Instance.Level).ToString();
        _startButton.onClick.AddListener(StartLevel);
    }

    void StartLevel()
    {
        SceneManager.LoadScene(Progress.Instance.Level);
    }
}
