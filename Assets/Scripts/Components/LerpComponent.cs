using System.Collections;
using UnityEngine;

public class LerpComponent : MonoBehaviour
{
    // Time it takes to executive moving animation.
    public float lerpPositionDuration = 1f;
    // Time it takes to execute rotation animation.
    public float lerpRotationDuration = 1.5f;

    public IEnumerator LerpPosition(Vector2 startValue, Vector2 endValue)
    {
        float time = 0;

        while (time < lerpPositionDuration)
        {
            transform.position = Vector2.Lerp(startValue, endValue, time / lerpPositionDuration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator LerpRotation(float angle)
    {
        // Initialize the time variables
        float currentTime = 0f;

        // Figure out the current angle/axis
        Quaternion sourceOrientation = transform.rotation;
        float sourceAngle;
        sourceOrientation.ToAngleAxis(out sourceAngle, out _);

        // Calculate a new target orientation
        float targetAngle = angle + sourceAngle;

        while (currentTime < lerpPositionDuration)
        {
            // Might as well wait first, especially on the first iteration where there'd be nothing to do otherwise.


            currentTime += Time.deltaTime;
            float progress = currentTime / lerpPositionDuration; // From 0 to 1 over the course of the transformation.

            // Interpolate to get the current angle/axis between the source and target.
            float currentAngle = Mathf.Lerp(sourceAngle, targetAngle, progress);

            // Assign the current rotation
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            yield return null;
        }
    }
}
