using UnityEngine;

public class RulesPanel : MonoBehaviour
{
    public GameObject Rules;
    private static bool isRules;

    void Start()
    {
        Rules.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isRules)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Rules.SetActive(true);
        Time.timeScale = 0f;
        isRules = true;
    }

    public void ResumeGame()
    {
        Rules.SetActive(false);
        Time.timeScale = 1f;
        isRules = false;
    }
    
}

