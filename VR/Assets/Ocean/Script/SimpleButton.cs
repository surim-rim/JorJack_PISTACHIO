using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
[RequireComponent(typeof(AudioSource))]
public class SimpleButton : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    private AudioSource audioSource;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnButtonPressed);
        Debug.Log("[DEBUG] Button Script Enabled");
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnButtonPressed);
    }

    public void OnButtonPressed(SelectEnterEventArgs args)
    {
        Debug.Log("[DEBUG] Button Pressed! Playing Sound.");

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.Log("[DEBUG] AudioSource or Clip is missing!");
        }
    }
}
