using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionsManager : MonoBehaviour
{
    public static CollectionsManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void OnBackButtonPress()
    {
        print("back button hit!");
        LevelManager.Instance.LoadScene("Main");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
