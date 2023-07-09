using UnityEngine;
using static DialogueEntity;

public class SceneLoadDialogEvent : MonoBehaviour, IDialogEvent
{
    public string SceneName;

    public void OnEachNode(Node node)
    {
        GameObject.Find("PlayerImage").GetComponent<CanvasGroup>().alpha = 
            string.Equals(node.RoleName, "Player") ?
            1 : 0;
    }

    public void End()
    {
        SceneTransition.Instance.StartFadeIn(SceneName);
    }
}
