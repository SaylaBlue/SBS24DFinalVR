using UnityEngine;

public class FanButton : MonoBehaviour
{
    // A reference to the fan's script.
    // We will link this in the Unity Inspector.
    [Tooltip("Drag the fan GameObject here.")]
    public FanSpinnerVR fanScript;

    // This function is called automatically by Unity when another
    // collider enters this object's trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that touched the button has the "PlayerHand" tag.
        // This prevents other objects from accidentally triggering it.
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log("Hand touched the button!");

            // If we have a valid reference to the fan script, call its ToggleFan function.
            if (fanScript != null)
            {
                fanScript.ToggleFan();
            }
            else
            {
                Debug.LogWarning("Fan Script not assigned on the button!");
            }
        }
    }
}