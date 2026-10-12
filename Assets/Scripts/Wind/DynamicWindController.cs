
using UnityEngine;

public class DynamicWindController : MonoBehaviour {
    public static DynamicWindController Instance { get; private set; }

    [Header("Runtime Wind Settings")]
    [Range(0f, 20f)]
    public float windSpeed = 5f;

    [Range(0f, 2f)]
    public float windStrength = 0.5f;

    [Range(0f, 360f)]
    public float windDirection = 0f;

    [Header("Natural Variation")]
    [Range(0f, 1f)]
    public float gustAmount = 0.25f;

    [Min(0.01f)]
    public float variationFrequency = 0.3f;

    [Range(0f, 45f)]
    public float directionVariation = 12f;

    public Vector3 WindVector { get; private set; }
    public float EffectiveStrength { get; private set; }

    private float noiseOffset;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        noiseOffset = Random.Range(0f, 1000f);
    }

    private void Update() {
        float t = Time.time * variationFrequency + noiseOffset;

        float gust = 1f + Mathf.Sin(t) * gustAmount;

        float noise = Mathf.PerlinNoise(t, 0.37f);
        float strengthVariation = Mathf.Lerp(0.85f, 1.15f, noise);

        float angle = windDirection +
            (Mathf.PerlinNoise(t * 0.35f, 0.73f) - 0.5f)
            * 2f * directionVariation;

        float radians = angle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(
            Mathf.Sin(radians),
            0f,
            Mathf.Cos(radians)
        );

        EffectiveStrength = Mathf.Max(
            0f, windStrength * gust * strengthVariation
        );

        WindVector = direction * windSpeed * EffectiveStrength;
    }

    public void SetWindSpeed(float value) {
        windSpeed = Mathf.Clamp(value, 0f, 20f);
    }

    public void SetWindStrength(float value) {
        windStrength = Mathf.Clamp(value, 0f, 2f);
    }

    public void SetWindDirection(float value) {
        windDirection = Mathf.Repeat(value, 360f);
    }
}
