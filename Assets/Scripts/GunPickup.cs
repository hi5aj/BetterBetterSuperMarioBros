using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Controller controller = collision.GetComponent<Player_Controller>();
        if (controller != null)
        {
            controller.PickupGun();
            Destroy(gameObject);
        }
    }
}
