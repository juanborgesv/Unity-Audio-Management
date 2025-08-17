using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Tooltip("The AudioSourcePlayer component responsible for playing audio cues.")]
    protected AudioSourcePlayer _audioPlayer;

    [SerializeField, Tooltip("The ScriptableObject event channel used to receive audio requests.")]
    protected AudioClipEventChannel _audioEventChannel;

    protected void OnEnable()
    {
        _audioEventChannel.OnAudioPlayRequested += PlayAudioClip;
        _audioEventChannel.OnAudioStopRequested += Stop;
        _audioEventChannel.OnAudioPauseRequested += Pause;
    }

    protected void OnDisable()
    {
        _audioEventChannel.OnAudioPlayRequested -= PlayAudioClip;
        _audioEventChannel.OnAudioStopRequested -= Stop;
        _audioEventChannel.OnAudioPauseRequested -= Pause;
    }

    /// <summary>
    /// Plays the provided AudioClip via the AudioSourcePlayer.
    /// </summary>
    /// <param name="clip">The AudioClip to be played.</param>
    protected virtual int PlayAudioClip(AudioClip clip, AudioPlayerSettingsSO settings)
    {
        // Don't play if there is no audio clip to play or if it's already playing the requested audio clip.
        if (clip == null || (clip != null && clip == _audioPlayer.ClipPlaying))
            return -1;

        _audioPlayer.PlayAudioClip(clip, settings);
        return 0;
    }

    /// <summary>
    /// Stops the currently playing music via the AudioSourcePlayer.
    /// </summary>
    protected virtual void Stop(int id, bool shouldFade) => _audioPlayer.Stop(shouldFade);

    /// <summary>
    /// Pauses the playing music via the AudioSourcePlayer.
    /// </summary>
    protected virtual void Pause(int id) => _audioPlayer.Pause();
}