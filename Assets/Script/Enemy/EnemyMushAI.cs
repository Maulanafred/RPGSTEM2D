using UnityEngine;

public class EnemyMushAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 1;

    public int maxHealth;
    public Animator animator;

    private Transform player;
    private float lastAttackTime;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Tambahkan SpriteRenderer
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            if (distance > attackRange)
            {
                MoveToPlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("attack", false);
        }

        if (maxHealth <= 0)
        {
            Debug.Log("Enemy mati");
            Destroy(gameObject);
            PlayerStats.instance.AddExp(50);
        }
    }

    void MoveToPlayer()
    {
        animator.SetBool("run", true);
        animator.SetBool("attack", false);

        Vector2 direction = (player.position - transform.position).normalized;

        // ➤ Flip kiri-kanan berdasarkan arah X
        if (direction.x < 0)
        {
            spriteRenderer.flipX = false;  // menghadap kiri
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = true; // menghadap kanan
        }

        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.SetBool("attack", true);
            animator.SetBool("run", false);
            lastAttackTime = Time.time;

            if (PlayerStats.instance != null)
            {
                PlayerStats.instance.TakeDamage(damage);
            }

            Debug.Log("Enemy menyerang!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


        public void TakeDamage(int damage)
        {
            maxHealth -= damage;
            maxHealth = Mathf.Clamp(maxHealth, 0, maxHealth);
            Debug.Log("Enemy menerima damage: " + damage + ", sisa health: " + maxHealth);
        }
}
