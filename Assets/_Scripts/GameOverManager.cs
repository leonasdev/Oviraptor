using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [SerializeField] private GameObject _gameOverCanvas;

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private TextMeshProUGUI _highesetScoreText;

    void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    void Destroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state)
    {
    }

    public async void OnGameOver()
    {
        int score = GameManager.Instance.GetScore();
        _scoreText.SetText(score.ToString());

        if(score > PlayerPrefs.GetInt("score")) {
            PlayerPrefs.SetInt("score", score);
            _highesetScoreText.SetText(PlayerPrefs.GetInt("score").ToString());
            await SaveHighestScore(score);
        } else {
            _highesetScoreText.SetText(PlayerPrefs.GetInt("score").ToString());
        }
    }

    public void OnMenuButtonPress()
    {
        // GameManager.Instance.UpdateGameState(GameState.MainMenu);
        SceneManager.LoadScene("Main");
    }

    public void OnRetryButtonPress()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    private async Task SaveHighestScore(int score)
    {
        Dictionary<string, object> element = new Dictionary<string, object>();

        element.Add("highest_score", score);

        await CloudSaveService.Instance.Data.ForceSaveAsync(element);

        Debug.Log($"Successfully saved highest_score:{score}");
    }

    [MenuItem("Custom/ClearPlayerPrefs")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
