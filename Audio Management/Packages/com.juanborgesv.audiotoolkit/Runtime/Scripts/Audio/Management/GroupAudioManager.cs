using UnityEngine;

/// <summary>
/// A manager class responsible for handling audio playback for a specific group 
/// of sounds.
/// </summary>
public class GroupAudioManager : MonoBehaviour
{
    [SerializeField, Tooltip("The pool of reusable AudioSourcePlayer instances.")]
    protected AudioSourcePlayerPoolSO pool;

    [SerializeField, Tooltip("The event channel used to send play requests for audio clips.")]
    protected GroupAudioEventChannel eventChannel;

    /// <summary>
    /// A container to track and manage currently active audio players.
    /// </summary>
    public AudioSourcePlayerContainer AudioContainer { get; protected set; }

    private void Awake()
    {
        AudioContainer = new();

        // Prepare pool.
        pool.SetParent(transform);
        pool.Prewarm();
    }

    protected virtual void OnEnable()
    {
        eventChannel.OnAudioPlayRequested += PlayAudioClip;
        eventChannel.OnAudioResumeRequested += ResumeAudioClip;
        eventChannel.OnAudioStopRequested += StopAudioClipPlayer;
        eventChannel.OnAudioPauseRequested += PauseAudioClipPlayer;
    }

    protected virtual void OnDisable()
    {
        eventChannel.OnAudioPlayRequested -= PlayAudioClip;
        eventChannel.OnAudioResumeRequested -= ResumeAudioClip;
        eventChannel.OnAudioStopRequested -= StopAudioClipPlayer;
        eventChannel.OnAudioPauseRequested -= PauseAudioClipPlayer;
    }

    /// <summary>
    /// Plays an audio clip from a pool of reusable players, returning a key to manage it.
    /// </summary>
    /// <param name="audioClip">The audio clip to be played.</param>
    /// <param name="settings">Settings for the audio player.</param>
    /// <returns>A unique integer key for the audio player instance.</returns>
    protected int PlayAudioClip(AudioClip audioClip, AudioPlayerSettingsSO settings)
    {
        var audioClipPlayer = pool.Request();

        int audioPlayerKey = AudioContainer.Add(audioClipPlayer);

        audioClipPlayer.PlayAudioClip(audioClip, settings);
        audioClipPlayer.onFinishedPlaying += StopAudioClipPlayer;

        return audioPlayerKey;
    }

    /// <summary>
    /// Resumes playback of a paused audio clip.
    /// </summary>
    /// <param name="id">The unique ID of the audio player to resume.</param>
    protected void ResumeAudioClip(int id)
    {
        if (AudioContainer.TryGet(id, out AudioSourcePlayer audioSourcePlayer) && !audioSourcePlayer.IsPlaying)
            audioSourcePlayer.Play();
    }

    /// <summary>
    /// Stops a specific audio player and returns it to the pool.
    /// </summary>
    /// <param name="audioSourcePlayer">The audio source player to stop and return.</param>
    protected void StopAudioClipPlayer(AudioSourcePlayer audioSourcePlayer)
    {
        // Stop listening to this event, also to prevent this event to
        // accumulate calls to this method.
        audioSourcePlayer.onFinishedPlaying -= StopAudioClipPlayer;

        audioSourcePlayer.Stop();

        pool.Return(audioSourcePlayer);
        AudioContainer.Remove(audioSourcePlayer);
    }

    /// <summary>
    /// Stops an audio player by its ID, removing it from the tracking container and returning it to the pool.
    /// </summary>
    /// <param name="id">The unique ID of the audio player to stop.</param>
    /// <param name="shouldFade">Indicates if the audio should fade out before stopping (parameter is not currently used).</param>
    protected void StopAudioClipPlayer(int id, bool shouldFade)
    {
        if (AudioContainer.TryGet(id, out AudioSourcePlayer audioSourcePlayer))
        {
            StopAudioClipPlayer(audioSourcePlayer);

            AudioContainer.Remove(id);
        }
    }

    /// <summary>
    /// Pauses an audio player identified by its ID.
    /// </summary>
    /// <param name="id">The unique ID of the audio player to pause.</param>
    protected void PauseAudioClipPlayer(int id)
    {
        if (AudioContainer.TryGet(id, out AudioSourcePlayer audioSourcePlayer))
            audioSourcePlayer.Pause();
    }
}
