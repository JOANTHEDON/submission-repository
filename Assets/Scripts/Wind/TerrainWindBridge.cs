
using UnityEngine;

public class TerrainWindBridge : MonoBehaviour {
    [SerializeField] private Terrain targetTerrain;
    [SerializeField] private WindZone targetWindZone;

    [Header("Wind Mapping")]
    [SerializeField] private float grassSpeedMultiplier = 0.15f;
    [SerializeField] private float grassBendingMultiplier = 0.5f;
    [SerializeField] private float treeWindMultiplier = 0.1f;

    private TerrainData terrainData;

    private void Awake() {
        if (targetTerrain != null)
            terrainData = targetTerrain.terrainData;
    }

    private void Update() {
        DynamicWindController wind = DynamicWindController.Instance;

        if (wind == null)
            return;

        float speed = wind.windSpeed;
        float strength = wind.EffectiveStrength;

        // Terrain's built-in grass animation.
        if (terrainData != null) {
            terrainData.wavingGrassSpeed =
                Mathf.Clamp(speed * grassSpeedMultiplier, 0f, 1f);

            terrainData.wavingGrassAmount =
                Mathf.Clamp(strength * grassBendingMultiplier, 0f, 1f);

            terrainData.wavingGrassStrength =
                Mathf.Clamp(strength * grassBendingMultiplier, 0f, 1f);
        }

        // Native Terrain tree wind.
        if (targetWindZone != null) {
            Vector3 direction = wind.WindVector;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
                targetWindZone.transform.rotation =
                    Quaternion.LookRotation(direction.normalized);

            targetWindZone.windMain =
                Mathf.Clamp(speed * strength * treeWindMultiplier, 0f, 1f);

            targetWindZone.windTurbulence =
                Mathf.Clamp(strength, 0f, 1f);
        }
    }
}
