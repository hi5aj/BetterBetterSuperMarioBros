using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{   
    public Transform target;
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    Animator animator;
    bool alive = true;

    public bool facingRight = true;

    float activationDistance = 5f;
    public GameObject FlyingEnemy;


    public AudioSource audiosource;
    public AudioClip enemyDeathClip;


    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        audiosource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        movement = direction;
        
        
        

            if (movement.x <= 0.01f && alive == true)
            {
                transform.localScale = new Vector3(-.3f, .3f, .3f);

            }
            else if (movement.x >= -0.01f && alive == true)
            {
                transform.localScale = new Vector3(.3f, .3f, .3f);

            }

            
    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player_Controller player = other.gameObject.GetComponent<Player_Controller>();

        if (player != null)
        {
            player.Damage();
        }
    }

    private void FixedUpdate() 
    {

        moveCharacter(movement);

                float h = Input.GetAxis("Horizontal");
        if(h > 0 && !facingRight)
            Flip();
        else if(h < 0 && facingRight)
            Flip();
        
    }
    void moveCharacter(Vector2 direction)
    {
        if ((FlyingEnemy.transform.position - target.transform.position).magnitude < activationDistance && alive == true)
        {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));


        }
       
    }

    public void KillEnemy()
    {
        audiosource.PlayOneShot(enemyDeathClip);
        Destroy(gameObject);
        alive = false;
    }

    void Flip ()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
