using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSt2 : MonoBehaviour
{
    public float speed = default;
    public float jumpPower = default;

    public int direction = default;
    private int tmpScore = 0;

    public bool isLive = default;
    public bool isJump = default;
    public bool isInvicible = default;

    private int scoreCount = 0;

    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    public AudioClip audioJump;
    public AudioClip audioScore;
    public AudioClip audioTrap;
    public AudioClip audioClear;
    public AudioClip audioHead;

    public FloatingJoystick joystick;
    public Button button;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isJump = false;
        isLive = true;
        isInvicible = false;
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLive)
        {
            if (!isJump)
            {
                Vector3 velocity = new Vector3(0f, 0f, 0f);
                rb.velocity = velocity;

                if (Input.GetAxis("Horizontal") == 0 && joystick.Horizontal == 0)
                {
                    animator.SetBool("Move", false);
                    animator.SetBool("BackMove", false);
                }
            }
            if (Input.GetKeyDown(KeyCode.Z) && !isJump && !isInvicible)
            {
                scoreCount = 0;
                tmpScore = 0;
                animator.SetBool("Jump", true);
                audioSource.PlayOneShot(audioJump);

                isJump = true;
                rb.velocity = Vector3.zero;
                if (direction > 0)
                {
                    rb.AddForce(new Vector2(speed*40, jumpPower));
                }
                else if (direction < 0)
                {
                    rb.AddForce(new Vector2(-speed * 40, jumpPower));
                }
                else
                {
                    rb.AddForce(new Vector2(0, jumpPower));
                }
            }

            if (!GameManager.instance.isJoystickActivate)
            {
                if (!isJump && !isInvicible)
                {
                    float x = Input.GetAxis("Horizontal");

                    if (x > 0)
                    {
                        float xSpeed = x * speed;

                        Vector3 velocity = new Vector3(xSpeed, 0f, 0f);

                        rb.velocity = velocity;
                        direction = 1;
                    }
                    else if (x < 0)
                    {
                        float xSpeed = x * speed * 0.7f;

                        Vector3 velocity = new Vector3(xSpeed, 0f, 0f);

                        rb.velocity = velocity;
                        direction = -1;
                    }
                    else
                    {
                        direction = 0;
                    }

                }
                if (!isInvicible)
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        animator.SetBool("BackMove", false);
                        animator.SetBool("Move", true);
                    }
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        animator.SetBool("Move", false);
                        animator.SetBool("BackMove", true);
                    }
                    if (Input.GetKeyUp(KeyCode.RightArrow))
                    {
                        animator.SetBool("Move", false);
                    }
                    if (Input.GetKeyUp(KeyCode.LeftArrow))
                    {
                        animator.SetBool("BackMove", false);
                    }
                }
            }
            else
            {
                float x = joystick.Horizontal;
                if (!isJump && !isInvicible)
                {

                    if (x > 0)
                    {
                        float xSpeed = x * speed;

                        Vector3 velocity = new Vector3(xSpeed, 0f, 0f);

                        rb.velocity = velocity;
                        direction = 1;
                    }
                    else if (x < 0)
                    {
                        float xSpeed = x * speed * 0.7f;

                        Vector3 velocity = new Vector3(xSpeed, 0f, 0f);

                        rb.velocity = velocity;
                        direction = -1;
                    }
                    else
                    {
                        direction = 0;
                    }

                }
                if (!isInvicible)
                {
                    if (x > 0)
                    {
                        animator.SetBool("BackMove", false);
                        animator.SetBool("Move", true);
                    }
                    if (x < 0)
                    {
                        animator.SetBool("Move", false);
                        animator.SetBool("BackMove", true);
                    }
                }
            }
        }

    }

    private void Die()
    {
        isLive = false;
        animator.SetTrigger("Dead");

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        tmpScore = 0;

        if (GameManager.instance.life == 0)
        {

            GameManager.instance.OnPlayerDead();
        }
        else
        {
            GameManager.instance.OnStaged2Fail(this);
        }
    }

    private void Win()
    {
        animator.SetTrigger("Win");

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

        GameManager.instance.OnStagedClear();
    }

    public void Jump()
    {
        if (!isJump && !isInvicible)
        {
            scoreCount = 0;
            tmpScore = 0;
            animator.SetBool("Jump", true);
            audioSource.PlayOneShot(audioJump);

            isJump = true;
            rb.velocity = Vector3.zero;
            if (direction > 0)
            {
                rb.AddForce(new Vector2(speed * 40, jumpPower));
            }
            else if (direction < 0)
            {
                rb.AddForce(new Vector2(-speed * 40, jumpPower));
            }
            else
            {
                rb.AddForce(new Vector2(0, jumpPower));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Score")
        {
            Gold gold = collision.GetComponent<Gold>();
            if (!gold.isTouched)
            {
                tmpScore += gold.score;
                gold.isTouched = true;
                scoreCount += 1;

                if (gold.isMonkey)
                {
                    if (gold.GetComponentInParent<ScrollAndJump>().isJump)
                    {
                        tmpScore += 500;
                    }
                }
            }
        }
        
        if (isLive && isJump && collision.tag == "Head")
        {
            float heightDistance = transform.position.y - collision.transform.position.y;
            GlobalFunction.Log(heightDistance);
            if (heightDistance <= 1.2 && heightDistance >= 1.15){
                RemoveObject tmp = collision.GetComponentInParent<RemoveObject>();
                if (tmp.isActive)
                {
                    audioSource.PlayOneShot(audioHead);
                    tmp.isActive = false;
                    rb.velocity = Vector3.zero;
                    rb.AddForce(new Vector2(0, jumpPower / 2));

                    scoreCount += 1;
                    tmpScore += 500;

                    tmp.DieToPress();
                }
            }
        }

        if (isLive && collision.tag == "Trap")
        {
            RemoveObject tmp = collision.GetComponentInParent<RemoveObject>();
            if (tmp.isActive)
            {
                animator.SetBool("Move", false);
                animator.SetBool("BackMove", false);
                animator.SetBool("Jump", false);
                if (isLive)
                {
                    audioSource.PlayOneShot(audioTrap);
                    Die();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.contacts[0].normal.y > 0.7)
        {
            if (collision.gameObject.tag == "Floor" && isJump)
            {
                if (tmpScore > 0)
                {
                    if (scoreCount >= 2)
                    {
                        tmpScore += 100;
                    }

                    if (tmpScore > 600)
                    {
                        audioSource.PlayOneShot(audioScore);
                    }
                    GameManager.instance.AddScore(tmpScore);
                }

                animator.SetBool("Jump", false);

                rb.velocity = Vector3.zero;
                isJump = false;
            }

            if (collision.gameObject.tag == "End" && isJump)
            {

                audioSource.PlayOneShot(audioClear);
                isInvicible = true;
                animator.SetBool("Jump", false);

                isJump = false;

                rb.velocity = Vector3.zero;
                rb.gravityScale = 0;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                StartCoroutine(Clear(collision.transform.position.x));
            }
        }
    }

    IEnumerator Clear(float x)
    {
        int direction = 0;
        if (transform.position.x - x < 0)
        {
            direction = 1;
            animator.SetBool("BackMove", true);
        }
        else if (transform.position.x - x > 0)
        {
            direction = -1;
            animator.SetBool("Move", true);
        }

        float currX = transform.position.x;

        while (currX != x)
        {
            if (direction == 1)
            {
                currX += .2f;
                transform.position = new Vector2(currX, transform.position.y);
            }
            else if (direction == -1)
            {
                currX -= .2f;
                transform.position = new Vector2(currX, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(currX, transform.position.y);
            }
            if (Mathf.Abs(currX - x) < .2f)
            {
                currX = x;
            }
            yield return new WaitForSeconds(.05f);
        }

        Win();

    }
}
