using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableCrate : MonoBehaviour
{
    public float pushForce = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 pushDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }
    }
}

