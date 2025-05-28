using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ModulHewan
{
    public string judul;
    [TextArea] public string deskripsi;
    public Sprite siluetHewan; // Gambar asli
    public Sprite siluetHewanSebelumDitemukan; // Siluet sebelum ditemukan
    public int soundEffectIndex = 0; // Index suara hewan di AudioManager, -1 jika tidak ada suara
}

// Definisi class ModulHewan ada di sini atau di file terpisah (seperti di atas)

public class BukuModul : MonoBehaviour
{
    public static BukuModul instance;
    public TextMeshProUGUI judulText;
    public TextMeshProUGUI deskripsiText;
    public Image gambarSiluet;

    public ModulHewan[] modulHewan;
    public RawImage[] modulIndicators;
    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;

    public TextMeshProUGUI statusText;

    private int index = 0;
    private bool[] modulUnlocked;

    // --- Tambahan untuk suara hewan dari tombol ---
    public string animalSoundCategory = "Hewan"; // Kategori suara hewan di AudioManager Anda
    private int activeAnimalSoundByButtonIndex = 0; // Index suara hewan yang aktif dimainkan oleh tombol
    // ---------------------------------------------

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        modulUnlocked = new bool[modulHewan.Length];
        // Saat mulai, panggil TampilkanModul. Ini juga akan memastikan tidak ada sisa suara aktif.
        TampilkanModul(index);
        UpdateIndicators();
    }

    public void TampilkanModul(int i)
    {
        if (i < 0 || i >= modulHewan.Length) return;

        AudioManager.Instance.StopSFXGroup("Hewan"); // Hentikan suara hewan sebelumnya

        // --- Hentikan suara hewan yang dimainkan oleh tombol JIKA modul berganti ---
        // Ini penting agar suara dari modul sebelumnya berhenti saat kita navigasi.
        // Asumsi: AudioManager Anda memiliki metode StopSFX(string category, int soundIndex)
        // -------------------------------------------------------------------------

        // Logika untuk menampilkan informasi modul (judul, deskripsi, gambar, status)
        // TIDAK ADA pemutaran suara otomatis di sini. Suara hanya dari tombol.
        if (modulUnlocked[i])
        {
            judulText.text = modulHewan[i].judul;
            deskripsiText.text = modulHewan[i].deskripsi;
            gambarSiluet.sprite = modulHewan[i].siluetHewan;
            activeAnimalSoundByButtonIndex = modulHewan[i].soundEffectIndex;
            statusText.text = "Sudah Ditemukan";
            statusText.color = Color.green;
        }
        else
        {
            judulText.text = "????";
            deskripsiText.text = modulHewan[i].deskripsi; // Atau sesuaikan deskripsi untuk modul terkunci
            gambarSiluet.sprite = modulHewan[i].siluetHewanSebelumDitemukan;
            activeAnimalSoundByButtonIndex = modulHewan[i].soundEffectIndex; // Set index suara hewan

            statusText.text = "Belum Ditemukan";
            statusText.color = Color.red;
        }

        UpdateIndicators();
    }

    public void NextModul()
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Suara klik UI (tetap ada)
        index = (index + 1) % modulHewan.Length;
        TampilkanModul(index); // TampilkanModul akan mengurus penghentian suara hewan dari tombol
    }

    public void PrevModul()
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Suara klik UI (tetap ada)
        index = (index - 1 + modulHewan.Length) % modulHewan.Length;
        TampilkanModul(index); // TampilkanModul akan mengurus penghentian suara hewan dari tombol
    }

    // --- FUNGSI BARU UNTUK TOMBOL PEMUTAR SUARA HEWAN ---// Dalam BukuModul.cs
    public void PlayCurrentAnimalSound()
    {
 
        AudioManager.Instance.PlaySFX("Hewan", index);
    


    }
    // ----------------------------------------------------

    public void UnlockModul(int i)
    {
        if (i < 0 || i >= modulHewan.Length) return;

        modulUnlocked[i] = true;
        if (i == index)
        {
            // Jika modul yang di-unlock adalah yang sedang tampil, refresh tampilannya.
            // TampilkanModul akan dipanggil, yang juga akan menghentikan suara
            // dari tombol yang mungkin aktif dari state sebelumnya.
            TampilkanModul(index);
        }
    }

    private void UpdateIndicators()
    {
        for (int j = 0; j < modulIndicators.Length; j++)
        {
            modulIndicators[j].color = (j == index) ? selectedColor : defaultColor;
        }
    }
}