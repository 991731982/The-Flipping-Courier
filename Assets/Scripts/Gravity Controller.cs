using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private Rigidbody rb;
    public bool gravityFlipped = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CanFlipGravity())
            {
                FlipGravity();
                Debug.Log("Gravity flipped successfully.");
            }

        }
    }

    public void FlipGravity()
    {
        gravityFlipped = !gravityFlipped;
        // Physics.gravity = gravityFlipped ? new Vector3(0, 9.81f, 0) : new Vector3(0, -9.81f, 0);
        if(gravityFlipped == false) { 
        Physics.gravity = new Vector3(0, -20.0F, 0);
        }
        else { Physics.gravity = new Vector3(0, 20.0F, 0); }

    }

    bool CanFlipGravity()
    {
        // Allow flipping gravity only if the player is grounded or roofed (stationary in vertical axis)
        bool canFlip = Mathf.Abs(rb.velocity.y) < 0.1f;
        Debug.Log("CanFlipGravity() = " + canFlip); // Debugging output to see the condition result
        return canFlip;
    }
}
