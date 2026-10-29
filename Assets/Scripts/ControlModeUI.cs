using UnityEngine;

public class ControlModeUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private GyroScope gyroScope;
    [SerializeField] private GameObject joystickRoot;
    [SerializeField] private UnityEngine.UI.Button modeButton;
    [SerializeField] private UnityEngine.UI.Button calibrateButton;
    [SerializeField] private UnityEngine.UI.Text modeLabel;
    [SerializeField] private UnityEngine.UI.Text statusLabel;

    private void OnEnable()
    {
        modeButton.onClick.AddListener(ToggleMode);
        calibrateButton.onClick.AddListener(gyroScope.Calibrate);
    }

    private void OnDisable()
    {
        modeButton.onClick.RemoveListener(ToggleMode);
        calibrateButton.onClick.RemoveListener(gyroScope.Calibrate);
    }

    private void ToggleMode()
    {
        movement.SetGyroscopeMode(!movement.UseGyroscope);
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        bool gyroMode = movement.UseGyroscope;
        if (joystickRoot.activeSelf == gyroMode)
            joystickRoot.SetActive(!gyroMode);

        modeLabel.text = gyroMode ? "Управление: гироскоп" : "Клавиатура / джойстик";
        calibrateButton.interactable = gyroMode && gyroScope.Calibrated;

        if (!gyroMode)
            statusLabel.text = "Двигай шар клавишами или экранным джойстиком";
        else if (!gyroScope.SensorAvailable)
            statusLabel.text = "Гироскоп не обнаружен. Подключи телефон";
        else if (!gyroScope.Calibrated)
            statusLabel.text = "Ожидание данных датчика…";
        else
            statusLabel.text = "Наклоняй телефон. Калибровка задаёт нейтральное положение";

        statusLabel.color = gyroMode && !gyroScope.SensorAvailable
            ? new Color(1f, 0.8f, 0.35f)
            : new Color(0.85f, 0.94f, 1f);
    }
}
