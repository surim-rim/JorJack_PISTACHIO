using UnityEngine;

public class FishingThrowController : MonoBehaviour
{
    public FishingGame fishingGame; // FishingGame 스크립트 참조
    public float throwForceMultiplier = 10f;

    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;

        if (currentVelocity.y < -2.0f)
        {
            fishingGame.ThrowFishingHook(currentVelocity * throwForceMultiplier);
        }

        previousPosition = transform.position;
    }
}
