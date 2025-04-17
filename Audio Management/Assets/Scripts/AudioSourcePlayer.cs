using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages an AudioSource component, providing functionality to play audio 
/// clips with optional fade-in and fade-out effects.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioSourcePlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0f), Tooltip("The duration of the fade effect in seconds.")]
    private float _fadeDuration = 0.5f;

    /// <summary>
    /// The audio source component attached to this game object.
    /// </summary>
    AudioSource _audioSource;

    /// <summary>
    /// The coroutine handling the fade effect.
    /// </summary>
    Coroutine _fadeCoroutine;

    /// <summary>
    /// Event that notifies when the audio source has finished playing the 
    /// audio clip given.
    /// </summary>
    public event UnityAction<AudioSourcePlayer> onFinishedPlaying;

    /// <summary>
    /// Is the audio clip playing right now.
    /// </summary>
    public bool IsPlaying => _audioSource.isPlaying;

    /// <summary>
    /// The audio clip being played.
    /// </summary>
    public AudioClip ClipPlaying => _audioSource.clip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the audio source.
    /// </summary>
    /// <param name="shouldFadeOut">Whether to fade out the audio before playing.</param>
    public void Play(bool shouldFadeOut = false)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        if (shouldFadeOut && _fadeDuration > 0f)
            _fadeCoroutine = StartCoroutine(FadeAudio(_audioSource.volume, 1, () => _audioSource.Play()));
        else if (_audioSource.clip != null)
        {
            _audioSource.Play();

            if (!_audioSource.loop)
                StartCoroutine(WaitAndNotifyWhenDone(_audioSource.clip.length));
        }
    }

    /// <summary>
    /// Pauses the audio source.
    /// </summary>
    public void Pause() => _audioSource.Pause();

    /// <summary>
    /// Stop the audio source.
    /// </summary>
    /// <param name="shouldFadeOut">Whether to fade out the audio before stopping.</param>
    public void Stop(bool shouldFadeOut = false)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        if (shouldFadeOut && _fadeDuration > 0f)
            _fadeCoroutine = StartCoroutine(FadeAudio(_audioSource.volume, 0, () => _audioSource.Stop()));
        else
            _audioSource.Stop();
    }

    /// <summary>
    /// Plays the specified audio clip.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="shouldFade">Whether to fade in the audio clip.</param>
    public void PlayAudioClip(AudioClip clip, AudioPlayerSettingsSO settings)
    {
        // Apply settings to the audio source.
        settings.ApplySettings(_audioSource);

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        if (settings.ShouldFade && _fadeDuration > 0f)
        {
            if (_audioSource.isPlaying)
            {
                // Transition from previously playing audio clip to new one with crossfade effect.
                _fadeCoroutine = StartCoroutine(Crossfade(clip));
            }
            else
            {
                // Start playing the audio clip while fading in the audio.
                _audioSource.clip = clip;
                _audioSource.volume = 0;
                _audioSource.Play();
                _fadeCoroutine = StartCoroutine(FadeAudio(0, 1));

                if (!_audioSource.loop)
                    StartCoroutine(WaitAndNotifyWhenDone(clip.length));
            }
        }
        else
        {
            // Play AudioClip directly at normal volume immediately.
            _audioSource.clip = clip;
            _audioSource.Play();

            if (!_audioSource.loop)
                StartCoroutine(WaitAndNotifyWhenDone(clip.length));
        }
    }

    /// <summary>
    /// Crossfades between the currently playing audio clip and the specified audio clip.
    /// </summary>
    /// <param name="clip">The audio clip to crossfade to.</param>
    /// <returns></returns>
    protected IEnumerator Crossfade(AudioClip clip)
    {
        // Fade out current audio.
        yield return FadeAudio(_audioSource.volume, 0);

        // Swap.
        _audioSource.clip = clip;
        _audioSource.Play();

        if (!_audioSource.loop)
            StartCoroutine(WaitAndNotifyWhenDone(clip.length));

        // Fade in current audio.
        yield return FadeAudio(_audioSource.volume, 1);
    }

    /// <summary>
    /// Fades the audio source's volume over time.
    /// </summary>
    /// <param name="startVolume">The starting volume.</param>
    /// <param name="targetVolume">The target volume.</param>
    /// <param name="OnComplete">An optional callback to invoke when the fade is complete.</param>
    /// <returns></returns>
    protected IEnumerator FadeAudio(float startVolume, float targetVolume, Action OnComplete = null)
    {
        float timeElapsed = 0f;

        // Fade volume.
        while (timeElapsed < _fadeDuration)
        {
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / _fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Override volume to target to ensure the correct volume is set.
        _audioSource.volume = targetVolume;

        // End.
        _fadeCoroutine = null;
        OnComplete?.Invoke();
    }

    protected IEnumerator WaitAndNotifyWhenDone(float clipDuration)
    {
        yield return new WaitForSeconds(clipDuration);

        onFinishedPlaying?.Invoke(this);
    }
}
