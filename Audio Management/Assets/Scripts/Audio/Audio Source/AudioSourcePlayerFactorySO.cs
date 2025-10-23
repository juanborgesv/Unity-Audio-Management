using UnityEngine;

[CreateAssetMenu(fileName = "New AudioSourcePlayer Factory", menuName = "Scriptable Objects/Factory/AudioSourcePlayer Factory")]
public class AudioSourcePlayerFactorySO : FactorySO<AudioSourcePlayer>
{
    [SerializeField]
    private AudioSourcePlayer prefab;

    public override AudioSourcePlayer Create() => Instantiate(prefab);
}
