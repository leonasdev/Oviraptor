using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject pre;
    public GameObject player;

    private Collider2D _lastTouchedCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player") == false) return;

        if(col == _lastTouchedCol) return;

        GameObject anchor = Instantiate(pre, transform.position, Quaternion.identity);
        anchor.GetComponent<UseMotor>().player = player;

        _lastTouchedCol = col; 
    }
}
