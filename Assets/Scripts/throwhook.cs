using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class throwhook : MonoBehaviour
{
    public GameObject hook;

    GameObject curHook;

    Rigidbody2D playerRigidbody;

    bool ropeActive = false;
    bool mouseHolding = false;
    bool isBreak = false;

    public float endPoint_x = 10;
    public float endPoint_y = 32;

    private float rolledUpSpeed;

    public float rotateSpeed = 0.2f;
    public float rotateSpeedMin = 500f;

    public Animator animator;
    public Sprite breakedEgg;

    private Vector2 initPosition;

    // for RotateBuffer()
    public float targetAngle;
    public float rotateBufferSpeedDvided;
    float t;

    public float velocityLimitX;
    public float velocityLimitDecaySpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initPosition = transform.position;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (mouseHolding)
        {
            Vector2 destiny = new Vector2(transform.position.x + endPoint_x, endPoint_y);

            if (ropeActive == false)
            {
                // ���Hook(�ѦҪ���, �ثe��m(player��), ����?)
                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);
                curHook.name = "curHook";

                // �p��_�I����I����V
                Vector2 direction = destiny - (Vector2)transform.position;
                direction = direction.normalized;
                // �q�_�I�Hdirection����V �M��O�_������(��LayerMask��filter)
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Obstacle"));

                // �p�G�o�g��V������N�g�b����W�A�S�����ܴN�g�b���I�W
                if (hit)
                {
                    // ����Ĳ���b����̤W��
                    Vector2 hitPointTop = new Vector2(hit.point.x, hit.point.y - breakedEgg.bounds.size.y / 2);
                    curHook.GetComponent<RopeScript>().destiny = hitPointTop;
                }
                else
                {
                    // �NHook���I�]�����I
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
                // �}�⺥���a�Vdestiny
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

            // �a�ŮɥH�T�w�t�ױ���
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
        mouseHolding = Input.GetMouseButton(0);

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

    void RotateBuffer() // �H�T�w��V�C�C���ؼШ���
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
            // TODO: ����Ϫ����t
            // playerRigidbody.AddForce(new Vector2(-velocityLimit_x - velocity_x, 0)*10);
        }
    }
}
