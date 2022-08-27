using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool notDie = false;

    void Awake()
    {
        Instance = this;
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
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu()
    {
        player.GetComponent<throwhook>().enabled = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void HandlePlaying()
    {
        player.GetComponent<throwhook>().enabled = true;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void Update()
    {
        if(InputManager.Instance.ButtonDownCount > 0) {
            // UpdateGameState(GameState.Playing);
        }

        if(InputManager.Instance.ButtonUpCount > 0) {
            // InputManager.Instance.InputEnable = true;
        }
    }
}
