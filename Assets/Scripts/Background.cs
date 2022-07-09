using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    private float length, startPosX;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;
    }

    void Update()
    {
        
    }
}
