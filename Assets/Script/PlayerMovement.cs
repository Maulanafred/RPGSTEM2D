using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float speed = 5f;
    [SerializeField] public InputActionReference inputActions;

    public Transform animalPosition;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator animator;
    public string currentTrigger = "";


    public GameObject fireballPrefab;
    public Transform firePoint; // Posisi tembakan fireball
    public GameObject staffObject; // Untuk aktifkan saat menyerang
    public float fireballSpeed = 5f;

    private bool isAttacking = false;

     public string[] attackableEnemyTags = {"Slime", "Mushroom"}; // Contoh daftar tag musuh
      public float attackSearchRange = 10f;

    // Header sounds
    [Header("Sounds")]
    public GameObject fireballSoundSFX;
    public GameObject charAttackSoundSFX;

    




    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ambil input setiap frame
        if (!ControlModeManager.instance.isScopeMode) // hanya gerak jika bukan scope
        {
            moveInput = inputActions.action.ReadValue<Vector2>();

            // Animasi dan arah
            if (moveInput.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
                string trigger = GetTriggerFromAngle(angle);

                if (trigger != currentTrigger)
                {
                    ResetAllTriggers();
                    animator.SetTrigger(trigger);
                    currentTrigger = trigger;
                }

                // Pastikan sound walk hanya diputar sekali
                if (!AudioManager.Instance.IsSFXPlaying("Player", 0))
                {
                    AudioManager.Instance.PlaySFX("Player", 0);
                }
            }
            else
            {
                if (currentTrigger != "idle")
                {
                    ResetAllTriggers();
                    animator.SetTrigger("idle");
                    currentTrigger = "idle";
                }

                // Hentikan sound walk saat karakter berhenti
                AudioManager.Instance.StopSFX("Player", 0);
            }
        }
        else
        {
            moveInput = Vector2.zero; // reset input player saat scope
            if (currentTrigger != "idle")
            {
                ResetAllTriggers();
                animator.SetTrigger("idle");
                currentTrigger = "idle";
            }

            // Hentikan sound walk saat scope mode aktif
            AudioManager.Instance.StopSFX("Player", 0);
        }


        //Jika player menekan J atau K pada keyboard
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            PlayerAttack(); // panggil fungsi serang
        }
    }


public void PlayerAttack() // Tidak perlu parameter tag lagi di sini
{
    if (isAttacking) // Jika sedang menyerang, jangan lakukan apa-apa
    {
        return;
    }

    Transform overallNearestEnemy = null;
    float shortestDistanceFound = attackSearchRange + 1f; // Inisialisasi dengan jarak lebih jauh dari jangkauan

    // Iterasi melalui setiap tag yang bisa diserang
    foreach (string enemyTag in attackableEnemyTags)
    {
        Transform nearestInThisTag = FindNearestEnemyByTag(enemyTag, attackSearchRange);

        if (nearestInThisTag != null)
        {
            float distanceToThisEnemy = Vector2.Distance(transform.position, nearestInThisTag.position);

            // Jika musuh dari tag ini lebih dekat dari yang sudah ditemukan sebelumnya (dari tag lain)
            // dan masih dalam jangkauan serangan utama (meskipun FindNearestEnemyByTag juga sudah mengecek range)
            if (distanceToThisEnemy < shortestDistanceFound && distanceToThisEnemy <= attackSearchRange)
            {
                shortestDistanceFound = distanceToThisEnemy;
                overallNearestEnemy = nearestInThisTag;
            }
        }
    }

    // Setelah memeriksa semua tag, jika ada musuh terdekat yang ditemukan
    if (overallNearestEnemy != null)
    {
        Vector2 directionToEnemy = (overallNearestEnemy.position - transform.position).normalized;

        // Atur animasi menghadap ke arah musuh
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
        string trigger = GetTriggerFromAngle(angle);
        ResetAllTriggers();
        animator.SetTrigger(trigger);
        currentTrigger = trigger;

        StartCoroutine(Attack(directionToEnemy)); // Serang ke arah musuh terdekat
    }
    else
    {
        Debug.Log("Tidak ada musuh (dari tag yang ditentukan) dalam jarak serangan.");
        // Opsional: Anda bisa membuat player menyerang ke arah depan jika tidak ada target
        // Vector2 facingDirection = GetFacingDirectionFromCurrentTrigger(); // Anda perlu buat fungsi ini
        // StartCoroutine(Attack(facingDirection));
    }
}

    void FixedUpdate()
    {
        // Gerak pakai Rigidbody biar tidak geter
        Vector2 movement = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public string GetTriggerFromAngle(float angle)
    {
        if (angle >= 60 && angle < 120)
            return "walkup";
        else if (angle >= 120 && angle < 180)
            return "walkupleft";
        else if (angle >= -180 && angle < -120)
            return "walkdownleft";
        else if (angle >= -120 && angle < -60)
            return "walkdown";
        else if (angle >= -60 && angle < 0)
            return "walkdownright";
        else // 0 - 60
            return "walkupright";
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("walkup");
        animator.ResetTrigger("walkdown");
        animator.ResetTrigger("walkupleft");
        animator.ResetTrigger("walkdownleft");
        animator.ResetTrigger("walkupright");
        animator.ResetTrigger("walkdownright");
        animator.ResetTrigger("idle");
    }



IEnumerator Attack(Vector2 attackDir)
{
    isAttacking = true;

    if (attackDir != Vector2.zero)
    {
        GameObject attackSound = Instantiate(charAttackSoundSFX, transform.position, Quaternion.identity);
        GameObject fireballSound = Instantiate(fireballSoundSFX, transform.position, Quaternion.identity);

        Destroy(fireballSound, 2f); // Hancurkan sound effect setelah 2 detik
        Destroy(attackSound, 2f); // Hancurkan sound effect setelah 2 detik

        float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;

        // --- Posisi dan rotasi Staff ---
        float staffOffset = 1f;
        Vector3 staffPos = transform.position + (Vector3)(attackDir * staffOffset);
        staffObject.transform.position = staffPos;
        staffObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        staffObject.SetActive(true);

        // --- Flip staff secara horizontal agar tidak terbalik di kiri ---
        SpriteRenderer sr = staffObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipY = (attackDir.x < 0); // flip kalau mengarah ke kiri
        }

        // --- FirePoint Posisi dan Arah ---
        float fireOffset = 1.2f;
        firePoint.position = transform.position + (Vector3)(attackDir * fireOffset);
        firePoint.right = attackDir;

        // --- Tembak Fireball ---
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D fireballRb = fireball.GetComponent<Rigidbody2D>();
        fireballRb.velocity = attackDir * fireballSpeed;

        Destroy(fireball, 4f);
    }

    yield return new WaitForSeconds(0.5f); // cooldown
    staffObject.SetActive(false);
    isAttacking = false;
}
    
    private Transform FindNearestEnemyByTag(string enemyTag, float range)
        {
            // Validasi input tag (opsional tapi baik)
            if (string.IsNullOrEmpty(enemyTag))
            {
                Debug.LogWarning("Tag musuh tidak boleh kosong atau null.");
                return null;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); // Menggunakan parameter enemyTag
            Transform nearestEnemy = null;
            float shortestDistance = range; // Hanya akan mencari musuh yang lebih dekat dari 'range' awal

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                
                // Hanya pertimbangkan musuh jika jaraknya kurang dari shortestDistance saat ini
                // (yang awalnya adalah 'range')
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
            return nearestEnemy;
        }



}
