using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static DialogueEntity;

public class OneGoDialogTrigger : BaseDialogTrigger
{
    private bool Triggered;

    private void Start()
    {
        DialogEvent = this.GetComponent<IDialogEvent>();
        TriggerDialog(Dialog);
    }

    public virtual void TriggerDialog(TextAsset dialog)
    {
        StartCoroutine(StartTalkWrap(dialog));
    }

    private IEnumerator StartTalkWrap(TextAsset dialog)
    {
        StartTalk(dialog);

        yield return new WaitForSeconds(0.1F);
    }

    protected void StartTalk(TextAsset dialog)
    {
            var dialogEngine = GameObject.Find("DialogCanvasEngine");
            var dialogueViewer = dialogEngine.GetComponent<DialogueViewer>();

            if (dialogueViewer != null)
            {
                StartTalk(dialogueViewer, dialog);
            }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.GetComponent<SceneLoadDialogEvent>().End();
        }
    }
}
