using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ScriptableObject-based event channel to handle AudioClip action requests.
/// </summary>
[CreateAssetMenu(fileName = "AudioClip Event Channel", menuName = "Scriptable Objects/Event Channel/AudioClip Event Channel")]
public class AudioClipEventChannel : ScriptableObject
{
    /// <summary>
    /// Event triggered when an audio play action request is raised.
    /// </summary>
    public AudioPlayAction OnAudioPlayRequested;

    /// <summary>
    /// Event triggered when an audio stop action request is raised.
    /// </summary>
    public AudioStopAction OnAudioStopRequested;

    /// <summary>
    /// Event triggered when an audio pause action request is raised.
    /// </summary>
    public AudioPauseAction OnAudioPauseRequested;

    /// <summary>
    /// Raises the event to request an AudioClip to be played.
    /// </summary>
    /// <param name="clip"></param>
    /// /// <param name="settings"></param>
    public void RaisePlayAudioEvent(AudioClip clip, AudioPlayerSettingsSO settings)
    {
        if (clip != null && OnAudioPlayRequested != null)
            OnAudioPlayRequested.Invoke(clip, settings);
    }

    /// <summary>
    /// Raises the event to request stopping the currently playing audio.
    /// </summary>
    /// <param name="shouldFade"></param>
    public void RaiseStopAudioEvent(bool shouldFade)
    {
        if (OnAudioStopRequested != null)
            OnAudioStopRequested.Invoke(0, shouldFade);
    }

    /// <summary>
    /// Raises the event to request stopping the currently playing audio.
    /// </summary>
    /// <param name="id">The id of the audio source player to stop.</param>
    /// <param name="shouldFade"></param>
    public void RaiseStopAudioEvent(int id, bool shouldFade)
    {
        if (OnAudioStopRequested != null)
            OnAudioStopRequested.Invoke(id, shouldFade);
    }

    /// <summary>
    /// Raises the event to request pausing the currently playing audio.
    /// </summary>
    public void RaisePauseAudioEvent(int id)
    {
        if (OnAudioPauseRequested != null)
            OnAudioPauseRequested.Invoke(id);
    }
}

public delegate int AudioPlayAction(AudioClip clip, AudioPlayerSettingsSO settings);
public delegate void AudioStopAction(int id, bool fade);
public delegate void AudioPauseAction(int id);