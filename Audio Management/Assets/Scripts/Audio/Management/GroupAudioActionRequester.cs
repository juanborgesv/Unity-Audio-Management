using UnityEngine;

public class GroupAudioActionRequester : MonoBehaviour
{
    [SerializeField, Tooltip("The AudioClip to play.")]
    protected AudioClip clip;

    [SerializeField, Tooltip("Settings that the AudioSource will use to play the AudioClip requested.")] 
    protected AudioPlayerSettingsSO clipSettings;

    [SerializeField, Tooltip("The event channel to send the request of playing the AudioClip.")]
    protected GroupAudioEventChannel requestEventChannel;

    /// <summary>
    /// The id of an Audio Clip Player is being referenced by to play the audio
    /// clip requested.
    /// </summary>
    public int RequestId { get; protected set; } = -1;

    /// <summary>
    /// Sends a request to play an audio clip.
    /// </summary>
    public virtual void RequestPlayAudioClip()
    {
        RequestId = requestEventChannel.RaisePlayAudioEvent(clip, clipSettings);
    }

    /// <summary>
    /// Sends a request to play an audio clip from its id.
    /// </summary>
    public virtual void RequestPlayAudioClipFromId()
    {
        if (RequestId != -1)
            requestEventChannel.RaisePlayAudioEvent(RequestId);
    }

    /// <summary>
    /// Sends a request to stop the audio clip previously asked to play.
    /// </summary>
    public virtual void RequestStopAudioClip()
    {
        if (RequestId != -1)
        {
            requestEventChannel.RaiseStopAudioEvent(RequestId, false);
            RequestId = -1; // Reset.
        }
    }

    /// <summary>
    /// Sends a request to pause the audio clip previously asked to play.
    /// </summary>
    public virtual void RequestPauseAudioClip()
    {
        if (RequestId != -1)
            requestEventChannel.RaisePauseAudioEvent(RequestId);
    }
}
