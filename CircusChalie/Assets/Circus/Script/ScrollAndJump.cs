using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAndJump : MonoBehaviour
{
    public float speed = default;

    public GameObject frontMonkey;

    private Rigidbody2D rb;
    public bool isJump;

    private void Start()
    {
        isJump = false;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (frontMonkey == null)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        else
        {
            if (transform.position.x - frontMonkey.transform.position.x <= 1.5f && transform.position.x - frontMonkey.transform.position.x > 0)
            {
                if (!isJump)
                {
                    isJump = true;
                    frontMonkey.GetComponent<Scroll>().speed = 0;
                    speed = 0;
                    rb.velocity = Vector3.zero;
                    rb.AddForce(new Vector2(-180, 500));
                }
            }
            else if (frontMonkey.transform.position.x - transform.position.x >= 2f)
            {
                if (isJump)
                {
                    isJump = false;
                    frontMonkey.GetComponent<Scroll>().speed = 4;
                    speed = 9;
                }
            }

            if (!isJump)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }
}
