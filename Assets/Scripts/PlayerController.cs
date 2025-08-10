using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxHealth = 3;
    public int currentHealth;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
        Interact();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // A/D or arrow keys
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void Attack()
    {
        
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Player attacks with dagger");
            
            // TODO: 攻击动画
            // animator.SetTrigger("Attack");

            // TODO: 伤害判定（检测前方敌人）
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f, LayerMask.GetMask("Enemy"));
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                // 让敌人受伤
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }
    }


    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player interacts");
            // TODO: iteraction: open the door or talk to npc
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WeaponPickup"))
        {
            string weaponName = other.GetComponent<ItemPickup>().itemName;
            GetComponent<PlayerInventory>().AddWeapon(weaponName);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ArmorPickup"))
        {
            string armorName = other.GetComponent<ItemPickup>().itemName;
            GetComponent<PlayerInventory>().AddArmor(armorName);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("KeyPickup"))
        {
            GetComponent<PlayerInventory>().AddKey(1);
            Destroy(other.gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage, current health: " + currentHealth);

        // could trigger wounded image

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // died screen
        // 禁用玩家控制
        this.enabled = false;
        // call GameManager，trigger died ui
    }

}
