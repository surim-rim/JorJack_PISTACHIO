using UnityEngine;
using UnityEngine.XR;

public class FishingGame : MonoBehaviour
{
    public Transform fishingRodTip;  // 낚싯대 끝
    public Transform fishingHook;    // 낚싯바늘
    public GameObject fishingLine;   // 낚싯줄 오브젝트
    public GameObject[] fishPrefabs; // 물고기 프리팹 배열
    public float throwForceMultiplier = 15f; // 던지기 힘 조절
    public float pullSpeed = 20f;    // 낚싯줄 당기는 속도
    public XRNode controllerNode = XRNode.RightHand; // 오른손 컨트롤러 기본값

    private bool isThrown = false;   // 낚싯바늘이 던져졌는지 여부
    private bool fishCaught = false; // 물고기 잡힘 여부
    private bool fishAtRodTip = false; // 물고기가 낚싯대 끝에 도달했는지 여부
    private GameObject spawnedFish; // 생성된 물고기
    private Rigidbody hookRigidbody;
    private Vector3 previousControllerPosition;

    void Start()
    {
        hookRigidbody = fishingHook.GetComponent<Rigidbody>();
        hookRigidbody.isKinematic = true;
        hookRigidbody.useGravity = false;

        // 컨트롤러 초기 위치 저장
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        previousControllerPosition = device.TryGetFeatureValue(CommonUsages.devicePosition, out var pos) ? pos : Vector3.zero;

        Debug.Log("FishingGame 초기화 완료");
    }

    void Update()
    {
        if (isThrown && !fishCaught)
        {
            LimitFishingLineLength();
        }

        if (fishCaught && !fishAtRodTip)
        {
            CheckControllerUpwardMotion();
        }

        UpdateFishingLine();
    }

    public void ThrowFishingHook(Vector3 velocity)
    {
        if (isThrown) return;

        isThrown = true;
        hookRigidbody.isKinematic = false;
        hookRigidbody.useGravity = true;

        fishingHook.position = fishingRodTip.position + Vector3.down * 0.5f;
        hookRigidbody.velocity = velocity * throwForceMultiplier;

        Debug.Log("낚싯바늘 던지기 시작");

        float randomTime = Random.Range(3f, 8f);
        Invoke(nameof(CatchFish), randomTime);
    }

    void CatchFish()
    {
        if (!isThrown) return;

        fishCaught = true;

        int randomIndex = Random.Range(0, fishPrefabs.Length);
        spawnedFish = Instantiate(fishPrefabs[randomIndex], fishingHook.position, Quaternion.identity);
        spawnedFish.transform.SetParent(fishingHook);

        Rigidbody fishRigidbody = spawnedFish.GetComponent<Rigidbody>();
        if (fishRigidbody != null)
        {
            fishRigidbody.isKinematic = true;
            fishRigidbody.useGravity = false;
        }

        Debug.Log($"물고기가 잡혔습니다: {spawnedFish.name}");
    }

    void CheckControllerUpwardMotion()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid)
        {
            Debug.LogWarning("컨트롤러가 유효하지 않습니다.");
            return;
        }

        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 currentPosition))
        {
            Vector3 velocity = (currentPosition - previousControllerPosition) / Mathf.Max(Time.deltaTime, 0.01f);

            if (velocity.y > 1.0f)
            {
                PullFishAndShortenLine();
            }

            previousControllerPosition = currentPosition;
        }
    }

    void PullFishAndShortenLine()
    {
        if (!fishCaught || fishingHook == null) return;

        hookRigidbody.isKinematic = true;
        hookRigidbody.useGravity = false;

        fishingHook.position = Vector3.MoveTowards(fishingHook.position, fishingRodTip.position, pullSpeed);

        UpdateFishingLine();

        if (Vector3.Distance(fishingHook.position, fishingRodTip.position) < 0.1f)
        {
            Debug.Log("물고기가 낚싯대 끝에 도달했습니다.");
            fishAtRodTip = true;
            AllowFishToBeGrabbed();
        }
    }

    void AllowFishToBeGrabbed()
    {
        if (spawnedFish == null) return;

        // 물고기를 손으로만 잡을 수 있도록 설정
        Rigidbody fishRigidbody = spawnedFish.GetComponent<Rigidbody>();
        if (fishRigidbody != null)
        {
            fishRigidbody.isKinematic = true; // 사용자가 잡을 때까지 고정
        }

        Debug.Log("물고기가 손으로 잡을 준비가 되었습니다.");
    }

    public void GrabFishManually()
    {
        if (!fishAtRodTip || spawnedFish == null) return;

        // 물고기를 손으로 잡았을 때만 낚싯대에서 분리
        spawnedFish.transform.SetParent(null);

        Rigidbody fishRigidbody = spawnedFish.GetComponent<Rigidbody>();
        if (fishRigidbody != null)
        {
            fishRigidbody.isKinematic = false;
            fishRigidbody.useGravity = true;
        }

        Debug.Log("물고기가 분리되었습니다.");
        ResetFishing();
    }

    void LimitFishingLineLength()
    {
        if (Vector3.Distance(fishingRodTip.position, fishingHook.position) > 10f)
        {
            Vector3 direction = (fishingHook.position - fishingRodTip.position).normalized;
            fishingHook.position = fishingRodTip.position + direction * 10f;

            Debug.LogWarning("낚싯줄이 최대 길이를 초과하여 제한되었습니다.");
        }
    }

    void UpdateFishingLine()
    {
        if (fishingLine != null && fishingRodTip != null && fishingHook != null)
        {
            Vector3 direction = fishingHook.position - fishingRodTip.position;

            fishingLine.transform.position = fishingRodTip.position + direction / 2;
            fishingLine.transform.rotation = Quaternion.LookRotation(direction);
            fishingLine.transform.localScale = new Vector3(0.01f, 0.01f, direction.magnitude);

            Debug.Log($"낚싯줄 길이 업데이트: {direction.magnitude}");
        }
        else
        {
            Debug.LogError("낚싯줄, 낚싯대 끝 또는 낚싯바늘이 연결되지 않았습니다!");
        }
    }

    public void ResetFishing()
    {
        isThrown = false;
        fishCaught = false;
        fishAtRodTip = false;

        hookRigidbody.isKinematic = true;
        hookRigidbody.useGravity = false;

        fishingHook.position = fishingRodTip.position + Vector3.down * 0.5f;

        UpdateFishingLine();
    }
}
