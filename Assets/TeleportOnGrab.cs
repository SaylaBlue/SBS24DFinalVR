using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public TeleportationProvider teleportProvider;
    public Transform destinationTransform;

    void Start()
    {
        // Get the required component on this object
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Ensure all references are assigned (Error checking from before)
        if (grabInteractable == null || teleportProvider == null || destinationTransform == null)
        {
            Debug.LogError("Required components/references are missing on " + gameObject.name);
            enabled = false;
            return;
        }

        // Subscribe to the Select Entered event (fires when the object is successfully grabbed)
        grabInteractable.selectEntered.AddListener(OnGrabbedAndTeleport);
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

        // 2. **Force the Interactor to Drop the Object**
        // This tells the Interaction Manager to end the selection relationship
        // between the interactor (the hand) and the interactable (the object).
        grabInteractable.interactionManager.SelectExit(args.interactorObject, grabInteractable);

        // Note: You could alternatively disable the object here if you only want 
        // the teleport to happen once and the object to disappear.
        // gameObject.SetActive(false);
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