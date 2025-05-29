using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("---------------Instance ---------------")]
    public UIManagementGame uiManagementGame;
    public QuestionLevel questionLevel; // Pastikan ini terhubung di Inspector


    [Header("--------------- Sounds ---------------")]
    public GameObject playerHitSFXPrefab; // Komentar asli: Suara saat naik level
    public static PlayerStats instance;


    [Header("Nyawa")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBar;

    [Header("EXP dan Level")]
    public int currentLevel = 1;
    public float currentExp = 0f;
    public float expToNextLevel = 100f;
    public Image expBar;
    public TMP_Text levelText;

    public TMP_Text expText; // Pastikan ini terhubung di Inspector
    public TMP_Text healthText; // Pastikan ini terhubung di Inspector


    void Awake()
    {
        // Singleton pattern sederhana
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Pastikan referensi penting tidak null
        if (uiManagementGame == null)
        {
            Debug.LogError("UIManagementGame belum di-assign di PlayerStats!");
        }
        if (questionLevel == null)
        {
            // Anda bisa mencari QuestionLevel secara otomatis jika belum di-assign
            // questionLevel = FindObjectOfType<QuestionLevel>();
            Debug.LogWarning("QuestionLevel belum di-assign di PlayerStats. Fitur pertanyaan mungkin tidak berfungsi.");
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        // Hitung expToNextLevel berdasarkan level awal
        CalculateExpToNextLevel(); // Panggil fungsi baru ini
        UpdateUI();
    }

    void Update()
    {
        // Setiap 10 detik akan regenerasi 1 nyawa
        if (currentHealth < maxHealth)
        {
            currentHealth += 0.1f * Time.deltaTime; // Regenerasi 0.1 nyawa per detik
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Pastikan tidak melebihi maxHealth
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        // Update bar dan level
        if (healthText != null)
        {
            // "F0" berarti format float dengan 0 angka di belakang koma (bilangan bulat)
            healthText.text = $"{currentHealth.ToString("F0")}/{maxHealth.ToString("F0")}";
        }

        if (expText != null)
        {
            // Anda juga bisa menggunakan "F0" untuk EXP jika ingin bilangan bulat
            expText.text = $"{currentExp.ToString("F0")}/{expToNextLevel.ToString("F0")}";
        }

        if (healthBar != null)
            healthBar.fillAmount = currentHealth / maxHealth;

        if (expBar != null)
            expBar.fillAmount = currentExp / expToNextLevel;

        if (levelText != null)
            levelText.text = currentLevel.ToString();
    }

    public void TakeDamage(float damage)
    {
        if (playerHitSFXPrefab != null)
        {
            GameObject playerHitSFX = Instantiate(playerHitSFXPrefab, transform.position, Quaternion.identity);
            Destroy(playerHitSFX, 2f);
        }
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            Invoke("ResetSpriteColor", 0.1f);
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateUI();

        if (currentHealth <= 0f && uiManagementGame != null && !uiManagementGame.isGameOver)
        {
            currentHealth = 0f; // Pastikan health tidak negatif
            uiManagementGame.ShowGameOverPanel();
        }
    }

    public void AddExp(float expGained)
    {
        currentExp += expGained;

        bool hasLeveledUp = false; // Flag untuk menandai apakah terjadi level up
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUpLogic(); // Panggil logika inti level up
            hasLeveledUp = true;
        }

        if (hasLeveledUp && questionLevel != null)
        {
            questionLevel.ShowQuestion(); // Tampilkan pertanyaan SETELAH semua proses level up selesai
        }
        else if (hasLeveledUp && questionLevel == null)
        {
             Debug.LogWarning("Naik level, tetapi QuestionLevel tidak terhubung. Pertanyaan tidak ditampilkan.");
        }

        UpdateUI();
    }

    // Memisahkan logika inti dari LevelUp untuk dipanggil dalam loop
    void LevelUpLogic()
    {
        currentLevel++;
        CalculateExpToNextLevel(); // Hitung ulang EXP yang dibutuhkan untuk level berikutnya
        Debug.Log("Naik level! Sekarang level: " + currentLevel);
        // Anda bisa menambahkan efek lain saat naik level di sini (misalnya suara, partikel)
        // Mungkin juga pemulihan HP penuh?
        // currentHealth = maxHealth;
    }

    // Fungsi untuk menghitung EXP ke level berikutnya
    void CalculateExpToNextLevel()
    {
        // Contoh formula: 100 XP untuk level 2, lalu +20 XP untuk setiap kebutuhan level berikutnya
        // Level 1 -> 2: 100 XP
        // Level 2 -> 3: 120 XP
        // Level 3 -> 4: 140 XP
        // dst.
        if (currentLevel == 1) {
             expToNextLevel = 100f;
        } else {
            // Pastikan currentLevel - 1 tidak negatif jika Anda memulai dari level 0 atau punya logika berbeda
            expToNextLevel = 100f + (currentLevel - 1) * 20f;
        }
    }
    
    // Fungsi yang bisa dipanggil oleh QuestionLevel setelah pertanyaan dijawab
    // (Mirip dengan yang kita diskusikan sebelumnya, jika QuestionLevel perlu memberi tahu PlayerStats)
    public void ProcessQuestionOutcome(bool answeredCorrectly)
    {
        if(answeredCorrectly)
        {
            Debug.Log("Jawaban benar! Memberikan hadiah (misalnya pulih HP).");
            HealPlayer(4f); // Contoh: pulihkan 20 HP
        }
        else
        {
            Debug.Log("Jawaban salah.");
        }
        UpdateUI(); // Selalu update UI setelah ada perubahan
    }

    public void HealPlayer(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateUI();
        Debug.Log($"Player dipulihkan {amount} HP. HP sekarang: {currentHealth.ToString("F0")}");
    }

    void ResetSpriteColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Kembalikan warna sprite ke default (putih)
        }
    }
}