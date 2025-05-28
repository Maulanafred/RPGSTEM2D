using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("--------------- Sounds ---------------")]

    public GameObject playerHitSFXPrefab; // Suara saat naik level
    public static PlayerStats instance;

    public UIManagementGame uiManagementGame;

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



    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Update bar dan level
        healthBar.fillAmount = currentHealth / maxHealth;
        expBar.fillAmount = currentExp / expToNextLevel;
        levelText.text = currentLevel.ToString();
    }

    public void TakeDamage(float damage)
    {
        GameObject playerHitSFX = Instantiate(playerHitSFXPrefab, transform.position, Quaternion.identity);

        Destroy(playerHitSFX, 2f); // Hancurkan prefab setelah 1 detik

        // Memerahkan sprite player sementara
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Ubah warna sprite menjadi merah
            Invoke("ResetSpriteColor", 0.1f); // Kembalikan warna setelah 0.1 detik
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0 && uiManagementGame.isGameOver == false)

        {
            currentHealth = 0; // Pastikan health tidak negatif
            uiManagementGame.ShowGameOverPanel();


        }
    }

    public void AddExp(float expGained)
    {
        currentExp += expGained;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        currentLevel++;
        expToNextLevel = 100f + (currentLevel - 1) * 20f; // Naik 50 tiap level
        Debug.Log("Naik level! Sekarang level: " + currentLevel);
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
