using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    private Button button;

    void Awake()
    {
        audioSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        button.onClick.AddListener(PlaySound);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    void PlaySound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
