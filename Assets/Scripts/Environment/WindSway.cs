
using UnityEngine;

public class WindSway : MonoBehaviour {
    [Header("Sway Settings")]
    [SerializeField] private float swayAngle = 4f;
    [SerializeField] private float swayFrequency = 1.5f;
    [SerializeField] private float directionInfluence = 1f;

    private Quaternion initialRotation;
    private float randomOffset;

    private void Start() {
        initialRotation = transform.localRotation;
        randomOffset = Random.Range(0f, 100f);
    }

    private void Update() {
        DynamicWindController wind = DynamicWindController.Instance;

        if (wind == null)
            return;

        Vector3 windDirection = wind.WindVector.normalized;

        // Wind direction in local space.
        Vector3 localDirection =
            transform.parent != null
            ? transform.parent.InverseTransformDirection(windDirection)
            : windDirection;

        float windAmount = Mathf.Clamp01(
            wind.WindVector.magnitude / 10f
        );

        float sway = Mathf.Sin(
            Time.time * swayFrequency + randomOffset
        ) * swayAngle * windAmount;

        // Bend around the local Z axis for X-directed wind,
        // and the local X axis for Z-directed wind.
        Vector3 rotationAxis = new Vector3(
            localDirection.z, 0f, -localDirection.x
        );

        if (rotationAxis.sqrMagnitude > 0.001f) {
            Quaternion windRotation = Quaternion.AngleAxis(
                sway * directionInfluence,
                rotationAxis.normalized
            );

            transform.localRotation = initialRotation * windRotation;
        } else {
            transform.localRotation = initialRotation;
        }
    }
}
