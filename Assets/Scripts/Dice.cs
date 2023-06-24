using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSlides;
    private new SpriteRenderer renderer;
    private bool coroutineAllowed = true;
    private int randomDiceSlide;
    private bool isCoroutineRunning;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        diceSlides = Resources.LoadAll<Sprite>("Dice/dice");
        renderer.sprite = diceSlides[5];
    }
    public IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        isCoroutineRunning = true;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSlide = Random.Range(0, 6);
            renderer.sprite = diceSlides[randomDiceSlide];
            image.sprite = diceSlides[randomDiceSlide];
            yield return new WaitForSeconds(0.2f);
        }

        randomDiceSlide += 1;
        isCoroutineRunning = false;
        coroutineAllowed = true;
    }

    public int RandomDiceSlide
    {
        get
        {
            if (isCoroutineRunning)
            {
                // Coroutine RollTheDice() is still running, return 0 as an error code
                return 0;
            }
            else
            {
                // Coroutine RollTheDice() has finished, return the final value of randomDiceSlide
                return randomDiceSlide;
            }
        }
    }
}