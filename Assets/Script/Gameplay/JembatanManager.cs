using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class JembatanManager : MonoBehaviour
{

    public TextMeshProUGUI woodCountText; // Referensi ke UI untuk menampilkan jumlah kayu


    public BoxCollider2D bangunJembatan; // Referensi ke skrip BangunJembatan

    public GameObject jembatandibangun; // Referensi ke objek jembatan yang sudah dibangun

    public GameObject buttonAmbilKayu; // Referensi ke tombol ambil kayu

    public GameObject kayuTerdekat; // Objek kayu yang sedang bisa diambil

    public int woodCount = 0; // Jumlah kayu yang dimiliki pemain

    public int maxWoodCount = 6; // Jumlah kayu maksimum yang diperlukan untuk menyelesaikan jembatan
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateUI()
    {
        // Memperbarui UI dengan jumlah kayu yang dimiliki pemain
        woodCountText.text = $"{woodCount} / {maxWoodCount} kayu terkumpul";
    }

    public void UpdateWoodCount(int amount)
    {
        // Memperbarui jumlah kayu yang dimiliki pemain
        woodCount += amount;
        Destroy(kayuTerdekat); // Hancurkan kayu
        buttonAmbilKayu.SetActive(false); // Sembunyikan tombol setelah diambil
        kayuTerdekat = null;


        // Pastikan jumlah kayu tidak melebihi maksimum
        if (woodCount >= maxWoodCount)
        {
            woodCount = maxWoodCount; // Set jumlah kayu ke maksimum
            bangunJembatan.enabled = true; // Aktifkan skrip BangunJembatan
            woodCountText.text = "Pergi ke tanda jembatan dibangun"; // Tampilkan pesan bahwa jembatan sudah dibangun

        }
        else
        {
            // Jika belum mencapai maksimum, perbarui UI
            UpdateUI();
        }

    }

    

}
