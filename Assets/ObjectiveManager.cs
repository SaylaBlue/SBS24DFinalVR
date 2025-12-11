using UnityEngine;
using TMPro; // Necessary for TextMeshPro UI

public class ObjectiveManager : MonoBehaviour
{
    // 1. Singleton Instance (Allows easy access from other scripts)
    public static ObjectiveManager Instance;

    [Tooltip("Drag the Objective_Text UI object here in the Inspector.")]
    public TextMeshProUGUI objectiveText;

    private void Awake()
    {
        // Set up the Singleton
        if (Instance == null)
        {
            Instance = this;
            // OPTIONAL: Keep the manager across scene loads
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- Public Function to Update the Objective ---

    public void UpdateObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "Current Objective: " + newObjective;
            Debug.Log("Objective Updated to: " + newObjective);
        }
        else
        {
            Debug.LogError("Objective Text reference is missing! Cannot update objective.");
        }
    }

    // --- Example Function for Demonstration ---

    public void StartInitialObjective()
    {
        UpdateObjective("Go Home");
    }

    void Start()
    {
        StartInitialObjective();
    }
}