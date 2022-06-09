using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwhook : MonoBehaviour
{
    public GameObject hook;

    GameObject curHook;

    bool ropeActive = false;
    bool mouseHolding = false;

    public float endPoint_x = 10;
    public float endPoint_y = 32;

    public float rolledUpSpeed = 0.1f;

    public float rotateSpeed = 0.2f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (mouseHolding)
        {
            Vector2 destiny = new Vector2(transform.position.x + endPoint_x, endPoint_y);
            if (ropeActive == false)
            {
                // 實例Hook(參考物件, 目前位置(player的), 角度?)
                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);

                // 將Hook終點設為終點
                curHook.GetComponent<RopeScript>().destiny = destiny;

                ropeActive = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, destiny, rolledUpSpeed * Time.deltaTime);
            // GetComponent<Rigidbody2D>().gravityScale = 8;
        }
        else
        {
            if (ropeActive)
            {
                // delete rope
                Destroy(curHook);

                ropeActive = false;
            }
            GetComponent<Rigidbody2D>().transform.Rotate(0, 0, GetComponent<Rigidbody2D>().velocity.x * rotateSpeed * Time.deltaTime);
            // GetComponent<Rigidbody2D>().gravityScale = 8;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mouseHolding = Input.GetMouseButton(0);

        if(mouseHolding)
        {
            animator.SetBool("IsSwing", true);
        }
        else
        {
            animator.SetBool("IsSwing", false);
        }
    }
}
