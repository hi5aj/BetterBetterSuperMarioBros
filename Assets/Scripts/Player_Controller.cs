using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player_Controller : MonoBehaviour
{
    //public static int levelCounter;
    //public GameObject portal;

    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;

    // Animator switcher
    public RuntimeAnimatorController unarmedController;
    public RuntimeAnimatorController gunController;

    // Invincibility frames
    bool isInvincible;
    float invincibleTimer;

    // Gun mechanics
    bool isGun = false;
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Player audio
    public AudioSource audiosource;
    public AudioClip jumpClip;
    public AudioClip shootClip;
    public AudioClip hurtClip;
    public AudioClip pickUpClip;
    

    // Death
    bool isDead = false;

    // Update is called once per frame
    void Update() {
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        if (!wasOnGround && onGround) {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }

        if (Input.GetButtonDown("Jump") && isDead == false) {
            jumpTimer = Time.time + jumpDelay;
        }

        animator.SetBool("onGround", onGround);
        if (isDead == false)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetButtonDown("Fire1") && isGun == true && isDead == false || Input.GetKeyDown(KeyCode.Z) && isGun == true && isDead == false)
        {
            animator.SetTrigger("isShooting");
            Shoot();
        }

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //levelCounter = 0;
        }

         if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        
        }
       // if () //entering a box collider for the end of the map invokes level change.
    }
    void FixedUpdate() {
        moveCharacter(direction.x);
        if(jumpTimer > Time.time && onGround){
            Jump();
            //onGround = false;
        }

        modifyPhysics();
    }
    void moveCharacter(float horizontal) {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)) {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vertical",rb.velocity.y);
    }
    void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = new Vector2(rb.velocity.y, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        animator.SetTrigger("Jump");
        audiosource.PlayOneShot(jumpClip);
        //StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    void modifyPhysics() {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if(onGround){
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections) {
                rb.drag = linearDrag;
            } else {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }else{
            rb.gravityScale = gravity;
            
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0){
                rb.gravityScale = gravity * fallMultiplier;
            }else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }
    void Flip() {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds) {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    public void PickupGun()
    {
        isGun = true;
        animator.runtimeAnimatorController = gunController as RuntimeAnimatorController;
        audiosource.PlayOneShot(pickUpClip);
    }

    public void Damage()
    {
        if (isGun == false)
        {
            Die();
        }
        else
        {
            isGun = false;
            audiosource.PlayOneShot(hurtClip);
            animator.runtimeAnimatorController = unarmedController as RuntimeAnimatorController;
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");
        GameOver();
    }

    void Shoot()
    {
        audiosource.PlayOneShot(shootClip);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "portal")
        {
        Invoke("LevelChange", 2);
        }
    }
    */

    void LevelChange()
    {
        
        SceneManager.LoadScene("BetterMarioBrosS2Tilemap");
        
     
        
        //levelCounter = 2;
    } 

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}