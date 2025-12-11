using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(XRSimpleInteractable))]
public class XRComputerTransition : MonoBehaviour
{
    [Tooltip("The Empty GameObject defining the player's position when using the computer.")]
    public Transform targetViewpoint;

    [Tooltip("The root XR Origin GameObject (the object you want to move).")]
    public GameObject xrOriginRoot;

    // --- References to Locomotion Components (ASSIGN THESE IN THE INSPECTOR) ---
    [Header("Locomotion Components to Disable")]
    public ActionBasedContinuousMoveProvider continuousMoveProvider;
    public ActionBasedContinuousTurnProvider continuousTurnProvider;
    // ---------------------------------------------------------------------

    private XRBaseInteractable interactable;
    private bool isUsingComputer = false;

    // Camera Offset Variables
    private Transform mainCameraTransform;
    private Vector3 positionOffset;

    void Start()
    {
        // Safety check to ensure required components are available
        if (!TryGetComponent(out interactable))
        {
            Debug.LogError("XRComputerTransition requires an XRBaseInteractable component on this GameObject.");
            return;
        }

        interactable.selectEntered.AddListener(OnComputerSelected);

        // Find the XR Origin and Camera references
        if (xrOriginRoot == null)
        {
            xrOriginRoot = GameObject.FindGameObjectWithTag("XROrigin");
            if (xrOriginRoot == null)
            {
                Debug.LogError("XR Origin Root not found. Please assign it in the Inspector.");
            }
        }

        mainCameraTransform = Camera.main.transform;
    }

    // --- Interaction Event Handler ---

    private void OnComputerSelected(SelectEnterEventArgs args)
    {
        ToggleComputerUse();
    }

    // --- Update Loop (Only for Exit Check) ---

    void Update()
    {
        // 1. **Movement Logic** (REMOVED: Movement is instant in ToggleComputerUse)

        // 2. **Exit Logic** (Using the Escape key)
        if (isUsingComputer && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleComputerUse();
        }
    }

    // --- Main Toggle Function ---

    public void ToggleComputerUse()
    {
        isUsingComputer = !isUsingComputer;

        if (isUsingComputer)
        {
            // --- Entering the Computer View ---

            // 1. Calculate the offset: Vector from the XR Origin's pivot to the Camera's global position.
            positionOffset = mainCameraTransform.position - xrOriginRoot.transform.position;

            // 2. INSTANTLY SNAP the position and rotation
            if (xrOriginRoot != null)
            {
                // Target position for the XR Origin root is the Viewpoint minus the height/offset
                Vector3 adjustedTargetPosition = targetViewpoint.position - positionOffset;

                xrOriginRoot.transform.position = adjustedTargetPosition;
                xrOriginRoot.transform.rotation = targetViewpoint.rotation;
            }

            // 3. DISABLE LOCOMOTION
            if (continuousMoveProvider != null)
            {
                continuousMoveProvider.enabled = false;
            }
            if (continuousTurnProvider != null)
            {
                continuousTurnProvider.enabled = false;
            }
        }
        else
        {
            // --- Exiting the Computer View ---

            // ENABLE LOCOMOTION
            if (continuousMoveProvider != null)
            {
                continuousMoveProvider.enabled = true;
            }
            if (continuousTurnProvider != null)
            {
                continuousTurnProvider.enabled = true;
            }
        }

        if (xrOriginRoot != null && interactable != null)
        {
            // Temporarily disable/enable the computer's interactable component
            interactable.enabled = !isUsingComputer;
        }

        Debug.Log("Computer Use Toggled: " + isUsingComputer);
    }
}