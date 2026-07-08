//Script for spinning blocks
//Attached to Spinning Platform under Bridge, Level Design in Level 1 scene
using UnityEngine;

public class SpinningBlock : MonoBehaviour
{
    [Header("Spin Speed (Degrees per Second)")]
    public float spinSpeed = 200f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.isKinematic == false)
        {
            float radiansPerSecond = spinSpeed * Mathf.Deg2Rad;
            rb.angularVelocity = new Vector3(0f, radiansPerSecond, 0f);
        }
    }
}