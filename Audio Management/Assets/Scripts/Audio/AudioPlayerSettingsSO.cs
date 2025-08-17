using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio Player Settings", menuName = "Scriptable Objects/Audio/Settings/Audio Player Settings")]
public class AudioPlayerSettingsSO : ScriptableObject
{
    [SerializeField, Tooltip("Set whether the sound should play through an Audio Mixer first or directly to the Audio Listener")]
    protected AudioMixerGroup outputAudioMixerGroup;

    [Space]
    [SerializeField, Tooltip("Mutes the sound.")]
    protected bool mute;
    [SerializeField, Tooltip("Bypass/ignore any applied effects on AudioSource.")]
    protected bool bypassEffects;
    [SerializeField, Tooltip("Bypass/ignore any reverb zones.")]
    protected bool bypassReverbZones;
    [SerializeField, Tooltip("Set the source to loop. If loop points are defined in the clip, these will be respected.")]
    protected bool loop;

    [Space]
    [SerializeField, Range(0f, 1f), Tooltip("Sets the overall volume of the sound.")]
    protected float volume = 1f;
    [SerializeField, Range(-3f, 3f), Tooltip("Sets the frequency of the sound. Use this to slow down or speed up the sound.")]
    protected float pitch = 1f;
    [SerializeField, Range(-1f, 1f), Tooltip("Only valid for Mono and Stereo AudioClips. Mono sounds will be panned at constant power left and right. Stereo sounds will have each left/right value faded up and down according to the specified pan value.")] 
    protected float panStereo = 0f;
    [SerializeField, Range(0f, 1.1f), Tooltip("Sets how much of the signal this AudioSource is mixing into the global reverb associated with the zones. [0, 1] is a linear range (like volume while [1, 1.1] lets you boost the reverb mix by 10 dB.")] 
    protected float reverbZoneMix = 1f;

    [Space]
    [SerializeField, Tooltip("Defines if fade/crossfade effect should be used when playing the audio clip.")]
    protected bool shouldFade;

    public bool ShouldFade => shouldFade;

    public void ApplySettings(AudioSource source)
    {
        source.outputAudioMixerGroup = outputAudioMixerGroup;
        source.mute = mute;
        source.bypassEffects = bypassEffects;
        source.bypassReverbZones = bypassReverbZones;
        source.loop = loop;
        source.volume = volume;
        source.pitch = pitch;
        source.panStereo = panStereo;
        source.reverbZoneMix = reverbZoneMix;
    }
}
