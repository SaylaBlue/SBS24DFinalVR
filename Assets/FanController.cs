using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class FanController : MonoBehaviour
{
    [Tooltip("The Transform component of the object you want to spin (e.g., the fan blades).")]
    public Transform rotatingObject;

    [Tooltip("The speed of the spin in degrees per second.")]
    public float spinSpeed = 500f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private bool isSpinning = false; // Tracks the current state of the fan

    void Start()
    {
        if (rotatingObject == null)
        {
            rotatingObject = transform;
            Debug.LogWarning("Rotating Object not set. Using the FanController's own Transform.");
        }

        if (TryGetComponent(out interactable))
        {
            // Subscribe to the Select Entered event for toggling
            interactable.selectEntered.AddListener(ToggleFanState);
        }
        else
        {
            Debug.LogError("XRSimpleInteractable component not found on the Fan object.");
        }
    }

    void Update()
    {
        if (isSpinning)
        {
            // *** CRUCIAL CHANGE: Rotates around Vector3.forward (the Z-axis) ***
            rotatingObject.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    // --- Interaction Toggle Function ---

    private void ToggleFanState(SelectEnterEventArgs args)
    {
        // 1. Flip the current state
        isSpinning = !isSpinning;

        // 2. Log the new state
        if (isSpinning)
        {
            Debug.Log("Fan Toggled ON! Starting Z-axis spin.");
        }
        else
        {
            Debug.Log("Fan Toggled OFF! Stopping Z-axis spin.");
        }
    }
}