using UnityEngine;
using UnityEngine.InputSystem; // Necessary for checking keyboard/mouse input

public class ComputerInteraction : MonoBehaviour
{
    [Tooltip("The Empty GameObject defining the player's position when using the computer.")]
    public Transform targetViewpoint;

    [Tooltip("The main Player/VR Camera Rig (the object you want to move).")]
    public GameObject playerCameraRig;

    // Optional: How fast the player should move to the computer
    public float transitionSpeed = 5f;

    // Tracks if the player is currently using the computer
    private bool isUsingComputer = false;

    void Start()
    {
        // Find the player rig if it wasn't set in the Inspector
        if (playerCameraRig == null)
        {
            // You might need a more specific tag or search depending on your rig setup (e.g., "Main Camera" tag or "XR Rig")
            playerCameraRig = Camera.main.transform.parent.gameObject;
        }
    }

    void Update()
    {
        // This is the **Desktop/Mouse Click** part of the logic
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            // Raycast from the camera to see what was clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10f))
            {
                // Check if the clicked object is THIS computer
                if (hit.collider.gameObject == gameObject)
                {
                    ToggleComputerUse();
                }
            }
        }

        // If currently using the computer, smoothly move the player towards the viewpoint
        if (isUsingComputer && playerCameraRig != null)
        {
            // Smoothly move the position
            playerCameraRig.transform.position = Vector3.Lerp(
                playerCameraRig.transform.position,
                targetViewpoint.position,
                Time.deltaTime * transitionSpeed
            );

            // Smoothly move the rotation
            playerCameraRig.transform.rotation = Quaternion.Lerp(
                playerCameraRig.transform.rotation,
                targetViewpoint.rotation,
                Time.deltaTime * transitionSpeed
            );
        }

        // **Exit Logic:** Allow the player to exit by pressing a key (e.g., 'E' or 'Escape')
        if (isUsingComputer && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleComputerUse();
        }
    }

    // Main function to switch modes
    public void ToggleComputerUse()
    {
        isUsingComputer = !isUsingComputer;

        // **Optional:** Disable Player Movement and Look Scripts when using the computer
        // This prevents the player from moving away from the screen while typing.

        // Example: Disable a script called PlayerController on the rig
        // playerCameraRig.GetComponent<PlayerController>().enabled = !isUsingComputer;

        Debug.Log("Computer Use Toggled: " + isUsingComputer);
    }
}