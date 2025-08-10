using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public enum EnemyType { RegularMonster, FlyingMonster }
    public EnemyType enemyType = EnemyType.RegularMonster;

    [Header("Basic Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public int health = 3;

    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Drop Items")]
    public GameObject[] dropItems; 

    [Header("Damage Flash")]
    public SpriteRenderer spriteRenderer;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);
    public float flashDuration = 0.1f;

    private Vector3 targetPoint;
    private Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;
    private float lastAttackTime = 0f;

    private bool isDead = false;
    private bool isFlashing = false;

    private enum State { Idle, Patrol, Chase, Attack }
    private State currentState = State.Idle;

    void Start()
    {
        if (pointA != null) targetPoint = pointA.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        currentState = State.Patrol;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: ChasePlayer(); break;
            case State.Attack: AttackPlayer(); break;
        }

        UpdateAnimation();
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        if (enemyType == EnemyType.RegularMonster)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(targetPoint.x, transform.position.y),
                patrolSpeed * Time.deltaTime
            );
        }
        else if (enemyType == EnemyType.FlyingMonster)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint,
                patrolSpeed * Time.deltaTime
            );
        }

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            targetPoint = (targetPoint == pointA.position) ? pointB.position : pointA.position;
            Flip();
        }
    }

    void ChasePlayer()
    {
        if (enemyType == EnemyType.RegularMonster)
        {
            Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPos,
                chaseSpeed * Time.deltaTime
            );
        }
        else if (enemyType == EnemyType.FlyingMonster)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );
        }

        if ((player.position.x > transform.position.x && transform.localScale.x < 0) ||
            (player.position.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    void AttackPlayer()
    {
        if (playerHealth == null) return;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log(name + " attacks player and deals " + damage + " damage");

            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (animator != null)
            animator.SetTrigger("Hurt");

        StartCoroutine(FlashDamage());

        if (health <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashDamage()
    {
        if (spriteRenderer == null) yield break;

        isFlashing = true;
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
        isFlashing = false;
    }

    void Die()
    {
        isDead = true;
        Debug.Log(name + " died");

        if (animator != null)
            animator.SetTrigger("Die");

        // stop movement
        this.enabled = false;

        // 掉落物品
        DropItem();

        // 死亡动画播放后销毁
        Destroy(gameObject, 2f);
    }

    void DropItem()
    {
        if (dropItems == null || dropItems.Length == 0) return;

        // random drop items
        int index = Random.Range(0, dropItems.Length);
        GameObject item = Instantiate(dropItems[index], transform.position, Quaternion.identity);
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("isPatrolling", currentState == State.Patrol);
        animator.SetBool("isChasing", currentState == State.Chase);
        animator.SetBool("isAttacking", currentState == State.Attack);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (pointA != null && pointB != null)
        {
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
