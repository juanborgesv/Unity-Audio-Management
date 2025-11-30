using UnityEngine;

/// <summary>
/// A manager class responsible for handling audio playback for a specific 
/// Audio Source Player.
/// </summary>
public class SingleAudioManager : MonoBehaviour
{
    [SerializeField, Tooltip("The AudioSourcePlayer component responsible for playing audio cues.")]
    protected AudioSourcePlayer _audioPlayer;

    [SerializeField, Tooltip("The ScriptableObject event channel used to receive audio requests.")]
    protected SingleAudioEventChannel _eventChannel;

    protected virtual void OnEnable()
    {
        _eventChannel.OnAudioPlayRequested += PlayAudioClip;
        _eventChannel.OnAudioStopRequested += Stop;
        _eventChannel.OnAudioPauseRequested += Pause;
    }

    protected virtual void OnDisable()
    {
        _eventChannel.OnAudioPlayRequested -= PlayAudioClip;
        _eventChannel.OnAudioStopRequested -= Stop;
        _eventChannel.OnAudioPauseRequested -= Pause;
    }

    /// <summary>
    /// Plays the provided AudioClip via the AudioSourcePlayer.
    /// </summary>
    /// <param name="clip">The AudioClip to be played.</param>
    protected virtual int PlayAudioClip(AudioClip clip, AudioPlayerSettingsSO settings)
    {
        // Don't play if there is no audio clip to play or if the requested
        // audio clip is already playing.
        if (clip == null || (clip != null && clip == _audioPlayer.ClipPlaying && _audioPlayer.IsPlaying))
            return -1;

        _audioPlayer.PlayAudioClip(clip, settings);
        return 0;
    }

    /// <summary>
    /// Stops the currently playing music via the AudioSourcePlayer.
    /// </summary>
    protected virtual void Stop(bool shouldFade) => _audioPlayer.Stop(shouldFade);

    /// <summary>
    /// Pauses the playing music via the AudioSourcePlayer.
    /// </summary>
    protected virtual void Pause() => _audioPlayer.Pause();
}