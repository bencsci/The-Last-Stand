using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoDrop : MonoBehaviour
{
    // Reference to the ammo box GameObject
    private GameObject ammoBox;

    // Speed for floating movement
    private float floatSpeed = 1.5f;

    // Speed for rotation
    private float rotationSpeed = 30f;


    void Start()
    {
        ammoBox = gameObject; 
        Destroy(ammoBox, 25f);
    }

    void Update()
    {
        // Make Game Object float up and down
        float floting = Mathf.Sin(Time.time * floatSpeed) * 0.25f + 1f;
        transform.position = new Vector3(transform.position.x, floting, transform.position.z);
        
        // Rotate 360 slowly
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AmmoManager.addAmmo(Player.currentWeapon, 1);
            Destroy(gameObject);
        }  
    }
}
