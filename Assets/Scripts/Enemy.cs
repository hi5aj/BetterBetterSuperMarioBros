using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip enemyDeathClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        audiosource.PlayOneShot(enemyDeathClip);
        Destroy(gameObject);
    }
}
