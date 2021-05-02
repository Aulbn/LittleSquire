using UnityEngine;

public class PulsateMaterial : MonoBehaviour
{
    private MeshRenderer rend;
    private float intensity;
    private bool decrease;

    [SerializeField] private float emissiveIntensity = 15f;
    [SerializeField] private float pulseSpeed = 5f;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (decrease)
            intensity -= Time.deltaTime * pulseSpeed;
        else
            intensity += Time.deltaTime * pulseSpeed;

        rend.material.SetColor("_EmissiveColor", rend.material.color * intensity);

        if (intensity >= emissiveIntensity)
            decrease = true;
        else if (intensity <= 1f)
            decrease = false;
    }
}
