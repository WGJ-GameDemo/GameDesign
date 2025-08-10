using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 300; // higer health
    private int currentHealth;

    public GameObject keyPrefab; // drop the key when boss die
    public Transform dropPoint;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // drop the key
        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, dropPoint.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
