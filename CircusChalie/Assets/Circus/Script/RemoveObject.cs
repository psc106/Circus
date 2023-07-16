using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{

    public bool isActive = default;
    Scroll scroll;
    ScrollAndJump scrollJump;

    private void Start()
    {
        scroll = GetComponent<Scroll>();
        scrollJump = GetComponent<ScrollAndJump>();
        isActive = true;
    }

    public void DieToPress()
    {
        if (scroll != null || scrollJump != null)
        {
            if (scroll != null) Destroy(scroll);
            if (scrollJump != null) Destroy(scrollJump);

            StartCoroutine(OnPressDead());
        }
    }

    public IEnumerator OnPressDead()
    {

        for (int i = 0; i < 5; i++)
        {
            transform.localScale = new Vector2(1, transform.localScale.y * .5f) ;
            transform.position = new Vector3(transform.position.x, transform.position.y- transform.localScale.y*.5f);

            yield return new WaitForSeconds(.02f);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
