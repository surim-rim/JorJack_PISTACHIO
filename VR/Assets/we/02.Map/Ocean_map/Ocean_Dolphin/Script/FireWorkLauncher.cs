using UnityEngine;
using System.Collections;

public class FireworkLauncher : MonoBehaviour
{
    public ParticleSystem[] fireworks;
    public float delayBetween = 0.5f;

    public void Activate()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        foreach (var fx in fireworks)
        {
            if (fx != null)
                fx.Play();

            yield return new WaitForSeconds(delayBetween);
        }
    }
}
