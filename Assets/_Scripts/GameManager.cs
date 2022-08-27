using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState 
{
    Playing,
    Collections,
    MainMenu,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    public GameObject player;

    public Vector3 initPlayerPos;

    public bool notDie = false;

    private int _score = 0;

    [SerializeField] private GameObject _playingCanvas;

    [SerializeField] private GameObject _gameOverCanvas;

    [SerializeField] private TextMeshProUGUI _scoreText;

    void Awake()
    {
        Instance = this;
        initPlayerPos = player.transform.position;
    }

    void Start()
    {
        UpdateGameState(GameState.MainMenu);
    } 

    public void UpdateGameState(GameState newState)
    {
        if (State == newState) return;
        
        State = newState;

        switch (newState) {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Collections:
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        _playingCanvas.SetActive(State == GameState.Playing);
        _gameOverCanvas.SetActive(State == GameState.GameOver);
        
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu()
    {
        player.transform.position = initPlayerPos;
        player.GetComponent<throwhook>().enabled = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void HandlePlaying()
    {
        player.transform.position = initPlayerPos;
        player.GetComponent<throwhook>().enabled = true;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void HandleGameOver()
    {
        player.GetComponent<throwhook>().enabled = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().rotation = 0;

        GameOverManager.Instance.OnGameOver();
    }

    public int GetScore()
    {
        return _score;
    }

    void Update()
    {
        _score = (int)player.transform.position.x / 10;
        _scoreText.SetText(_score.ToString());
    }
}
