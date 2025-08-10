using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider; // optional: ui display life
    public Image damageFlashImage; // optional
    public Color flashColor = new Color(1f, 0f, 0f, 0.2f);
    public float flashSpeed = 5f;

    [Header("Invincibility Settings")]
    public float invincibleTime = 1.0f;
    private bool isInvincible = false;

    [Header("Animation")]
    private Animator animator;
    private bool isDead = false;
    private bool damaged = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;

        UpdateHealthUI();
    }

    void Update()
    {
        if (damaged && damageFlashImage != null)
        {
            damageFlashImage.color = flashColor;
        }
        else if (damageFlashImage != null)
        {
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || isInvincible) return;

        damaged = true;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibleCoroutine());
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        // 禁用控制脚本
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
            pc.enabled = false;

        // shows died
        GameManager.Instance.PlayerDied();

       
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }
}
