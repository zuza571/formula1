using System.Collections;
using UnityEngine;
public class Dice : MonoBehaviour
{
    private Sprite[] diceSlides;
    private new SpriteRenderer renderer;
    private bool coroutineAllowed = true;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        diceSlides = Resources.LoadAll<Sprite>("Dice/dice");
        renderer.sprite = diceSlides[5];
    }

    private void OnMouseDown()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(RollTheDice());
        }
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSlide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSlide = Random.Range(0, 6);
            renderer.sprite = diceSlides[randomDiceSlide];
            yield return new WaitForSeconds(0.1f);
        }

        /*
        GameMaster.diceSideThrown = randomDiceSlide + 1;
        if (whosTurn == 1)
        {
            GameMaster.MovePlayer(1);
        }
        else if (whosTurn == -1)
        {
            GameMaster.MovePlayer(2);
        }

        whosTurn *= -1;
        */
        
        coroutineAllowed = true;
    }
}
