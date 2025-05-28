using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance

    public int score ; // Skor awal

    public int level ; // Level awal

    public int salahmenjawab ; // Jumlah jawaban salah

    public int jumlahpuzzlediperbaiki; // Jumlah puzzle yang diperbaiki
    public int jumlahmusuhdikalahkan ; // Jumlah musuh yang dikalahkan

    [Header("UI Komponen")]
    public TMPro.TMP_Text scoreText; // Referensi ke teks skor di UI
    public TMPro.TMP_Text levelText; // Referensi ke teks level di UI
    public TMPro.TMP_Text salahmenjawabText; // Referensi ke teks jumlah jawaban salah di UI
    public TMPro.TMP_Text jumlahpuzzlediperbaikiText; // Referensi ke teks jumlah puzzle yang diperbaiki di UI
    public TMPro.TMP_Text jumlahmusuhdikalahkanText; // Referensi ke teks jumlah musuh yang dikalahkan di UI

    // Start is called before the first frame update
    void Start()
    {
        Instance = this; // Inisialisasi singleton instance
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
