using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource src;
    public AudioClip input, delete, correct, incorrect;

    private void Start()
    {
        EventsManager.OnCorrectGuess += GuessCorrectSound;
        EventsManager.OnIncorrectGuess += GuessIncorrectSound;
        EventsManager.OnTextDelete += TextDeleteSound;
        EventsManager.OnTextInput += TextInputSound;
    }

    public void TextInputSound()
    {
        StartCoroutine(PlaySound(Instantiate(src), input));
    }

    public void TextDeleteSound()
    {
        StartCoroutine(PlaySound(Instantiate(src), delete));
    }

    public void GuessCorrectSound(List<string> words = null)
    {
        StartCoroutine(PlaySound(Instantiate(src), correct));
    }

    public void GuessIncorrectSound()
    {
        StartCoroutine(PlaySound(Instantiate(src), incorrect));
    }

    IEnumerator PlaySound(AudioSource src, AudioClip clip)
    {
        src.clip = clip;
        src.Play();
        yield return new WaitForSeconds(1);
        Destroy(src.gameObject);
    }

    private void OnDestroy()
    {
        EventsManager.OnCorrectGuess -= GuessCorrectSound;
        EventsManager.OnIncorrectGuess -= GuessIncorrectSound;
        EventsManager.OnTextDelete -= TextDeleteSound;
        EventsManager.OnTextInput -= TextInputSound;

    }
}
