using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SFXManager : MonoBehaviour
{
    [SerializeField]
    protected AudioSourcePlayerPoolSO pool;

    [SerializeField]
    protected AudioClipEventChannel eventChannel;

    private void Awake()
    {
        // Prepare pool.
        pool.SetParent(transform);
        pool.Prewarm();
    }

    private void OnEnable()
    {
        eventChannel.OnAudioPlayRequested += PlaySFX;
    }

    private void OnDisable()
    {
        eventChannel.OnAudioPlayRequested -= PlaySFX;
    }

    private void PlaySFX(AudioClip audioClip, bool shouldFade)
    {
        var audioClipPlayer = pool.Request();
        audioClipPlayer.PlayAudioClip(audioClip, shouldFade);
        audioClipPlayer.onFinishedPlaying += StopAndCleanAudioClipPlayer;
    }

    private void StopSFX(AudioClip audioClip, bool shouldFade)
    {

    }

    private void StopAndCleanAudioClipPlayer(AudioSourcePlayer audioSourcePlayer)
    {
        audioSourcePlayer.Stop();
        pool.Return(audioSourcePlayer);
    }
}
