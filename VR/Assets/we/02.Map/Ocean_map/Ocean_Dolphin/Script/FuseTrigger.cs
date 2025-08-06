using UnityEngine;

public class FuseTrigger : MonoBehaviour
{
    private FireworkRocket rocket;

    void Start()
    {
        rocket = GetComponentInParent<FireworkRocket>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lighter"))
        {
            rocket.LightFuse();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lighter"))
        {
            rocket.StopFuse(); // ´ê´Ù°¡ ºüÁö¸é Ç»Áî ²¨Áü
        }
    }
}
