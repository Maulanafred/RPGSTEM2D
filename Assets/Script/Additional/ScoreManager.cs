using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Jangan lupa tambahkan ini jika belum ada

public class ScoreManager : MonoBehaviour
{
    public UIManagementGame uiManagementGame; // Referensi ke UIManagementGame
    public static ScoreManager Instance; // Singleton instance

    [Header("Data Skor & Progres")]
    public int score; // Skor akan dihitung berdasarkan aksi
    public int level = 1; // Level awal
    public int soalDiselesaikan; // Jumlah soal yang diselesaikan
    public int salahmenjawab; // Jumlah jawaban salah
    public int jumlahpuzzlediperbaiki; // Jumlah puzzle yang diperbaiki
    public int jumlahmusuhdikalahkan; // Jumlah musuh yang dikalahkan

    [Header("UI Komponen Hasil")]
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text soalDiselesaikanText;
    public TMP_Text salahmenjawabText;
    public TMP_Text jumlahpuzzlediperbaikiText;
    public TMP_Text jumlahmusuhdikalahkanText;
    public TMP_Text rankText; // << TEXT BARU untuk Peringkat/Rank
    public TMP_Text kataKataMotivasiText; // << TEXT BARU untuk Kata-Kata Motivasi

    // private bool hasilSudahDitampilkan = false; // Anda menghapus ini, pastikan TampilkanHasilAkhir dipanggil dengan benar dari skrip lain

    void Awake() // Menggunakan Awake untuk inisialisasi Singleton
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Opsional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inisialisasi awal jika perlu
    }

    void Update()
    {

        // Kalkulasi skor dipindahkan ke Update sesuai kode Anda:
        float skorKalkulasi = 0f;
        skorKalkulasi += soalDiselesaikan * 5f;
        skorKalkulasi += jumlahmusuhdikalahkan * 0.5f;
        skorKalkulasi -= salahmenjawab * 2f;
        skorKalkulasi += jumlahpuzzlediperbaiki * 10f;
        score = Mathf.RoundToInt(skorKalkulasi);

        // PENTING: Logika untuk memanggil TampilkanHasilAkhir()
        // (if (uiManagementGame.isGameOver && !hasilSudahDitampilkan))
        // telah Anda hapus dari Update. Pastikan Anda memanggil
        // ScoreManager.Instance.TampilkanHasilAkhir() dari skrip lain
        // (misalnya UIManagementGame) ketika game benar-benar berakhir.
    }

    // Fungsi HitungSkorAkhir() sekarang kosong di kode Anda,
    // karena kalkulasi sudah di Update(). Saya biarkan kosong.
    public void HitungSkorAkhir()
    {
        // Kalkulasi skor sekarang ada di Update().
        // Fungsi ini bisa Anda gunakan jika ingin memicu kalkulasi ulang secara manual
        // di luar Update(), atau bisa juga dihapus jika tidak perlu.
        // Jika ingin digunakan, pindahkan kembali logika kalkulasi skor ke sini dari Update().
    }

    // Fungsi untuk menampilkan semua hasil ke UI
    public void TampilkanHasilAkhir()
    {
        // 'score' sudah terupdate dari Update()
        // Jika Anda ingin memastikan skor paling baru dihitung tepat sebelum tampil,
        // Anda bisa memindahkan logika kalkulasi dari Update() ke sini atau ke HitungSkorAkhir()
        // dan memanggil HitungSkorAkhir() di sini.
        // Untuk saat ini, saya asumsikan 'score' dari Update() sudah cukup.

        Debug.Log($"Menampilkan Hasil Akhir: Skor = {score}, Level = {level}, Misi Selesai = {soalDiselesaikan}, Salah = {salahmenjawab}, Puzzle = {jumlahpuzzlediperbaiki}, Musuh = {jumlahmusuhdikalahkan}");

        if (scoreText != null)
        {
            scoreText.text = $"SKOR AKHIR: {score}";
        }
        if (levelText != null) // Menambahkan kembali update levelText
        {
            levelText.text = $"Level: {level}";
        }
        if (soalDiselesaikanText != null)
        {
            soalDiselesaikanText.text = $"Misi Diselesaikan: {soalDiselesaikan} (+{soalDiselesaikan * 5} poin)";
        }
        if (salahmenjawabText != null)
        {
            salahmenjawabText.text = $"Salah Menjawab: {salahmenjawab} (-{salahmenjawab * 2} poin)";
        }
        if (jumlahpuzzlediperbaikiText != null)
        {
            // Menambahkan tanda '+' untuk konsistensi poin positif
            jumlahpuzzlediperbaikiText.text = $"Puzzle Diperbaiki: {jumlahpuzzlediperbaiki} (+{jumlahpuzzlediperbaiki * 10} poin)";
        }
        if (jumlahmusuhdikalahkanText != null)
        {
            // Menambahkan tanda '+' untuk konsistensi poin positif
            jumlahmusuhdikalahkanText.text = $"Musuh Dikalahkan: {jumlahmusuhdikalahkan} (+{(jumlahmusuhdikalahkan * 0.5f).ToString("F1")} poin)";
        }

        // --- Logika untuk Peringkat dan Kata-Kata Motivasi ---
        string peringkatDiterima = "D"; // Peringkat default
        string kataMotivasi = "Terus semangat belajar ya! Setiap usaha pasti ada hasilnya!"; // Kata-kata default

        if (score >= 100)
        {
            peringkatDiterima = "S+";
            kataMotivasi = "LUAR BIASA! Kamu adalah seorang jenius! Teruslah menginspirasi!";
        }
        else if (score >= 90)
        {
            peringkatDiterima = "S ";
            kataMotivasi = "WOW! Kehebatanmu sungguh mengagumkan! Kamu calon bintang masa depan!";
        }
        else if (score >= 80)
        {
            peringkatDiterima = "A";
            kataMotivasi = "Hebat! Prestasimu sangat membanggakan! Terus asah kemampuanmu!";
        }
        else if (score >= 70)
        {
            peringkatDiterima = "B";
            kataMotivasi = "Bagus sekali! Kamu sudah di jalur yang benar. Jangan berhenti belajar ya!";
        }
        else if (score >= 40)
        {
            peringkatDiterima = "C";
            kataMotivasi = "Sudah cukup baik! Dengan sedikit usaha lagi, kamu pasti bisa lebih hebat!";
        }
        // Jika skor < 50, akan menggunakan peringkat D dan kata motivasi default.

        if (rankText != null)
        {
            rankText.text = $"{peringkatDiterima}";
        }
        else
        {
            Debug.LogWarning("rankText belum di-assign di Inspector!");
        }

        if (kataKataMotivasiText != null)
        {
            kataKataMotivasiText.text = kataMotivasi;
        }
        else
        {
            Debug.LogWarning("kataKataMotivasiText belum di-assign di Inspector!");
        }
        // --- Selesai Logika Peringkat dan Kata-Kata ---
    }

    // Metode untuk menambah data (nama sudah konsisten)
    public void SalahMenjawab()
    {
        salahmenjawab++;
    }

    public void TambahMusuhDikalahkan()
    {
        jumlahmusuhdikalahkan++;
    }

    public void TambahPuzzleDiperbaiki()
    {
        jumlahpuzzlediperbaiki++;
    }

    public void TambahSoalDiselesaikan()
    {
        soalDiselesaikan++;
    }
}