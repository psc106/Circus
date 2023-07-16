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

    int scoreCount = 0;

    Animator animator;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
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
            if (Input.GetKeyDown(KeyCode.Z) && !isJump && !isInvicible)
            {
                scoreCount = 0;
                tmpScore = 0;
                animator.SetBool("Jump", true);

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
                    float xSpeed = x * speed *0.7f;

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
            GameManager.instance.OnStagedFail(this);
        }
    }

    private void Win()
    {
        animator.SetTrigger("Win");

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

        GameManager.instance.OnStagedClear();
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
            RemoveObject tmp = collision.GetComponentInParent<RemoveObject>();
            if (tmp.isActive)
            {
                tmp.isActive = false;
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector2(0, jumpPower / 2));

                scoreCount += 1;
                tmpScore += 500;

                Destroy(tmp.gameObject);
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
                    GameManager.instance.AddScore(tmpScore);
                }

                animator.SetBool("Jump", false);

                rb.velocity = Vector3.zero;
                isJump = false;
            }

            if (collision.gameObject.tag == "End" && isJump)
            {
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

        while (currX == x)
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
