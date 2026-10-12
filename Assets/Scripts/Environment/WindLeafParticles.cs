
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WindLeafParticles : MonoBehaviour {
    private ParticleSystem particles;
    private ParticleSystem.VelocityOverLifetimeModule velocity;

    [SerializeField] private float fallSpeed = 0.5f;

    private void Awake() {
        particles = GetComponent<ParticleSystem>();

        velocity = particles.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.World;
    }

    private void Update() {
        DynamicWindController wind =
            DynamicWindController.Instance;

        if (wind == null)
            return;

        Vector3 windVector = wind.WindVector;

        velocity.x = new ParticleSystem.MinMaxCurve(windVector.x);
        velocity.y = new ParticleSystem.MinMaxCurve(-fallSpeed);
        velocity.z = new ParticleSystem.MinMaxCurve(windVector.z);
    }
}
