using UnityEngine;

[CreateAssetMenu(fileName = "New AudioSourcePlayer Pool", menuName = "Scriptable Objects/Pool/AudioSourcePlayer Pool")]
public class AudioSourcePlayerPoolSO : ComponentPoolSO<AudioSourcePlayer>
{
    [SerializeField]
    private AudioSourcePlayerFactorySO _factory;

    public override IFactory<AudioSourcePlayer> Factory 
    { 
        get => _factory; 
        set => _factory = value as AudioSourcePlayerFactorySO; 
    }
}
