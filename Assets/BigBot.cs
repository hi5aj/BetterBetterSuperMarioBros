using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBot : MonoBehaviour
{    
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D rb;
    BoxCollider2D collider;

    public AudioSource audiosource;
    public AudioClip enemyDeathClip;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
                if (IsFacingRight())
        {
            rb.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    

private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
        //if (!collider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        //{
          //  transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
        //}
   // }

   // private void OnTriggerEnter2D(Collider2D collision)
    //{
       // if (collider.IsTouchingLayers(LayerMask.GetMask("Ground")))
       // {
       //     transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
       // }
   // }

   // private void OnCollisionEnter2D(Collision2D other)
    //{
       // Player_Controller player = other.gameObject.GetComponent<Player_Controller>();

       // if (player != null)
       // {
        //    player.Damage();
        //}
   // }

    public void KillEnemy()
    {
        audiosource.PlayOneShot(enemyDeathClip);
        Destroy(gameObject);
    }
}
