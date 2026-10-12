
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindControlUI : MonoBehaviour {
    [Header("Wind Controller")]
    [SerializeField] private DynamicWindController wind;

    [Header("Sliders")]
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider strengthSlider;
    [SerializeField] private Slider directionSlider;

    [Header("Value Labels")]
    [SerializeField] private TMP_Text speedValue;
    [SerializeField] private TMP_Text strengthValue;
    [SerializeField] private TMP_Text directionValue;

    private void Start() {
        if (wind == null)
            wind = DynamicWindController.Instance;

        if (wind == null) {
            Debug.LogError("WindControlUI: DynamicWindController not found.");
            enabled = false;
            return;
        }

        speedSlider.minValue = 0f;
        speedSlider.maxValue = 20f;

        strengthSlider.minValue = 0f;
        strengthSlider.maxValue = 2f;

        directionSlider.minValue = 0f;
        directionSlider.maxValue = 360f;

        speedSlider.SetValueWithoutNotify(wind.windSpeed);
        strengthSlider.SetValueWithoutNotify(wind.windStrength);
        directionSlider.SetValueWithoutNotify(wind.windDirection);

        speedSlider.onValueChanged.AddListener(OnSpeedChanged);
        strengthSlider.onValueChanged.AddListener(OnStrengthChanged);
        directionSlider.onValueChanged.AddListener(OnDirectionChanged);

        RefreshLabels();
    }

    private void OnDestroy() {
        if (speedSlider != null)
            speedSlider.onValueChanged.RemoveListener(OnSpeedChanged);

        if (strengthSlider != null)
            strengthSlider.onValueChanged.RemoveListener(OnStrengthChanged);

        if (directionSlider != null)
            directionSlider.onValueChanged.RemoveListener(OnDirectionChanged);
    }

    private void OnSpeedChanged(float value) {
        wind.SetWindSpeed(value);
        RefreshLabels();
    }

    private void OnStrengthChanged(float value) {
        wind.SetWindStrength(value);
        RefreshLabels();
    }

    private void OnDirectionChanged(float value) {
        wind.SetWindDirection(value);
        RefreshLabels();
    }

    private void RefreshLabels() {
        speedValue.text = $"Speed: {speedSlider.value:0.0}";
        strengthValue.text = $"Strength: {strengthSlider.value:0.00}";
        directionValue.text = $"Direction: {directionSlider.value:0}°";
    }

    public void ResetWind() {
        speedSlider.SetValueWithoutNotify(5f);
        strengthSlider.SetValueWithoutNotify(0.5f);
        directionSlider.SetValueWithoutNotify(0f);

        wind.SetWindSpeed(5f);
        wind.SetWindStrength(0.5f);
        wind.SetWindDirection(0f);

        RefreshLabels();
    }

    public void CalmWind() {
        strengthSlider.SetValueWithoutNotify(0f);
        wind.SetWindStrength(0f);
        RefreshLabels();
    }
}
