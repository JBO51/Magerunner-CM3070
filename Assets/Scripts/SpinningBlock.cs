//Script for spinning blocks
//Used in Level 1
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
        float radiansPerSecond = spinSpeed * Mathf.Deg2Rad;
        rb.angularVelocity = new Vector3(0f, radiansPerSecond, 0f);
    }
}