using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public TeleportationProvider teleportProvider;
    public Transform destinationTransform;

    // --- NEW: Objective Change Variable ---
    [Header("Objective Update")]
    [Tooltip("The new objective text to display after teleporting.")]
    public string newObjectiveText = "Find the next challenge in this area!";
    // -------------------------------------

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Ensure all references are assigned
        if (grabInteractable == null || teleportProvider == null || destinationTransform == null)
        {
            Debug.LogError("Required components/references are missing on " + gameObject.name);
            enabled = false;
            return;
        }

        // Subscribe to the Select Entered event (fires when the object is successfully grabbed)
        grabInteractable.selectEntered.AddListener(OnGrabbedAndTeleport);

        // --- NEW: Initial Check for Objective Manager ---
        if (ObjectiveManager.Instance == null)
        {
            Debug.LogError("ObjectiveManager.Instance is not found. The objective will not update.");
        }
    }

    private void OnGrabbedAndTeleport(SelectEnterEventArgs args)
    {
        // 1. **Perform the Teleportation**
        TeleportRequest request = new TeleportRequest()
        {
            matchOrientation = MatchOrientation.TargetUpAndForward,
            destinationPosition = destinationTransform.position,
            destinationRotation = destinationTransform.rotation
        };
        teleportProvider.QueueTeleportRequest(request);

        // --- NEW: UPDATE THE OBJECTIVE ---
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.UpdateObjective(newObjectiveText);
        }
        // -------------------------------------

        // 2. **Force the Interactor to Drop the Object**
        grabInteractable.interactionManager.SelectExit(args.interactorObject, grabInteractable);

        // OPTIONAL: If this should only happen once, disable the interactable/object
        // grabInteractable.enabled = false;
    }

    void OnDestroy()
    {
        // Clean up the event listener
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbedAndTeleport);
        }
    }
}