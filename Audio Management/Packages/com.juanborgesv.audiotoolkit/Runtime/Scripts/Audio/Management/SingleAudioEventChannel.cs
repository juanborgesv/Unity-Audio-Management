using System;

using UnityEngine;

[CreateAssetMenu(fileName = "AudioClip Event Channel", menuName = "Scriptable Objects/Event Channel/Single Audio Event Channel")]
public class SingleAudioEventChannel : ScriptableObject
{
    /// <summary>
    /// Event triggered when an audio play action request is raised.
    /// </summary>
    public AudioPlayAction OnAudioPlayRequested;

    /// <summary>
    /// Event triggered when an audio stop action request is raised.
    /// </summary>
    public event Action<bool> OnAudioStopRequested;

    /// <summary>
    /// Event triggered when an audio pause action request is raised.
    /// </summary>
    public event Action OnAudioPauseRequested;

    /// <summary>
    /// Raises the event to request an AudioClip to be played.
    /// </summary>
    /// <param name="clip"></param>
    /// /// <param name="settings"></param>
    public int RaisePlayAudioEvent(AudioClip clip, AudioPlayerSettingsSO settings)
    {
        if (clip != null && OnAudioPlayRequested != null)
            return OnAudioPlayRequested.Invoke(clip, settings);

        return -1; // Invalid ID due to not playing the audio requested.
    }

    /// <summary>
    /// Raises the event to request stopping the currently playing audio.
    /// </summary>
    /// <param name="shouldFade"></param>
    public void RaiseStopAudioEvent(bool shouldFade)
    {
        if (OnAudioStopRequested != null)
            OnAudioStopRequested.Invoke(shouldFade);
    }

    /// <summary>
    /// Raises the event to request pausing the currently playing audio.
    /// </summary>
    public void RaisePauseAudioEvent()
    {
        if (OnAudioPauseRequested != null)
            OnAudioPauseRequested.Invoke();
    }
}
