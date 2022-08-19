using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSpeed : MonoBehaviour
{
    public Text text;
    private float upSpeed;
    private float rightSpeed;
    void Start()
    {
        upSpeed = PlayerState.RolledUpSpeed;
        rightSpeed = PlayerState.VelocityLimitX;
        text.text = "Up speed: " + upSpeed.ToString() + "\n" + "Right speed: " + rightSpeed.ToString();
    }

    void Update()
    {
        upSpeed = PlayerState.RolledUpSpeed;
        rightSpeed = PlayerState.VelocityLimitX;
        text.text = "Up speed: " + upSpeed.ToString() + "\n" + "Right speed: " + rightSpeed.ToString();
    }
}
