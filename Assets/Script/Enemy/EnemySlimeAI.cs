using UnityEngine;

public class EnemySlimeAI : MonoBehaviour
{
    public GameObject deadSFXPrefab; // Prefab untuk efek suara saat musuh mati
    public static EnemySlimeAI instance;
    [Header("Enemy Slime AI Settings")]
    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    public int maxHealth;

    private Transform player;
    private float lastAttackTime;
    private Rigidbody2D rb;

    public int exp;

    public bool isBlocked = false; // Flag untuk menghentikan pergerakan sementara

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

            if (player == null)
    {
        Debug.LogError("Player tidak ditemukan! Pastikan objek Player memiliki tag 'Player'.");
    }
    }

    void Update()
    {
        if (player == null) return;

        if (isBlocked) // Jika musuh sedang diblokir, hentikan pergerakan
        {
            rb.velocity = Vector2.zero; // Hentikan pergeraka
            return;
        }

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

        if (maxHealth <= 0)
        {
            GameObject deadSFX = Instantiate(deadSFXPrefab, transform.position, Quaternion.identity);

   
            Destroy(deadSFX, 4f); // Hancurkan efek suara setelah 2 detik
            Debug.Log("Enemy mati");
            Destroy(gameObject); // Hancurkan enemy jika health habis
            PlayerStats.instance.AddExp(exp); // Tambah EXP ke player
        }
    }

    void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            lastAttackTime = Time.time;

            // Panggil fungsi damage ke player
            if (PlayerStats.instance != null)
            {
                PlayerStats.instance.TakeDamage(damage);
            }

            Debug.Log("Enemy menyerang!");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visual jarak
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
