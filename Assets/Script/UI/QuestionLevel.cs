using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable] // Agar bisa terlihat dan di-edit di Inspector
public class QuestionData
{
    [TextArea(3, 5)] // Membuat input field di Inspector lebih besar untuk teks pertanyaan
    public string questionText;        // Teks pertanyaan
    public string[] answerChoices = new string[3]; // Pilihan jawaban A, B, C, D
    [Range(0, 2)] // Jawaban benar berdasarkan indeks (0 untuk A, 1 untuk B, dst.)
    public int correctAnswerIndex;
    public string explanationForCorrectAnswer; // Teks yang ditampilkan jika jawaban benar (dulu 'jawabanbenar')
}

public class QuestionLevel : MonoBehaviour
{
    public static QuestionLevel instance; // Singleton instance

    [Header("UI Komponen")]
    public GameObject questionPanel; // Panel untuk menampilkan pertanyaan
    public TMP_Text questionTextUI; // TAMBAHKAN INI: Teks untuk menampilkan pertanyaan
    public Button[] answerButtons; // Tombol untuk pilihan A, B, C, D (pastikan ada 4 di Inspector)
    public TMP_Text resultText; // Teks hasil benar/salah

    [Header("Data Pertanyaan")]
    public List<QuestionData> allQuestions = new List<QuestionData>(); // Daftar semua pertanyaan
    private QuestionData currentDisplayedQuestion; // Pertanyaan yang sedang ditampilkan

    [Header("Referensi Lain")]
    public PlayerMovement playerMovement; // Referensi ke PlayerMovement (pastikan terhubung)
    // Tidak perlu correctAnswerIndex atau answerChoices tunggal lagi di sini

    void Awake() // Ganti Start ke Awake untuk inisialisasi singleton lebih awal
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Pastikan referensi PlayerMovement ada
        if (playerMovement == null)
        {
            // Coba cari otomatis jika tidak di-assign
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
                Debug.LogError("PlayerMovement tidak ditemukan di scene atau tidak di-assign ke QuestionLevel!");
        }
    }

    void Start()
    {
        if (questionPanel != null)
            questionPanel.SetActive(false); // Sembunyikan panel pertanyaan di awal
        else
            Debug.LogError("Question Panel belum di-assign di QuestionLevel!");

        if (questionTextUI == null)
            Debug.LogError("Question Text UI belum di-assign di QuestionLevel!");

    }

    public void ShowQuestion()
    {
        if(PlayerStats.instance.currentLevel >=10)
        {
            Debug.LogError("PlayerStats instance tidak ditemukan! Pastikan PlayerStats sudah diinisialisasi sebelum memanggil ShowQuestion.");
            return;
        }
        if (allQuestions == null || allQuestions.Count == 0)
        {
            Debug.LogWarning("Tidak ada pertanyaan yang tersedia di QuestionLevel. Melewati pertanyaan.");
            // Jika tidak ada pertanyaan, mungkin langsung panggil PlayerStats untuk menandakan tidak ada jawaban (atau anggap benar tanpa hadiah)
            // PlayerStats.instance.ProcessQuestionOutcome(false); // atau true jika ada default behavior
            Time.timeScale = 1f; // Pastikan game tidak terjeda selamanya
            if (playerMovement != null) playerMovement.enabled = true;
            return;
        }

        resultText.color = Color.white;

        // Pilih pertanyaan secara acak
        int randomIndex = Random.Range(0, allQuestions.Count);
        currentDisplayedQuestion = allQuestions[randomIndex];

        if (resultText != null)
            resultText.text = "Kesempatan menjawab hanya 1 kali saja"; // Kosongkan teks hasil
        
        if (questionTextUI != null)
            questionTextUI.text = currentDisplayedQuestion.questionText;
        else
        {
            Debug.LogError("QuestionTextUI belum di-assign!");
            return; // Keluar jika UI teks pertanyaan tidak ada
        }


        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answerButtons[i] != null)
            {
                answerButtons[i].interactable = true;
                // Pastikan ada komponen TMP_Text di dalam children Button
                TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null && i < currentDisplayedQuestion.answerChoices.Length)
                {
                    buttonText.text = currentDisplayedQuestion.answerChoices[i];
                }
                else if(buttonText == null)
                {
                    Debug.LogError($"Button ke-{i} tidak memiliki TMP_Text sebagai child.");
                }


                answerButtons[i].onClick.RemoveAllListeners(); // Bersihkan listener sebelumnya
                int index = i; // Simpan index ke variabel lokal agar tidak tertukar dalam lambda
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }
        }

        Time.timeScale = 0f; // Pause game saat pertanyaan muncul
        AudioManager.Instance.StopSFX("Player", 0); // Hentikan SFX langkah kaki player jika ada
        if (playerMovement != null)
        {
            if (playerMovement.animator != null) playerMovement.animator.SetTrigger("idle");
            playerMovement.enabled = false; // Nonaktifkan PlayerMovement saat pertanyaan muncul
        }
        
        // Hentikan SFX langkah kaki player jika ada
        // (Anda mungkin perlu menyesuaikan nama parameter atau cara AudioManager Anda bekerja)
        // AudioManager.Instance.StopSFX("Player", 0); 
        
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik saat panel muncul
        AudioManager.Instance.PlaySFX("Soal", 2); // Mainkan efek suara khusus soal muncul

        if (questionPanel != null)
            questionPanel.SetActive(true);
    }

    void CheckAnswer(int selectedIndex)
    {
        bool isCorrect = (selectedIndex == currentDisplayedQuestion.correctAnswerIndex);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answerButtons[i] != null)
                answerButtons[i].interactable = false; // Nonaktifkan semua tombol jawaban
        }

        if (isCorrect)
        {
            AudioManager.Instance.PlaySFX("Soal", 0); // Mainkan efek suara jawaban benar
            if (resultText != null)
            {
                resultText.color = Color.green;
                resultText.text = currentDisplayedQuestion.explanationForCorrectAnswer; // Tampilkan penjelasan jawaban benar
            }
            ScoreManager.Instance.TambahSoalDiselesaikan(); // Tambah jumlah soal yang diselesaikan
            PlayerStats.instance.ProcessQuestionOutcome(true); // Kirim hasil ke PlayerStats
        }
        else
        {
            AudioManager.Instance.PlaySFX("Soal", 1); // Mainkan efek suara jawaban salah
            if (resultText != null)
            {
                resultText.color = Color.red; // Warna default untuk pesan ini adalah merah

                // Ambil teks jawaban yang benar
                string correctAnswerText = currentDisplayedQuestion.answerChoices[currentDisplayedQuestion.correctAnswerIndex];

                // Gunakan Rich Text Tag <color=green> untuk jawaban yang benar
                resultText.text = "❌ Jawaban Salah! Yang benar adalah: <color=green>" + correctAnswerText + "</color>";

            }
            ScoreManager.Instance.SalahMenjawab(); // Tambah jumlah jawaban salah
            PlayerStats.instance.ProcessQuestionOutcome(false); // Kirim hasil ke PlayerStats
        }
        StartCoroutine(Selesai()); // Akhiri dialog setelah jawaban
    }

    IEnumerator Selesai()
    {
        yield return new WaitForSecondsRealtime(3f); // Tunggu 3 detik sebelum menyembunyikan panel

        if (questionPanel != null)
            questionPanel.SetActive(false); // Sembunyikan panel pertanyaan
        
        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Aktifkan kembali PlayerMovement
            // Coba set animasi ke idle, biarkan PlayerMovement handle animasi berdasarkan input di frame berikutnya
            if (playerMovement.animator != null) playerMovement.animator.SetTrigger("idle");
        }
        Time.timeScale = 1f; // Lanjutkan game

        // Bagian di bawah ini untuk set animasi setelah game resume mungkin lebih baik ditangani oleh PlayerMovement.Update()
        // Namun jika ingin dipertahankan:
        // Pastikan inputActions sudah terinisialisasi dan playerMovement.enabled = true sebelum baris ini
        /*
        if (playerMovement != null && playerMovement.enabled) {
            Vector2 moveInput = playerMovement.inputActions.action.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 0.01f)
            {
                // float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
                // string trigger = playerMovement.GetTriggerFromAngle(angle); // Asumsi ada fungsi ini
                // playerMovement.animator.SetTrigger(trigger);
            }
            // else
            // {
            // if (playerMovement.animator != null) playerMovement.animator.SetTrigger("idle");
            // }
        }
        */
    }
}