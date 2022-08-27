using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuCanvas;

    [SerializeField]
    private GameObject _settingsButton, _collectionsButton, _tapToStart;

    [SerializeField]
    private TextMeshProUGUI _tapToStartText;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state)
    {
        // _tapToStart.SetActive(state == GameState.MainMenu);
        // _settingsButton.SetActive(state == GameState.MainMenu);
        // _collectionsButton.SetActive(state == GameState.MainMenu);
        // _tapToStartText.enabled = state == GameState.MainMenu;
        _mainMenuCanvas.SetActive(state == GameState.MainMenu);
    }

    public void OnSettingsButtonPress()
    {
        print("Settings Button hit!");
    }

    public void OnCollectionsButtonPress()
    {
        print("Collections Button hit!");
        LevelManager.Instance.LoadScene("Collections");
    }

    public void TapToStart()
    {
        print("Tap to Start hit!");
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
