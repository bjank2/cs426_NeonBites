using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public WheelVehicle bikeSc;
    public TextMeshProUGUI speedtxt;
    public GameObject needleImage;
    public float updateInterval = 0.25f; // Time in seconds between speed updates
    private float lastUpdate;

    private float displayedSpeed = 0f;
    public float smoothSpeedFactor = 0.1f; // Factor for smoothing the speed (lower is smoother)

    // Needle rotation settings
    private const float MinAngle = 135f;
    private const float MaxAngle = -135f;
    private const float MidAngle = 0f;

    void Update()
    {
        // Only update the UI at a fixed interval
        if (Time.time - lastUpdate >= updateInterval)
        {
            lastUpdate = Time.time;
            UpdateSpeedometer();
        }
    }

    void UpdateSpeedometer()
    {
        // Smooth the speed value for display
        float targetSpeed = bikeSc.Speed;

        //displayedSpeed = Mathf.Lerp(displayedSpeed, targetSpeed, smoothSpeedFactor);
        int speedInt = Mathf.FloorToInt(targetSpeed / 1.9f);
        if(speedInt < 0) { speedInt = 0; }

        // Update the speed text with the smoothed integer value
        speedtxt.text = speedInt.ToString(); // Add "km/h" or your desired unit

        // Rotate the needle based on the current speed
        float needleAngle = SpeedToNeedleAngle(speedInt);
        needleImage.transform.localEulerAngles = new Vector3(0, 0, needleAngle);
    }

    // Map the speed value to the needle's angle
    float SpeedToNeedleAngle(float speed)
    {
        // Normalize the speed value between 0 and 1
        float normalizedSpeed = speed / 100;

        // Lerp between the min and max angles based on normalized speed
        // This assumes a linear scale, but you could apply different scaling if needed
        if (normalizedSpeed <= 0.5f) // Speeds from 0 to 50 mph
        {
            // Map from 0 to 0.5 normalized speed to 135 to 0 degrees
            return Mathf.Lerp(MinAngle, MidAngle, normalizedSpeed * 2);
        }
        else // Speeds from 50 to 100 mph
        {
            // Map from 0.5 to 1 normalized speed to 0 to -135 degrees
            return Mathf.Lerp(MidAngle, MaxAngle, (normalizedSpeed - 0.5f) * 2);
        }
    }
}
