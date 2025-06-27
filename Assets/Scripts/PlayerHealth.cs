using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    private int maxHealth = 100;
    private int maxStamina = 50;
    private int currentStamina;
    public int currentHealth;
    public MicroBar healthBar;
    public MicroBar staminaBar;
    public TMP_Text txtHealth;
    public TMP_Text txtStamina;
    public TMP_Text txtName;

    private Animator animator;

    void Start()
    {
        Instance = this;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        healthBar.Initialize(currentHealth);
        staminaBar.Initialize(currentStamina);
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
    public void IsWin()
    {
        animator.SetTrigger("isVictory");
        Invoke("DelayHideName", 1f);
    }
    public void DelayHideName()
    {
        txtName.enabled = false;
    }
    public void UpdateHealth()
    {
        healthBar.UpdateBar(currentHealth);
        txtHealth.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        staminaBar.UpdateBar(currentStamina);
        txtStamina.text = currentStamina.ToString() + "/" + maxStamina.ToString();
    }
}
