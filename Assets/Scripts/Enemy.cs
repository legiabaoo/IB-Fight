using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    private Animator animator;
    public Animator aniPlayer;
    public int dameEnemy = 10;
    public GameManager gameManager;
    void Start()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator Attack(Action onComplete)
    {
        int rand = UnityEngine.Random.Range(1, 3);
        string trigger = "attack" + rand;
        animator.SetTrigger(trigger);

        if (rand == 1 || rand == 3)
        {
            yield return new WaitForSeconds(0.65f);
            if (gameManager.currentPlayer == PlayerSelect.player1)
            {
                gameManager.players[0].GetComponent<Animator>().SetTrigger("isHHit");
            }
            else
            {
                gameManager.players[1].GetComponent<Animator>().SetTrigger("isHHit");
            }

        }
        else
        {
            yield return new WaitForSeconds(0.8f);
            if (gameManager.currentPlayer == PlayerSelect.player1)
            {
                gameManager.players[0].GetComponent<Animator>().SetTrigger("isSHit");
            }
            else
            {
                gameManager.players[1].GetComponent<Animator>().SetTrigger("isSHit");
            }
        }
        if (gameManager.currentPlayer == PlayerSelect.player1)
        {
            gameManager.players[0].GetComponent<PlayerHealth>().currentHealth -= dameEnemy;
            gameManager.players[0].GetComponent<PlayerHealth>().UpdateHealth();
        }
        else
        {
            gameManager.players[1].GetComponent<PlayerHealth>().currentHealth -= dameEnemy;
            gameManager.players[1].GetComponent<PlayerHealth>().UpdateHealth();
        }
        yield return new WaitForSeconds(1.0f);
        onComplete?.Invoke();
    }
}
