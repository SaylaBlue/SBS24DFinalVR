using System.Collections; // Required for Coroutines
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class TVRemoteController : MonoBehaviour
{
    [Tooltip("The VideoPlayer component on the TV_Player_Controller object.")]
    public VideoPlayer videoPlayer;

    // --- NEW: Objective Control Variables ---
    [Header("Objective Settings")]
    [Tooltip("The objective text to display 2 seconds after the remote is clicked.")]
    public string newObjectiveText = "Find the next room key now that the TV is on.";

    [Tooltip("The delay in seconds before the objective text updates.")]
    public float objectiveDelay = 2.0f;
    // ----------------------------------------

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private bool isPlaying = false;

    void Start()
    {
        if (TryGetComponent(out interactable))
        {
            // Subscribe to the Select Entered event for the single click toggle
            interactable.selectEntered.AddListener(OnRemoteClicked);
        }
        else
        {
            Debug.LogError("XRSimpleInteractable component not found on the Remote.");
        }

        if (videoPlayer == null)
        {
            Debug.LogError("Video Player reference is missing! Please assign it in the Inspector.");
        }
    }

    // --- Interaction Toggle Function (Called instantly on click) ---

    private void OnRemoteClicked(SelectEnterEventArgs args)
    {
        if (videoPlayer == null) return;

        // Toggle the state and control the video
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            videoPlayer.Play();
            Debug.Log("TV Toggled ON.");

            // Start the delayed objective update Coroutine
            StartCoroutine(UpdateObjectiveAfterDelay(newObjectiveText));

        }
        else
        {
            videoPlayer.Stop();
            Debug.Log("TV Toggled OFF.");

            // OPTIONAL: Reset the objective to a default/blank state after a delay
            // StartCoroutine(UpdateObjectiveAfterDelay("Wait for the next goal...", 1.0f));
        }
    }

    // --- NEW: Coroutine for Delayed Execution ---

    IEnumerator UpdateObjectiveAfterDelay(string textToDisplay)
    {
        // 1. PAUSE: Wait for the specified duration (2.0 seconds by default)
        yield return new WaitForSeconds(objectiveDelay);

        // 2. EXECUTE: This code runs only AFTER the delay has passed.
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.UpdateObjective(textToDisplay);
        }
        else
        {
            Debug.LogError("ObjectiveManager not found! Cannot update objective.");
        }

        // OPTIONAL: Prevent rapid re-triggering of the objective if player spams the remote
        // interactable.enabled = true; 
    }
}