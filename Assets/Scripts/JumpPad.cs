//Script for Jump Pads
//Attached to JumpPad under Level Design -> PlatformGap and 
//Level Design -> Tower in Level 1 Scene
//Attached to Level Design --> Platform 6 in Level 2 Scene
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Launch Force")]
    public float launchForce = 15f;

    private void OnTriggerEnter(Collider obj)
    {
        //get rigidbody of whatever fell into trigger 
        Rigidbody rb = obj.attachedRigidbody;

        //keep body's current X and Z speed, then overwrite Y velocity with launch force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, launchForce, rb.linearVelocity.z);
    }
}