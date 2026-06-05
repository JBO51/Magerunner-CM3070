//Script for Jump Pads
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Launch Force")]
    public float launchForce = 15f;

    private void OnTriggerEnter(Collider obj)
    {
        //get rigidbody of whatever fell into trigger 
        Rigidbody rb = obj.attachedRigidbody;

        //keep player's current X and Z speed, then overwrite Y velocity with launch force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, launchForce, rb.linearVelocity.z);
    }
}