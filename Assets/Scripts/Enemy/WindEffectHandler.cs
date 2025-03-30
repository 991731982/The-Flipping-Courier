using System.Collections;
using UnityEngine;

public class WindEffectHandler : MonoBehaviour
{
    private Rigidbody rb;
    private WindFan windFan;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        windFan = FindObjectOfType<WindFan>();
    }

    void FixedUpdate()
    {
        if (windFan != null && rb != null)
        {
            Vector3 windDirection = windFan.transform.right;
            Vector3 windOrigin = windFan.transform.position + windDirection * (windFan.boxSize.x / 2);
            Collider[] colliders = Physics.OverlapBox(windOrigin, windFan.boxSize / 2, windFan.transform.rotation);

            foreach (Collider col in colliders)
            {
                if (col.gameObject == gameObject)
                {
                    float windMultiplier = rb.mass < 60f ? 2f : 1f;
                    rb.AddForce(windDirection * windFan.windForce * windMultiplier, ForceMode.Acceleration);
                }
            }
        }
    }
}