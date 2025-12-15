
// Unused script

using UnityEngine;
using System.Collections;
using Oculus.Interaction;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OVRGrabbable))]
public class OculusCubePhysics : MonoBehaviour
{
    private Rigidbody rb;
    private OVRGrabbable grabbable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<OVRGrabbable>();

        if (grabbable == null) Debug.LogError("nope");

        // Make sure cube starts with physics
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        // When grabbed, disable physics so it follows the hand
        if (grabbable.isGrabbed)
        {
            rb.isKinematic = true;
        }
        else
        {
            // When released, physics takes over
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public void select() {
        rb.isKinematic = true;
    }

    public void unselect()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
