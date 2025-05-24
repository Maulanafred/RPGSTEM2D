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
}

public class BukuModul : MonoBehaviour
{
    public static BukuModul instance;
    public TextMeshProUGUI judulText;
    public TextMeshProUGUI deskripsiText;
    public Image gambarSiluet;

    public ModulHewan[] modulHewan;
    public RawImage[] modulIndicators; // Array untuk menyimpan raw image indikator modul
    public Color selectedColor = Color.green; // Warna untuk modul yang dipilih
    public Color defaultColor = Color.white; // Warna default untuk modul yang tidak dipilih

    public TextMeshProUGUI statusText; // TMP untuk status "ditemukan" atau "belum ditemukan"

    private int index = 0;
    private bool[] modulUnlocked; // Array untuk melacak status unlock modul

    void Start()
    {
        if (instance == null)
        {
            instance = this; // Set instance jika belum ada
        }

        modulUnlocked = new bool[modulHewan.Length]; // Inisialisasi array unlock
        TampilkanModul(index);
        UpdateIndicators(); // Perbarui indikator saat modul pertama kali ditampilkan
    }

    public void TampilkanModul(int i)
    {
        if (i < 0 || i >= modulHewan.Length) return;

        if (modulUnlocked[i])
        {
            // Jika modul sudah ditemukan, tampilkan nama dan gambar asli
            judulText.text = modulHewan[i].judul;
            deskripsiText.text = modulHewan[i].deskripsi;
            gambarSiluet.sprite = modulHewan[i].siluetHewan;

            // Perbarui status TMP
            statusText.text = "Sudah Ditemukan";
            statusText.color = Color.green; // Warna hijau untuk status ditemukan
        }
        else
        {
            // Jika modul belum ditemukan, tampilkan "????" dan siluet sebelum ditemukan
            judulText.text = "????";
            deskripsiText.text = modulHewan[i].deskripsi;
            gambarSiluet.sprite = modulHewan[i].siluetHewanSebelumDitemukan;

            // Perbarui status TMP
            statusText.text = "Belum Ditemukan";
            statusText.color = Color.red; // Warna merah untuk status belum ditemukan
        }

        UpdateIndicators(); // Perbarui indikator setiap kali modul ditampilkan
    }

    public void NextModul()
    {
        index = (index + 1) % modulHewan.Length;
        TampilkanModul(index);
    }

    public void PrevModul()
    {
        index = (index - 1 + modulHewan.Length) % modulHewan.Length;
        TampilkanModul(index);
    }

    public void UnlockModul(int i)
    {
        if (i < 0 || i >= modulHewan.Length) return;

        modulUnlocked[i] = true; // Tandai modul sebagai ditemukan
        TampilkanModul(index); // Perbarui tampilan jika modul yang di-unlock adalah modul yang sedang ditampilkan
    }

    private void UpdateIndicators()
    {
        for (int i = 0; i < modulIndicators.Length; i++)
        {
            if (i == index)
            {
                modulIndicators[i].color = selectedColor; // Ubah warna menjadi "selected"
            }
            else
            {
                modulIndicators[i].color = defaultColor; // Kembalikan ke warna default
            }
        }
    }
}