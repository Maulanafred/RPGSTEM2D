using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangunJembatan : MonoBehaviour
{

    public Camera mainCamera; // Referensi ke kamera utama

    public Camera jembatanCamera; // Referensi ke kamera jembatan

    public GameObject uiBangunJembatan; // UI untuk membangun jembatan

    public GameObject uiTransisi;

    public GameObject uiGame; // Referensi ke objek jembatan

    public BoxCollider2D jembatancollider;

    public GameObject jembatandibangun; // Referensi ke objek jembatan yang sudah dibangun

    public bool sudahDibangun = false; // Status apakah jembatan sudah dibangun

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && sudahDibangun == false)
        {
            uiBangunJembatan.SetActive(true); // Tampilkan UI saat pemain masuk ke area trigger
        }

    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && sudahDibangun == false)
        {
            uiBangunJembatan.SetActive(false); // Sembunyikan UI saat pemain keluar dari area trigger
        }
    }

    public void GastartBangunJembatan()
    {
        uiTransisi.SetActive(true); // Tampilkan UI transisi
        sudahDibangun = true; // Tandai jembatan sudah dibangun
        uiGame.SetActive(false); // Sembunyikan UI jembatan

        // Nonaktifkan kamera utama
        mainCamera.gameObject.SetActive(false);

        // Aktifkan kamera jembatan
        jembatanCamera.gameObject.SetActive(true);

        StartCoroutine(BangunJembatanCoroutine());
    }

    private IEnumerator BangunJembatanCoroutine()
    {
        
        jembatancollider.enabled = false; // Nonaktifkan collider jembatan

        yield return new WaitForSeconds(2f);
        jembatandibangun.SetActive(true); // Aktifkan objek jembatan yang sudah dibangun
        // Tunggu selama 3 detik
        yield return new WaitForSeconds(4f);
        uiTransisi.SetActive(false);

        // Sembunyikan UI setelah 3 detik
        uiBangunJembatan.SetActive(false);

        mainCamera.gameObject.SetActive(true);
        uiTransisi.SetActive(false); // Tampilkan UI transisi

        uiGame.SetActive(true); // Sembunyikan UI jembatan

        // Nonaktifkan kamera jembatan
        jembatanCamera.gameObject.SetActive(false);
    }
}
