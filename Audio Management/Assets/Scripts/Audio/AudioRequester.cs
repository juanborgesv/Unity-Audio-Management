using UnityEngine;

public class AudioRequester : MonoBehaviour
{
    [SerializeField, Tooltip("The AudioClip to play.")]
    protected AudioClip clip;

    [SerializeField, Tooltip("Settings that the AudioSource will use to play the AudioClip requested.")] 
    protected AudioPlayerSettingsSO clipSettings;

    [SerializeField, Tooltip("The event channel to send the request of playing the AudioClip.")]
    protected AudioClipEventChannel requestEventChannel;

    [ContextMenu("Request")]
    public void RequestPlayAudioClip() => requestEventChannel.RaisePlayAudioEvent(clip, clipSettings);
}
