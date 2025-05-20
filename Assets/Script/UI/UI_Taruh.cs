using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UI_Taruh : MonoBehaviour
{
    public static UI_Taruh instance;

    public GameObject jalan;


    public int totalTaruh;

    public Button tombolTaruh;
    public Button tombolAmbil;
    private HewanHandler hewanAktif;

    private bool jalanSudahDibuka = false;


    public void Update()
    {
        if (totalTaruh >= 4 && !jalanSudahDibuka)
        {
            BukaJalan();
            jalanSudahDibuka = true;
        }
    }
    void Awake()
    {
        instance = this;

        // Sembunyikan tombol taruh di awal
        tombolTaruh.gameObject.SetActive(false);
        tombolTaruh.onClick.AddListener(OnTaruh);

        // Sembunyikan tombol ambil di awal
        tombolAmbil.gameObject.SetActive(false);
        tombolAmbil.onClick.AddListener(OnAmbil);
    }

    public void Tampilkan(HewanHandler hewan)
    {
        hewanAktif = hewan;
        tombolAmbil.gameObject.SetActive(true); // Menampilkan tombol ambil jika hewan ditemukan
    }

    public void Sembunyikan()
    {
        tombolTaruh.gameObject.SetActive(false);
        tombolAmbil.gameObject.SetActive(false);
        hewanAktif = null;
    }

    private void OnTaruh()
    {
        if (hewanAktif != null && hewanAktif.HewanSudahDiHabitat()) // Cek apakah habitat sesuai
        {
            hewanAktif.Taruh();
        }
        else
        {
            // Jika habitat tidak sesuai, beri feedback
            Debug.Log("Habitat tidak sesuai dengan hewan.");
        }
    }

    private void OnAmbil()
    {
        if (hewanAktif != null)
        {
            hewanAktif.Ambil();
            tombolTaruh.gameObject.SetActive(false); // Sembunyikan tombol taruh saat hewan diambil
            tombolAmbil.gameObject.SetActive(false); // Sembunyikan tombol ambil setelah hewan diambil
        }
    }
    public static bool SedangBawaHewan
    {
        get { return instance != null && instance.hewanAktif != null; }
    }

    public void TampilkanTaruh(HewanHandler hewan)
    {
        hewanAktif = hewan;
        tombolTaruh.gameObject.SetActive(true);
    }


    // Fungsi untuk memeriksa apakah tombol taruh bisa ditampilkan berdasarkan jarak ke habitat
    public void CekPosisiPemain(HewanHandler hewanAktif)
    {
        if (hewanAktif != null && hewanAktif.HewanSudahDiHabitat())
        {
            tombolTaruh.gameObject.SetActive(true); // Menampilkan tombol taruh saat hewan sudah berada di habitat yang benar
        }
        else
        {
            tombolTaruh.gameObject.SetActive(false); // Menyembunyikan tombol taruh jika belum dekat dengan habitat
        }
    }

    public void BukaJalan(){
        jalan.SetActive(true);
    }

    public void AddTotalTaruh(){
        totalTaruh++;
    }
}
