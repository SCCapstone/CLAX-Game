using UnityEngine;
/**
 * Controls the moving platform behavior
 */
public class MovingPlatform : MonoBehaviour
{
    public GameObject startStage;
    public GameObject endStage;
    public GameObject stage;

    public InterpolationMode interpolationMode;

    public enum InterpolationMode
    {
        Linear,
        Cubic
    }

    public float travelDuration;
    public float stayDuration;

    private float counter = 0.0f;
    private bool reverse = false;
    /**
     * Moves the platform 
     */
    void FixedUpdate()
    {
        if (travelDuration <= 0.0f)
        {
            return;
        }

        if (counter > travelDuration + stayDuration)
        {
            counter = travelDuration + stayDuration;

            reverse = true;
        }
        else if (counter < -stayDuration)
        {
            counter = -stayDuration;

            reverse = false;
        }

        // Move platform before updating velocities

        float alpha = Mathf.Clamp(counter / travelDuration, 0.0f, 1.0f);

        switch (interpolationMode)
        {
            case (InterpolationMode.Cubic):
                alpha = CubicInterpolate(0.0f, 1.0f, alpha);

                break;
            default:
                break;
        }

        stage.transform.position = Vector3.Lerp(startStage.transform.position, endStage.transform.position, alpha);
        stage.transform.rotation = Quaternion.Lerp(startStage.transform.rotation, endStage.transform.rotation, alpha);

        counter += reverse ? -Time.fixedDeltaTime : Time.fixedDeltaTime;
    }

    public float CubicInterpolate(float a, float b, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);

        return
            (2.0f * (a - b) * Mathf.Pow(t, 3.0f))
            + (3.0f * (b - a) * Mathf.Pow(t, 2.0f))
            + a;
    }
}
