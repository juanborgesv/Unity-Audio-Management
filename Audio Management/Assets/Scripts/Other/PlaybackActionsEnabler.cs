using UnityEngine;

public class PlaybackActionsEnabler : MonoBehaviour
{
    [SerializeField] 
    private GroupAudioActionRequester actionsRequester;

    [SerializeField]
    private GameObject actionsControlButtonsRoot;

    private void Update()
    {
        // Enables the control bar (buttons) as long as there is an ongoing
        // request of playing audio. 
        actionsControlButtonsRoot.SetActive(actionsRequester.RequestId != -1);
    }
}
