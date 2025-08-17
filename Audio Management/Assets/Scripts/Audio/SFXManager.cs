using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SFXManager : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    protected AudioSourcePlayerPoolSO pool;

    [SerializeField, Tooltip("")]
    protected AudioClipEventChannel eventChannel;

    protected AudioSourcePlayerContainer audioContainer;

    private void Awake()
    {
        // Prepare pool.
        pool.SetParent(transform);
        pool.Prewarm();
    }

    private void OnEnable()
    {
        eventChannel.OnAudioPlayRequested += PlayAudioClip;
        eventChannel.OnAudioStopRequested += StopAudioClipPlayer;
        eventChannel.OnAudioPauseRequested += PauseAudioClipPlayer;
    }

    private void OnDisable()
    {
        eventChannel.OnAudioPlayRequested -= PlayAudioClip;
        eventChannel.OnAudioStopRequested -= StopAudioClipPlayer;
        eventChannel.OnAudioPauseRequested -= PauseAudioClipPlayer;
    }

    protected int PlayAudioClip(AudioClip audioClip, AudioPlayerSettingsSO settings)
    {
        var audioClipPlayer = pool.Request();

        int audioPlayerKey = audioContainer.Add(audioClipPlayer);

        audioClipPlayer.PlayAudioClip(audioClip, settings);
        audioClipPlayer.onFinishedPlaying += StopAudioClipPlayer;

        return audioPlayerKey;
    }

    protected void StopAudioClipPlayer(AudioSourcePlayer audioSourcePlayer)
    {
        audioSourcePlayer.Stop();
        pool.Return(audioSourcePlayer);
    }

    protected void StopAudioClipPlayer(int id, bool shouldFade)
    {
        if (audioContainer.TryGet(id, out AudioSourcePlayer audioSourcePlayer))
        {
            StopAudioClipPlayer(audioSourcePlayer);

            audioContainer.Remove(id);
        }
    }

    protected void PauseAudioClipPlayer(int id)
    {
        if (audioContainer.TryGet(id, out AudioSourcePlayer audioSourcePlayer))
            audioSourcePlayer.Pause();
    }
}
