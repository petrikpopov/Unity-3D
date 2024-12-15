using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private float charge;
    private float worktime = 100.0f;
    private Light flashLight;

    public float chargeLevel => charge;

    void Start()
    {
        charge = 1.0f;
        flashLight = GetComponent<Light>();
        GameState.AddEventListener(ItemCollected, "Battery");
    }

    private void ItemCollected(string eventName, object data)
    {
        if(data is GameEvents.MessageEvent m)
        {
            charge += (float) m.data;
        }
    }

    void Update()
    {
        if (!GameState.isDay)
        {
            if(charge > 0)
            {
                flashLight.intensity = Mathf.Clamp01(charge);
                charge -= Time.deltaTime / worktime;
            }
        }

        if (GameState.isFpv)
        {
            this.transform.forward = Camera.main.transform.forward;
        }
        else
        {
            Vector3 f = Camera.main.transform.forward;
            f.y = 0.0f;
            if (f == Vector3.zero) f = Camera.main.transform.up;
            this.transform.forward = f.normalized;
        }        
    }

    private void OnDestroy()
    {
        GameState.RemoveEventListener(ItemCollected, "Battery");
    }
}
