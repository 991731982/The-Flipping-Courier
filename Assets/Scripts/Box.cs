using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int hitsToDestroy = 3;         // Hits required to destroy the box
    public float fallAmount = 0.1f;       // Amount the box moves down each hit
    public GameObject smallCubePrefab;    // Prefab for the small cube to spawn

    public int currentHits = 0;          // Track current hits
    private bool canRegisterHit = true;   // Control when hits can be registered

    void Start()
    {
        // Ensure the box has a Rigidbody and set it to kinematic (so it doesn't move, but still collides)
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  // This prevents the box from moving due to physics, but still allows collisions.
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding is tagged as "Weight" and if hits can be registered
        if (collision.gameObject.CompareTag("Weight") && canRegisterHit)
        {
            // Get the contact point and its normal
            ContactPoint contact = collision.contacts[0];
            print(Vector3.Dot(contact.normal, transform.up));

            // Check if the collision is from the top
            if (collision.relativeVelocity.y < 0 && Vector3.Dot(contact.normal, transform.up) > 0.9f)
            {
                // Register a hit and start the cooldown
                currentHits++;
                Debug.Log("Weight hit the box from the top! Current hits: " + currentHits);

                // Move the box down by the fall amount
                transform.position -= new Vector3(0, fallAmount, 0);

                // Check if the box should spawn the small cube
                if (currentHits >= hitsToDestroy)
                {
                    Vector3 spawnPosition = transform.position + new Vector3(0, transform.localScale.y / 2, 0);
                    Instantiate(smallCubePrefab, spawnPosition, Quaternion.identity);
                    Debug.Log("Key spawned on top of the box!");
                }

                // Start cooldown
                StartCoroutine(HitCooldown());
            }
            else
            {
                Debug.Log("Weight hit the side or at a low angle, not counted.");
            }
        }
    }

    // Coroutine to handle the cooldown period between hits
    private IEnumerator HitCooldown()
    {
        canRegisterHit = false;
        yield return new WaitForSeconds(2f);  // Wait for 2 seconds
        canRegisterHit = true;
    }
}
