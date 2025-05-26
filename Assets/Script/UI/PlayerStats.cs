using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    void UpdateUI()
    {
        // Update bar dan level
        healthBar.fillAmount = currentHealth / maxHealth;
        expBar.fillAmount = currentExp / expToNextLevel;
        levelText.text = currentLevel.ToString();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Player Mati");
            // bisa panggil GameOver di sini
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
        expToNextLevel = 100f + (currentLevel - 1) * 50f; // Naik 50 tiap level
        Debug.Log("Naik level! Sekarang level: " + currentLevel);
    }
}
