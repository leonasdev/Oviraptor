using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class throwhook : MonoBehaviour
{
    public GameObject hook;

    private GameObject curHook;

    private Rigidbody2D playerRigidbody;

    private bool ropeActive = false;
    private bool mouseHolding = false;
    private bool isBreak = false;

    public float endPoint_x = 10;
    public GameObject ceiling;

    private float rolledUpSpeed;

    public float rotateSpeed = 0.2f;
    public float rotateSpeedMin = 500f;

    public Animator animator;
    public Sprite breakedEgg;

    private Vector2 initPosition;

    // for RotateBuffer()
    public float targetAngle;
    public float rotateBufferSpeedDvided;
    private float t;

    public float velocityLimitX;
    public float velocityLimitDecaySpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initPosition = transform.position;
        playerRigidbody = GetComponent<Rigidbody2D>();

        PlayerState.RolledUpSpeed = 30;
        PlayerState.VelocityLimitX = 25;
    }

    void FixedUpdate()
    {
        if (mouseHolding)
        {
            Vector2 destiny = new Vector2(transform.position.x + endPoint_x, ceiling.transform.position.y);

            if (ropeActive == false)
            {
                // 實例Hook(參考物件, 目前位置(player的), 角度)
                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);
                curHook.name = "curHook";

                // 計算起點到終點的方向
                Vector2 direction = destiny - (Vector2)transform.position;
                direction = direction.normalized;
                // 從起點以direction的方向 尋找是否有物件(用LayerMask當filter)
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Obstacle"));

                // 如果發射方向有物件就射在物件上，沒有的話就射在終點上
                if (hit)
                {
                    // 讓接觸面在物件最上面
                    Vector2 hitPointTop = new Vector2(hit.point.x, hit.point.y - breakedEgg.bounds.size.y / 2);
                    curHook.GetComponent<RopeScript>().destiny = hitPointTop;
                }
                else
                {
                    // 將Hook終點設為終點
                    curHook.GetComponent<RopeScript>().destiny = destiny;
                }

                ropeActive = true;
            }

            if((Vector2)curHook.transform.position == curHook.GetComponent<RopeScript>().destiny)
            {
                isBreak = true;
            }

            if(isBreak)
            {
                // 腳色漸漸靠向destiny
                transform.position = Vector2.MoveTowards(transform.position, destiny, rolledUpSpeed * Time.deltaTime);
            }
            RotateBuffer();
        }
        else
        {
            if (ropeActive)
            {
                // delete rope
                Destroy(curHook);
                ropeActive = false;
                isBreak = false;
            }

            // 懸空時以固定速度旋轉
            playerRigidbody.transform.Rotate(0, 0, rotateSpeedMin +  playerRigidbody.velocity.x * rotateSpeed * Time.deltaTime);
            t = 0; // for RotateBuffer()
        }
        VelocityLimiter();
    }

    // Update is called once per frame
    void Update()
    {
        rolledUpSpeed = PlayerState.RolledUpSpeed;
        velocityLimitX = PlayerState.VelocityLimitX;
        mouseHolding = Input.GetMouseButton(0) && InputManager.Instance.InputEnable;

        if(mouseHolding)
        {
            animator.SetBool("IsSwing", true);
        }
        else
        {
            animator.SetBool("IsSwing", false);
        }

        if(curHook != null)
        {
            if(isBreak)
            {
                curHook.GetComponent<Animator>().SetBool("isBreak", true);
            }
            else
            {
                curHook.GetComponent<Animator>().SetBool("isBreak", false);
            }
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            PlayerState.RolledUpSpeed -= 5;
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            PlayerState.RolledUpSpeed += 5;
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            PlayerState.VelocityLimitX -= 5;
        }
        else if(Input.GetKeyUp(KeyCode.X))
        {
            PlayerState.VelocityLimitX += 5;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RotateBuffer() // 以固定轉向慢慢轉到目標角度
    {
        t += Time.deltaTime / rotateBufferSpeedDvided;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(transform.rotation.eulerAngles.z, targetAngle, 1));
    }

    void VelocityLimiter()
    {
        float velocity_x = playerRigidbody.velocity.x;

        if(velocity_x > 0 && velocity_x > velocityLimitX)
        {
            playerRigidbody.AddForce(new Vector2(velocityLimitX - velocity_x, 0) * velocityLimitDecaySpeed);
        }
        else if(velocity_x < 0 && -velocity_x > velocityLimitX)
        {
            // TODO: 往後甩的限速
            // playerRigidbody.AddForce(new Vector2(-velocityLimit_x - velocity_x, 0)*10);
        }
    }
}
