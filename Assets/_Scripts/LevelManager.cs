using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject _loaderCanvas;

    void Awake()
    {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do {
            print("loading...");
            await System.Threading.Tasks.Task.Delay(100);
        } while(scene.progress < 0.9f);

        await System.Threading.Tasks.Task.Delay(1000);

        scene.allowSceneActivation = true;

        _loaderCanvas.SetActive(false);
    }
}
