using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionSelect : MonoBehaviour
{
    public List<Button> Options = new List<Button>();
    public int index { set; get; } = 0;

    void Start()
    {
        Options = new List<Button>()
        {
                GameObject.Find("ResponseI").GetComponent<Button>(),
                GameObject.Find("ResponseII").GetComponent<Button>(),
                GameObject.Find("ResponseIII").GetComponent<Button>(),
        };
    }

    void Update()
    {
        var direction = 0;

        var responseI = GameObject.Find("ResponseI");

        if (responseI.GetComponent<CanvasGroup>().alpha > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                direction = -1;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                direction = 1;
            }
        }

        if (direction != 0)
        {
            if (index == -1)
            {
                index = 0;
                EventSystem.current.SetSelectedGameObject(Options[index].gameObject);
                return;
            }

            if (direction > 0)
            {
                index = (++index) % Options.Count;
                EventSystem.current.SetSelectedGameObject(Options[index].gameObject);
                return;
            }

            if (direction < 0)
            {
                if ((--index) < 0)
                {
                    index = Options.Count - 1;
                }
                EventSystem.current.SetSelectedGameObject(Options[index].gameObject);

                return;
            }
        }
    }
}
