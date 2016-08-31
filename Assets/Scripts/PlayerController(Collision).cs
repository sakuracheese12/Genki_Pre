using UnityEngine;
using System.Collections;

public partial class PlayerController : MonoBehaviour
{

    void OnCollisionStay(Collision collisionInfo)
    {
        ContactPoint contact = collisionInfo.contacts[0];
        Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
        Debug.Log("s"+contact.normal);
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        ContactPoint contact = collisionInfo.contacts[0];
        Debug.DrawRay(contact.point, contact.normal * 10, Color.blue);
        Debug.Log("e"+contact.normal);
    }

}
