using UnityEngine;

public class SpotLightBlinker : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light spotLight;
    [SerializeField] private float onDuration = 0.5f;
    [SerializeField] private float offDuration = 0.5f;
    [SerializeField] private bool startOn = true;

    private float timer;
    private bool isLightOn;

    void Start()
    {
        // If no spotlight is assigned, try to find one on this GameObject
        if (spotLight == null)
        {
            spotLight = GetComponent<Light>();
            if (spotLight == null || spotLight.type != LightType.Spot)
            {
                Debug.LogError("No Spot Light component found or assigned to this GameObject.");
                enabled = false; // Disable the script if no valid spotlight is found
                return;
            }
        }

        isLightOn = startOn;
        spotLight.enabled = isLightOn;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isLightOn)
        {
            if (timer >= onDuration)
            {
                spotLight.enabled = false;
                isLightOn = false;
                timer = 0f;
            }
        }
        else
        {
            if (timer >= offDuration)
            {
                spotLight.enabled = true;
                isLightOn = true;
                timer = 0f;
            }
        }
    }
}