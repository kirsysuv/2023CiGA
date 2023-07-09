using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    public void OnPointerEnter()
    {
        this.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnPointerExit()
    {
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
