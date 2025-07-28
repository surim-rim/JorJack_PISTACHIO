using UnityEngine;
using System.Collections;

public class FireworkRocket : MonoBehaviour
{
    [Header("Effects")]
    public ParticleSystem[] explosionEffects;
    public AudioSource launchSound;
    public AudioSource explosionSound;

    [Header("Settings")]
    public float delayBeforeLaunch = 3f;
    public float launchForce = 40f;
    public float timeBetweenExplosions = 0.5f;

    private Rigidbody rb;
    private bool launched = false;
    private float fuseTimer = 0f;
    private bool fuseLit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (fuseLit && !launched)
        {
            fuseTimer += Time.deltaTime;

            if (fuseTimer >= delayBeforeLaunch)
            {
                Launch();
            }
        }
    }

    public void LightFuse()
    {
        fuseLit = true;
        fuseTimer = 0f;
    }

    public void StopFuse()
    {
        fuseLit = false;
        fuseTimer = 0f;
    }

    public void Launch()
    {
        if (launched) return;
        launched = true;

        rb.isKinematic = false;
        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        if (launchSound != null) launchSound.Play();

        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        yield return new WaitForSeconds(2f); // 비행 시간

        if (explosionSound != null) explosionSound.Play();

        foreach (var fx in explosionEffects)
        {
            fx.Play();
            yield return new WaitForSeconds(timeBetweenExplosions);
        }

        Destroy(gameObject, 2f); // 다 끝나고 삭제
    }
}
