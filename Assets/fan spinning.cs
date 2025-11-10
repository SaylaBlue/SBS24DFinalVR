using UnityEngine;

public class FanSpinnerVR : MonoBehaviour
{
    [Tooltip("The speed of rotation in degrees per second.")]
    public float rotationSpeed = 200f;

    [Tooltip("The axis of rotation (e.g., Vector3.up for the Y-axis).")]
    public Vector3 rotationAxis = Vector3.up;

    // A boolean to track whether the fan should be spinning.
    private bool isSpinning = false;

    void Update()
    {
        // Only rotate the fan if isSpinning is true.
        if (isSpinning)
        {
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
        }
    }

    // This is a public function that other scripts can call.
    // It will toggle the fan's state.
    public void ToggleFan()
    {
        isSpinning = !isSpinning;
    }
}