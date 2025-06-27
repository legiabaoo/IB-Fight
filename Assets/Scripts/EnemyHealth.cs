using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth instance;
    public int maxHealth=100;
    public int currentHealth;
    public MicroBar healthBar;
    private Animator animator;
    public TMP_Text txtHealth;
    public TMP_Text txtName;
    void Start()
    {
        instance = this;
        //currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            txtHealth.text = currentHealth.ToString() + "/" + maxHealth.ToString();
            animator.SetTrigger("isKO");
            Invoke("DelayHideName", 3f);
        }
    }
    public void DelayHideName()
    {
        txtName.enabled = false;
    }
    public void UpdateHealth()
    {
        healthBar.UpdateBar(currentHealth);
        txtHealth.text = currentHealth.ToString()+ "/"+ maxHealth.ToString(); 
    }
    public void Sync()
    {
        currentHealth = maxHealth;
        healthBar.Initialize(currentHealth);
        txtHealth.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void IsWin()
    {
        animator.SetTrigger("isVictory");
        Invoke("DelayHideName", 1f);
    }
}
