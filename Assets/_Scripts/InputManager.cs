using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool InputEnable {get; set;}

    public int ButtonUpCount {get => buttonUpCount;}
    private int buttonUpCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            buttonUpCount++;
        }
    }
}
