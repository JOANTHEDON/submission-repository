
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WindPhysicsController : MonoBehaviour {
    [Header("Wind Response")]
    [SerializeField] private float windResponse = 1f;
    [SerializeField] private float maxWindAcceleration = 8f;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        DynamicWindController wind =
            DynamicWindController.Instance;

        if (wind == null || rb.isKinematic)
            return;

        // Desired movement from the current wind.
        Vector3 windVelocity = wind.WindVector;

        // Accelerate toward the wind velocity, without
        // applying unlimited force.
        Vector3 relativeWind = windVelocity - rb.linearVelocity;
        Vector3 acceleration = relativeWind * windResponse;

        acceleration = Vector3.ClampMagnitude(
            acceleration,
            maxWindAcceleration
        );

        rb.AddForce(acceleration, ForceMode.Acceleration);
    }
}
