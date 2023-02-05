using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadCheck : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    public AudioSource audiosource;
    public AudioClip stompClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyStompCheck>())
        {
            audiosource.PlayOneShot(stompClip);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * 300f);
        }
    }
}
