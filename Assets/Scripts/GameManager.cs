using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winObject;
    [SerializeField] private GameObject _loseObject;

    public static GameManager Instance;

    public UnityEvent OnWin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Win()
    {
        _winObject.SetActive(true);
        OnWin.Invoke();
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        Progress.Instance.SetLevel(currentLevelIndex + 1);
        Progress.Instance.AddCoins(50);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(Progress.Instance.Level);
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Lose()
    {
        _loseObject.SetActive(true);
    }

}
