using UnityEngine;

/// <summary>
/// This script teleports a specified GameObject to a target location upon collision.
/// Attach this script to the object that will act as the teleporter trigger.
/// </summary>
public class Teleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("The GameObject to be teleported (e.g., the Player).")]
    public GameObject objectToTeleport;

    [Tooltip("The Transform of the destination object. The player will be moved to this position.")]
    public Transform teleportTarget;

    /// <summary>
    /// This method is called when this collider/rigidbody has begun touching another rigidbody/collider.
    /// </summary>
    /// <param name="collision">The collision data associated with this collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with is the one we intend to teleport.
        // It's often better to check by tag, e.g., if(collision.gameObject.CompareTag("Player"))
        if (collision.gameObject == objectToTeleport)
        {
            // Ensure that a destination has been assigned in the Inspector.
            if (teleportTarget != null)
            {
                // Move the object to the target's position.
                // Note: This directly sets the position. For CharacterControllers, you might
                // need to disable the controller temporarily before and after the move.
                objectToTeleport.transform.position = teleportTarget.position;
            }
            else
            {
                // Log a warning if the target is not set, to help with debugging.
                Debug.LogWarning("Teleport target has not been assigned.", this);
            }
        }
    }
}
