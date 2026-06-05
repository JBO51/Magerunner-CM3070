//Injector script for changing objects physics behaviour with stasis spell

using UnityEngine;
using System.Collections;

public class StasisHandler : MonoBehaviour
{
    //Store the current rigidbody, the original constraints
    //and the accumulated force + duration of stasis
    private Rigidbody rb;
    private RigidbodyConstraints originalConstraints;
    private Vector3 accumulatedForces = Vector3.zero;
    private float stasisDuration = 3f;

    //Awake for when script is injected into object
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            //store original constraints to return it later
            originalConstraints = rb.constraints;

            //Freeze object velocity so it stops 
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //Freeze constraints so it freezes completely
            rb.constraints = RigidbodyConstraints.FreezeAll;

            //StartCoroutine function to wait 3 seconds before
            //applying accumulated forces and unfreezing
            StartCoroutine(StasisTimer());
        }
    }

    //Function to add force from spells while objecty is frozen
    public void AddStoredForce(Vector3 force)
    {
        accumulatedForces += force;
    }

    private IEnumerator StasisTimer()
    {
        //wait 3 seconds
        yield return new WaitForSeconds(stasisDuration);

        if (rb != null)
        {
            //Restore original constraints
            rb.constraints = originalConstraints;

            //wake up the physics body so it registers the physics update
            rb.WakeUp();

            //move object up a tiny bit in case of ground physics bugs 
            transform.position += Vector3.up * 0.01f;

            //add all force vectors that were added during stasis all at once
            rb.AddForce(accumulatedForces, ForceMode.Impulse);
        }

        //Remove StasisObject script from the object so it goes back to normal
        Destroy(this);
    }
}