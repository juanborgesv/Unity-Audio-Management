using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ScriptableObject-based event channel to handle AudioClip requests.
/// </summary>
[CreateAssetMenu(fileName = "AudioClip Event Channel", menuName = "ScriptableObject/Event Channel/AudioClip Event Channel")]
public class AudioClipEventChannel : ScriptableObject
{
    /// <summary>
    /// Event triggered when an audio play action request is raised.
    /// </summary>
    public UnityAction<AudioClip, bool> OnAudioPlayRequested;

    /// <summary>
    /// Event triggered when an audio stop action request is raised.
    /// </summary>
    public UnityAction<bool> OnAudioStopRequested;

    /// <summary>
    /// Raises the event to request an AudioClip to be played.
    /// </summary>
    /// <param name="clip"></param>XD
    public void RaisePlayAudioEvent(AudioClip clip, bool shouldFade)
    {
        if (clip != null && OnAudioPlayRequested != null)
            OnAudioPlayRequested.Invoke(clip, shouldFade);
    }

    /// <summary>
    /// Raises the event to request stopping the currently playing audio.
    /// </summary>
    public void RaiseStopAudioEvent(bool shouldFade)
    {
        if (OnAudioStopRequested != null)
            OnAudioStopRequested.Invoke(shouldFade);
    }
}
