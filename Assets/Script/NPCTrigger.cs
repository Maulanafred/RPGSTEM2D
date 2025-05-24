using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public GameObject triggerChat; // UI untuk tombol interaksi

    public GameObject dialogBox; // UI untuk dialog
    public dialogSystem dialogSystem;

    private bool hasTriggered = false; // Melacak apakah trigger sudah dipicu
    private bool isPlayerInRange = false; // Melacak apakah pemain berada di area trigger

    void Start()
    {
        triggerChat.SetActive(false); // Pastikan tombol interaksi tidak terlihat di awal
    }

    void Update()
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // Cek apakah trigger belum dipicu
        {
            isPlayerInRange = true; // Tandai bahwa pemain berada di area trigger
            triggerChat.SetActive(true); // Tampilkan tombol interaksi
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; // Tandai bahwa pemain keluar dari area trigger
            triggerChat.SetActive(false); // Sembunyikan tombol interaksi
        }
    }

    public void StartDialog()
    {
        dialogBox.SetActive(true); // Tampilkan UI dialog
        hasTriggered = true; // Tandai bahwa trigger sudah dipicu
        isPlayerInRange = false; // Nonaktifkan interaksi lebih lanjut
        triggerChat.SetActive(false); // Sembunyikan tombol interaksi

    }
}