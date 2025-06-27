using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public static Player instance;
    private Animator aniPlayer;
    public GameObject rightHand;
    public GameObject leftHand;
    PlayerRightHand playerRightHand;
    PlayerLeftHand playerLeftHand;
    public int dameHeadPunch = 15;
    public int dameKidneyPunch = 40;
    public int dameStomachPunch = 10;
    void Start()
    {
        instance = this;
        aniPlayer = GetComponent<Animator>();
        playerRightHand = rightHand.GetComponent<PlayerRightHand>();
        playerLeftHand = leftHand.GetComponent<PlayerLeftHand>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HeadPunch(Action onComplete)
    {
        playerRightHand.isHead = true;
        aniPlayer.SetTrigger("isHP");
        if(gameManager.currentEnemy == EnemySelect.enemy1)
        {
            gameManager.enemies[0].GetComponent<EnemyHealth>().currentHealth -= dameHeadPunch;
            gameManager.enemies[0].GetComponent<EnemyHealth>().UpdateHealth();
        }
        else
        {
            gameManager.enemies[1].GetComponent<EnemyHealth>().currentHealth -= dameHeadPunch;
            gameManager.enemies[1].GetComponent<EnemyHealth>().UpdateHealth();

        }
        StartCoroutine(WaitForAnimation(onComplete));
    }
    public void StomachPunch(Action onComplete)
    {
        playerRightHand.isBody = true;
        aniPlayer.SetTrigger("isSP");
        if (gameManager.currentEnemy == EnemySelect.enemy1)
        {
            gameManager.enemies[0].GetComponent<EnemyHealth>().currentHealth -= dameHeadPunch;
            gameManager.enemies[0].GetComponent<EnemyHealth>().UpdateHealth();
        }
        else
        {
            gameManager.enemies[1].GetComponent<EnemyHealth>().currentHealth -= dameHeadPunch;
            gameManager.enemies[1].GetComponent<EnemyHealth>().UpdateHealth();

        }
        StartCoroutine(WaitForAnimation(onComplete));
    }
    public void KidneyPunch(Action onComplete)
    {
        playerLeftHand.isHead = true;
        aniPlayer.SetTrigger("isKPLeft");
        if (gameManager.currentEnemy == EnemySelect.enemy1)
        {
            gameManager.enemies[0].GetComponent<EnemyHealth>().currentHealth -= dameKidneyPunch;
            gameManager.enemies[0].GetComponent<EnemyHealth>().UpdateHealth();
        }
        else
        {
            gameManager.enemies[1].GetComponent<EnemyHealth>().currentHealth -= dameKidneyPunch;
            gameManager.enemies[1].GetComponent<EnemyHealth>().UpdateHealth();

        }
        StartCoroutine(WaitForAnimation(onComplete));
    }
    private IEnumerator WaitForAnimation(Action onComplete)
    {
        yield return new WaitForSeconds(3.0f); // thời gian animation
        onComplete?.Invoke();
    }
}
