using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseDialogTrigger : MonoBehaviour, IDialogTrigger
{
    [SerializeField] public TextAsset Dialog;

    public IDialogEvent DialogEvent { get; set; }
    public string DialogName { get; set; }

    public bool Enabled
    {
        get
        {
            return this.enabled;
        }

        set
        {
            enabled = value;
        }
    }

    protected void StartTalk(DialogueViewer dialogueViewer, TextAsset dialog)
    {
        if (DialogEvent != null)
        {
            dialogueViewer.nodeEnterHandler += DialogEvent.OnEachNode;
            dialogueViewer.OnLeaveDialogue += DialogEvent.End;
        }

        dialogueViewer.Launch(dialog);
    }
}
