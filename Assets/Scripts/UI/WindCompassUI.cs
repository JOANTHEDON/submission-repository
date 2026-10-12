
using TMPro;
using UnityEngine;

public class WindCompassUI : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RectTransform compassArrow;
    [SerializeField] private TMP_Text windStatusText;
    [SerializeField] private TMP_Text effectiveWindText;

    [Header("Display")]
    [SerializeField] private float directionOffset = 0f;

    private void Update() {
        DynamicWindController wind = DynamicWindController.Instance;
        if (wind == null) return;

        // 0 degrees = +Z, 90 = +X.
        // UI arrow rotation is clockwise from its default upward direction.
        if (compassArrow != null) {
            compassArrow.localEulerAngles = new Vector3(
                0f, 0f, -(wind.windDirection + directionOffset)
            );
        }

        float effective = wind.EffectiveStrength;
        float speed = wind.windSpeed * effective;

        if (windStatusText != null) {
            if (speed < 0.1f)
                windStatusText.text = "Wind Status: Calm";
            else if (speed < 3f)
                windStatusText.text = "Wind Status: Light";
            else if (speed < 7f)
                windStatusText.text = "Wind Status: Moderate";
            else
                windStatusText.text = "Wind Status: Strong";
        }

        if (effectiveWindText != null) {
            effectiveWindText.text =
                $"Effective Wind: {speed:0.0} m/s";
        }
    }
}
