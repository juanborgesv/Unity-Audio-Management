using UnityEngine;


public class LastSingleAudioActionRequester : SingleAudioActionRequester
{
    private void OnEnable()
    {
        requestEventChannel.OnAudioPlayRequested += GetAudioClipRequestedToPlay;
    }

    private void OnDisable()
    {
        requestEventChannel.OnAudioPlayRequested += GetAudioClipRequestedToPlay;
    }

    protected int GetAudioClipRequestedToPlay(AudioClip clip, AudioPlayerSettingsSO settings)
    {
        this.clip = clip;
        clipSettings = settings;

        return 0;
    }

    private void OnValidate()
    {
        // Force this component to not contain clip nor settings to work as its
        // not intended.
        if (!Application.isPlaying)
        {
            clip = null;
            clipSettings = null;
        }
    }
}
