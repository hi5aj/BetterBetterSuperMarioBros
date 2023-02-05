using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{   
    public Transform player;
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer mySpriteRenderer;
    Animator animator;
    bool alive = true;

    float activationDistance = 5f;
    public GameObject Drone;

    //public int hitCount;

    AudioSource audioSource;
    //public GameObject dogbark;
    //public AudioClip dogyelp;



    // Start is called before the first frame update
    //void Awake()
    //{
        //rb = this.GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
       // SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();
        //animator.SetTrigger("Idle");
        //audioSource = GetComponent<AudioSource>();
        //dogbark.SetActive(false);
        

    //}

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = player.position - transform.position;
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

        
        //if (player.GetComponent<Player_Controller>().currentHealth == 0)
        //{
        //    GetComponent<Rigidbody2D>().simulated = false;
        //    animator.SetTrigger("Idle");
        //    dogbark.SetActive(false);
        //}
            
    }
    private void FixedUpdate() 
    {

        moveCharacter(movement);
        
    }
    void moveCharacter(Vector2 direction)
    {
        if ((Drone.transform.position - player.transform.position).magnitude < activationDistance && alive == true)
        {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        //animator.SetTrigger("Idle");

        //dogbark.SetActive(true);

        }
        else if ((Drone.transform.position - player.transform.position).magnitude > activationDistance)
        {
            //animator.SetTrigger("Idle");        
            //dogbark.SetActive(false);        
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
    
    public void KillEnemy()
    {
        Destroy(gameObject);
    }
}
