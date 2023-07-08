using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueEntity;
using UnityEngine.Events;
using System;
using System.Runtime.InteropServices;
using System.Linq;
using TMPro;
using UnityEngine.Assertions;
using static DialogueController;

public class DialogueViewer : MonoBehaviour
{
    [SerializeField] GameObject messageArea;
    [SerializeField] GameObject speaker;
    [SerializeField] Canvas canvas;
    //[SerializeField] Image imgMemory;
    //[SerializeField] Button btnSpeedyProgress;
    DialogueController controller;
    static Node currentNode;
    [SerializeField] Button[] responseButtons;
    private CanvasGroup canvasGroup;
    private int paragraghIndex = 0;
    private bool isOptionShowed;
    public event Action OnLeaveDialogue;
    public event NodeEnteredHandler nodeEnterHandler;

    [DllImport("__Internal")]
    private static extern void openPage(string url);

    public void Launch(TextAsset dialog)
    {
        controller = GetComponent<DialogueController>();
        controller.onEnteredNode += OnNodeEntered;

        controller.onEnteredNode += nodeEnterHandler;

        controller.onLeaveCurrentNode += onLeaveCurrentNode;
        controller.InitializeDialogue(dialog);
    }

    private void onLeaveCurrentNode(Node node)
    {
        // var npcCardTags = node.tags.Where(tag => tag.Contains("!"));

        // foreach (var npcCardTag in npcCardTags)
        // {
        //     StateMaintain.Instance.GameState.NpcCards.Add(npcCardTag);
        // }

        // if (npcCardTags.Any())
        // {
        //     GameObject.Find("NpcPromptPanel").GetComponent<AutoFade>().StartFade();
        // }
    }

    private void Awake()
    {
        InitDialogView();
    }

    private void InitDialogView()
    {
        if (messageArea == null)
        {
            messageArea = GameObject.Find("DialogMessage");
        }

        if (speaker == null)
        {
            speaker = GameObject.Find("DialogSpeaker");
        }

        var dialogCanvas = GameObject.Find("DialogCanvasEngine");
        if (canvas == null)
        {
            this.canvas = dialogCanvas.GetComponent<Canvas>();
        }
        canvasGroup = dialogCanvas.GetComponent<CanvasGroup>();

        responseButtons = new Button[3]
        {
                GameObject.Find("ResponseI").GetComponent<Button>(),
                GameObject.Find("ResponseII").GetComponent<Button>(),
                GameObject.Find("ResponseIII").GetComponent<Button>(),
        };
    }

    private void OnNodeSelected(int indexChosen)
    {
        Debug.Log("Chose: " + indexChosen);
        controller.ChooseResponse(indexChosen);
    }
    private void OnNodeEntered(Node newNode)
    {
        messageArea.GetComponent<TextMeshProUGUI>().text = string.Empty;
        speaker.GetComponent<TextMeshProUGUI>().text = string.Empty;
        currentNode = newNode;
        paragraghIndex = 0;
        isOptionShowed = false;
        canvasGroup.alpha = 1;
        SwitchOptionVisible(false);
    }

    private void ProcessAnimation(Node newNode)
    {
        var names = newNode.tags.Where(tag => tag.Contains(":") || tag.Contains("��")).ToList();

        var player = GameObject.Find("Player");

        if (names.Any())
        {

            if (string.Equals(names[0].Substring(1), "����"))
            {
                //var talkObject = GameObject.FindGameObjectWithTag("Talkable");

                //if (talkObject != null && talkObject.GetComponent<Animator>() != null)
                //{
                //    talkObject.GetComponent<Animator>().SetInteger("ActionControlEnum", (int)ActionControlEnum.Still);
                //}

                //if (player.GetComponent<PlayerMoveV2>().FaceDirection > 0)
                //{
                //    player.GetComponent<Animator>().SetInteger("ActionControlEnum", (int)ActionControlEnum.TalkRight);
                //}
                //else
                //{
                //    player.GetComponent<Animator>().SetInteger("ActionControlEnum", (int)ActionControlEnum.TalkLeft);
                //}
            }
            else
            {
                //player.GetComponent<Animator>().SetInteger("ActionControlEnum", (int)ActionControlEnum.Still);

                //var npc = GameObject.Find(names.First().Substring(1));

                //if (npc != null)
                //{
                //    var animator = npc.GetComponent<Animator>();

                //    if (animator != null)
                //    {
                //        animator.SetInteger("ActionControlEnum", (int)ActionControlEnum.TalkRight);
                //    }
                //}
            }
        }
    }
    //private void ProcessEvent(Node newNode)
    //{
    //    var tags = newNode.tags.Where(tag => tag.Contains("#"));

    //    foreach (var tag in tags)
    //    {
    //        var DialogueEvent = tag.Substring(1);
    //        var DialogueEventObject = GameObject.Find(DialogueEvent);

    //        DialogueEventObject.GetComponent<IDialogEvent>().OnEachNode(newNode);
    //    }
    //}

    // private void ProcessCard(Node newNode)
    // {
    //     var tags = newNode.tags.Where(tag => tag.Contains("*"));

    //     foreach (var tag in tags)
    //     {
    //         CardUtilities.AddNewCard(tag.Substring(1));
    //         CardUtilities.ShowNewCard(tag.Substring(1));
    //     }
    // }

    private void HandleOptions(Node newNode)
    {
        if (newNode.responses.Count == 1)
        {
            if (newNode.tags.Contains(DialogConstants.SINGLECHOICE))
            {
                ShowOption(newNode);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnNodeSelected(DialogConstants.DefaultOptionIndex);
                }
            }
        }
        else if (newNode.responses.Count > 1)
        {
            ShowOption(newNode);
        }
        else
        {
            throw new Exception("This node is last node and should be ended");
        }
    }

    private void Update()
    {
        if (canvasGroup.alpha == 1 && !isOptionShowed)
        {
            if (!IsFinalParagraghReached())
            {
                if ((paragraghIndex == 0 || Input.GetKeyDown(KeyCode.Space)))
                {
                    messageArea.GetComponent<TextMeshProUGUI>().text = currentNode.paragraphs[paragraghIndex++];
                    speaker.GetComponent<TextMeshProUGUI>().text = currentNode.RoleName;
                }
            }
            else if (currentNode.responses.Count > 0)
            {
                HandleOptions(currentNode);
            }
            else
            {
                if (currentNode.IsEndNode() && Input.GetKeyDown(KeyCode.Space))
                {
                    canvasGroup.alpha = 0;
                    SwitchOptionVisible(false);

                    if (OnLeaveDialogue != null)
                    {
                        OnLeaveDialogue.Invoke();
                        foreach (Delegate delegation in OnLeaveDialogue?.GetInvocationList())
                        {
                            OnLeaveDialogue -= (Action)delegation;
                        }
                    }
                }
            }
        }
    }

    private bool IsStartParagragh()
    {
        return paragraghIndex == 0;
    }

    private void SwitchOptionVisible(bool isVisible)
    {
        foreach (var responseButton in responseButtons)
        {
            if (isVisible)
            {
                isOptionShowed = true;
                responseButton.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                responseButton.interactable = true;
            }
            else
            {
                isOptionShowed = false;
                responseButton.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                responseButton.interactable = false;
            }
        }
    }

    private bool IsFinalParagraghReached()
    {
        return paragraghIndex >= currentNode.paragraphs.Count();
    }

    private void ShowOption(Node newNode)
    {
        isOptionShowed = true;
        for (int i = 0; i < newNode.responses.Count; i++)
        {
            var response = newNode.responses[i];
            var responseButton = responseButtons[i];
            responseButton.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            responseButton.GetComponentsInChildren<TextMeshProUGUI>()[0].text = response.displayText;
            responseButton.gameObject.GetComponent<CanvasGroup>().interactable = true;
            responseButton.interactable = true;

            int optionIndex = i;

            responseButton.onClick.RemoveAllListeners();
            responseButton.onClick.AddListener(delegate
            {
                isOptionShowed = false;
                OnNodeSelected(optionIndex);
            });
        }
    }


    private IEnumerator ProcessParagraghAndNode()
    {
        if (canvasGroup.alpha == 1)
        {
            for (int i = 0; i < currentNode.paragraphs.Count(); i++)
            {
                messageArea.GetComponent<TextMeshProUGUI>().text = currentNode.paragraphs[i];

                if (i + 1 == currentNode.paragraphs.Count())
                {
                    HandleOptions(currentNode);
                }
                else
                {
                    while (true)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            break;
                        }
                    }
                }
            }
        }

        yield return null;
    }

    public static Sprite Texture2DToSprite(Texture2D t)
    {
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
    }

    private void ShowContinueButton(UnityAction onContinuePressed)
    {
        //var responceButton = Instantiate(prefab_btnResponse, parentOfResponses);
        //responceButton.GetComponentInChildren<SlowTyper>().Begin("continue");
        //responceButton.onClick.AddListener(delegate
        //{
        //    imgMemory.GetComponent<Oscillate>().SetValue(Oscillate.ValueSet.Max);
        //    imgMemory.GetComponent<Oscillate>().SetDirection(false);
        //    KillAllChildren(parentOfResponses);
        //    onContinuePressed();
        //});
    }
}