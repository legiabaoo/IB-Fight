using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRightHand : MonoBehaviour
{
    public static PlayerRightHand instance;
    public GameManager gameManager;
    public List<Animator> aniEnermies;
    private bool hasHit = false;
    public bool isHead = false;
    public bool isBody = false;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        //if (other.CompareTag("HeadE"))
        //{
        //    Debug.Log("Trung dau E");
        //    aniEnermy.SetTrigger("isHHit");
        //    hasHit = true;
        //    StartCoroutine(ResetHitCooldown());
        //}
        //else if (other.CompareTag("BodyE"))
        //{
        //    Debug.Log("Trung nguoi E");
        //    aniEnermy.SetTrigger("isSHit");
        //    hasHit = true;
        //    StartCoroutine(ResetHitCooldown());
        //}
        if (other.CompareTag("Enemy"))
        {
            if (gameManager.currentEnemy == EnemySelect.enemy1)
            {
                if (isBody)
                {
                    aniEnermies[0].SetTrigger("isSHit");
                    isBody = false;
                }
                if (isHead)
                {
                    aniEnermies[0].SetTrigger("isHHit");
                    isHead = false;
                }
                hasHit = true;
                StartCoroutine(ResetHitCooldown());
            }
            else
            {
                if (isBody)
                {
                    aniEnermies[1].SetTrigger("isSHit");
                    isBody = false;
                }
                if (isHead)
                {
                    aniEnermies[1].SetTrigger("isHHit");
                    isHead = false;
                }
                hasHit = true;
                StartCoroutine(ResetHitCooldown());
            }
            //Debug.Log("trung");

        }
    }
    private IEnumerator ResetHitCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        hasHit = false;
    }
}
