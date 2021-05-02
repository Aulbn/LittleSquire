using UnityEngine;

public class PulsateLight : MonoBehaviour
{
    private Light light;
    private bool decrease;

    [SerializeField] private float emissiveIntensity = 50f;
    [SerializeField] private float pulseSpeed = 5f;
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        if (decrease)
            light.intensity -= Time.deltaTime * pulseSpeed;
        else
            light.intensity += Time.deltaTime * pulseSpeed;

        if (light.intensity >= emissiveIntensity)
            decrease = true;
        else if (light.intensity <= 1f)
            decrease = false;        
    }
}
