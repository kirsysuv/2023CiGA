using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueEntity;

public class DialogueController : MonoBehaviour
{
    Dialogue curDialogue;
    Node curNode;

    public delegate void NodeEnteredHandler(Node node);
    public event NodeEnteredHandler onEnteredNode;
    public event NodeEnteredHandler onLeaveCurrentNode;

    public Node GetCurrentNode()
    {
        return curNode;
    }

    public void InitializeDialogue(TextAsset dialog)
    {
        curDialogue = new Dialogue(dialog);
        curNode = curDialogue.GetStartNode();
        onEnteredNode(curNode);
    }

    public List<Response> GetCurrentResponses()
    {
        return curNode.responses;
    }

    public void ChooseResponse(int responseIndex)
    {
        onLeaveCurrentNode?.Invoke(curNode);
        string nextNodeID = curNode.responses[responseIndex].destinationNode;
        Node nextNode = curDialogue.GetNode(nextNodeID);
        curNode = nextNode;
        onEnteredNode(nextNode);
    }
}