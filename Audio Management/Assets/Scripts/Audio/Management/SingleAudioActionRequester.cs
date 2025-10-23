using UnityEngine;

public class SingleAudioActionRequester : MonoBehaviour
{
    [SerializeField, Tooltip("The AudioClip to play.")]
    protected AudioClip clip;

    [SerializeField, Tooltip("Settings that the AudioSource will use to play the AudioClip requested.")]
    protected AudioPlayerSettingsSO clipSettings;

    [SerializeField, Tooltip("The event channel to send the request of playing the AudioClip.")]
    protected SingleAudioEventChannel requestEventChannel;

    /// <summary>
    /// Sends a request to play an audio clip.
    /// </summary>
    public virtual void RequestPlayAudioClip()
    {
        requestEventChannel.RaisePlayAudioEvent(clip, clipSettings);
    }

    /// <summary>
    /// Sends a request to stop the audio clip previously asked to play.
    /// </summary>
    public virtual void RequestStopAudioClip()
    {
        requestEventChannel.RaiseStopAudioEvent(false);
    }

    /// <summary>
    /// Sends a request to pause the audio clip previously asked to play.
    /// </summary>
    public virtual void RequestPauseAudioClip()
    {
        requestEventChannel.RaisePauseAudioEvent();
    }
}
