using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PauseUICtl : MonoBehaviour
{
    public Button ContinueBtn;
    public Button ReturnBtn;
    public Button ExitBtn;
    public GameObject root;
    public GameObject image;



    void Start()
    {
        ContinueBtn.onClick.AddListener(Continue);
        ReturnBtn.onClick.AddListener(Return);
        ExitBtn.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Return()
    {
        Debug.Log("返回标题");
        root.SetActive(false);
        image.SetActive(false);
        Time.timeScale = 1;
        SceneTransition.Instance.StartFadeIn(Const.InitScene);
    }

    private void Continue()
    {
        Debug.Log("继续游戏");
        root.SetActive(false);
        image.SetActive(false);
        Time.timeScale = 1;
    }



    // Update is called once per frame
    void Update()
    {
        // 暂停按键
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("暂停");
            Time.timeScale = 0;
            if (root != null)
            {
                root.SetActive(true);
            }
            if (image != null)
            {
                image.SetActive(true);
            }
        }
    }
}
