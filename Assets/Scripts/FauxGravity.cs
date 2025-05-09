using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravity : MonoBehaviour
{
    public float Gravity = -10;

    public void Attract(Transform Body)
    {
        Vector3 GravityUp = (Body.position - transform.position).normalized;
        Vector3 BodyUp = Body.up;
        Rigidbody RigidBody = Body.GetComponent<Rigidbody>();

        RigidBody.AddForce(GravityUp * Gravity);

        Quaternion TargetRotation = Quaternion.FromToRotation(BodyUp, GravityUp) * Body.rotation;
        Body.rotation = Quaternion.Slerp(Body.rotation, TargetRotation, 50 * Time.deltaTime);
    }
}
