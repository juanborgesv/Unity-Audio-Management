using UnityEngine;

public class AudioRequester : MonoBehaviour
{
    [SerializeField]
    protected AudioClip clip;

    [SerializeField] 
    protected bool shouldFade;

    [SerializeField]
    protected AudioClipEventChannel requestEventChannel;

    [ContextMenu("Request")]
    protected void RequestPlayAudioClip()
    {
        requestEventChannel.RaisePlayAudioEvent(clip, shouldFade);
    }
}
