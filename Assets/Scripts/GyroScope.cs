using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class GyroScope : MonoBehaviour
{
    [SerializeField] private bool sensorAvailable;
    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private Vector2 direction;
    [SerializeField, Range(1f, 60f)] private float maxTilt = 25f;
    [SerializeField, Range(0f, 0.5f)] private float deadZone = 0.1f;

    public Vector2 Direction => direction;
    public bool SensorAvailable => sensorAvailable;
    public bool Calibrated => calibrated;

    private Gyroscope gyro;
    private AttitudeSensor attitude;
    private Quaternion neutralRotation;
    private bool calibrated;
    private bool gyroEnabledHere;
    private bool attitudeEnabledHere;
    private ScreenOrientation lastOrientation;

    private void OnEnable()
    {
        lastOrientation = Screen.orientation;
        ConnectSensors();
    }

    private void ConnectSensors()
    {
        gyro = Gyroscope.current;
        attitude = AttitudeSensor.current;
        calibrated = false;
        direction = Vector2.zero;
        angularVelocity = Vector3.zero;
        gyroEnabledHere = false;
        attitudeEnabledHere = false;
        sensorAvailable = gyro != null && attitude != null;

        if (!sensorAvailable)
            return;

        gyroEnabledHere = !gyro.enabled;
        attitudeEnabledHere = !attitude.enabled;
        if (gyroEnabledHere)
            InputSystem.EnableDevice(gyro);
        if (attitudeEnabledHere)
            InputSystem.EnableDevice(attitude);
    }

    private void Update()
    {
        // Remote can connect or disconnect while Play Mode is running.
        if (gyro == null || !gyro.added || attitude == null || !attitude.added)
        {
            ReleaseSensors();
            ConnectSensors();
        }

        sensorAvailable =
            gyro != null && gyro.added && gyro.enabled &&
            attitude != null && attitude.added && attitude.enabled;

        if (!sensorAvailable)
        {
            direction = Vector2.zero;
            angularVelocity = Vector3.zero;
            calibrated = false;
            return;
        }

        if (lastOrientation != Screen.orientation)
        {
            lastOrientation = Screen.orientation;
            calibrated = false;
        }

        angularVelocity = gyro.angularVelocity.ReadValue();
        if (attitude.lastUpdateTime <= 0)
            return;

        if (!calibrated)
            Calibrate();

        Quaternion relative = Quaternion.Inverse(neutralRotation) * ReadRotation();
        Vector3 normal = relative * Vector3.forward;
        float fullTilt = Mathf.Sin(Mathf.Clamp(maxTilt, 1f, 60f) * Mathf.Deg2Rad);
        Vector2 input = Vector2.ClampMagnitude(
            new Vector2(normal.x, normal.y) / fullTilt, 1f);
        direction = input.magnitude < deadZone ? Vector2.zero : input;
    }

    [ContextMenu("Calibrate")]
    public void Calibrate()
    {
        if (!sensorAvailable || attitude == null || !attitude.added ||
            attitude.lastUpdateTime <= 0)
            return;

        neutralRotation = ReadRotation();
        calibrated = true;
        direction = Vector2.zero;
    }

    private Quaternion ReadRotation()
    {
        Quaternion value = attitude.attitude.ReadValue();
        return new Quaternion(value.x, value.y, -value.z, -value.w);
    }

    private void OnDisable()
    {
        ReleaseSensors();
    }

    private void ReleaseSensors()
    {
        if (gyroEnabledHere && gyro != null && gyro.added && gyro.enabled)
            InputSystem.DisableDevice(gyro);
        if (attitudeEnabledHere && attitude != null && attitude.added && attitude.enabled)
            InputSystem.DisableDevice(attitude);

        gyro = null;
        attitude = null;
        gyroEnabledHere = false;
        attitudeEnabledHere = false;
        sensorAvailable = false;
        direction = Vector2.zero;
        calibrated = false;
    }
}
