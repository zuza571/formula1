using UnityEngine;
using UnityEngine.UI;

public class SkipTurnButtonScript : MonoBehaviour
{
    public Button button;
    private bool resetPoints = false; 

    void Start()
    {
        button.onClick.AddListener(OnClick);
        button.gameObject.SetActive(false);
    }

    void OnClick()
    {
        resetPoints = true;
    }

    public bool ShouldResetPoints()
    {
        if (resetPoints)
        {
            resetPoints = false;
            return true;
        }
        return false;
    }
}
