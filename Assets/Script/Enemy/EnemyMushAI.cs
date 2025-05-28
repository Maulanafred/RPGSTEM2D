using UnityEngine;
using System.Collections;

public class EnemyMushAI : MonoBehaviour
{
    public GameObject deadSFXPrefab; // Prefab efek suara saat musuh mati
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

    private bool isAttacking = false; // Flag untuk memastikan musuh tidak terus berganti animasi

    public bool isBlocked = false; // Flag untuk menghentikan pergerakan sementara

    public int exp ; // Jumlah exp yang diberikan saat mati
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Tambahkan SpriteRenderer
    }



void Update()
{
    if (player == null) return;

    if (isBlocked) // Jika musuh sedang diblokir, hentikan pergerakan
    {
        rb.velocity = Vector2.zero; // Hentikan pergerakan
        animator.SetBool("run", false);
        return;
    }

    float distance = Vector2.Distance(transform.position, player.position);

    if (distance <= detectRange)
    {
        if (distance > attackRange)
        {
            if (!isAttacking) // Hanya bergerak jika tidak sedang menyerang
            {
                MoveToPlayer();
            }
        }
        else
        {
            if (!isAttacking) // Mulai menyerang jika tidak sedang menyerang
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }
    else
    {
        if (!isAttacking) // Hanya set animasi idle jika tidak sedang menyerang
        {
            rb.velocity = Vector2.zero; // Hentikan pergerakan musuh
            animator.SetBool("run", false);
            animator.SetBool("attack", false);
        }
    }

    if (maxHealth <= 0)
    {
        GameObject deadSFX = Instantiate(deadSFXPrefab, transform.position, Quaternion.identity);

   
        Destroy(deadSFX, 4f); // Hancurkan efek suara setelah 2 detik
        Debug.Log("Enemy mati");
        Destroy(gameObject);
        PlayerStats.instance.AddExp(exp);
    }
}
    void MoveToPlayer()
    {
        if (isAttacking) return; // Jangan bergerak jika sedang menyerang

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

        // Gunakan velocity untuk pergerakan
        rb.velocity = direction * moveSpeed;
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true; // Set flag menyerang
        rb.velocity = Vector2.zero; // Hentikan pergerakan musuh
        animator.SetBool("attack", true);
        animator.SetBool("run", false);

        yield return new WaitForSeconds(attackCooldown); // Tunggu cooldown serangan

        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.TakeDamage(damage);
        }

        Debug.Log("Enemy menyerang!");
        isAttacking = false; // Reset flag menyerang
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
