using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingFlooring : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
    }
}
