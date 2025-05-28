using UnityEngine;
using UnityEngine.Events; // Penting untuk menggunakan UnityEvent

public class OneTimeTrigger : MonoBehaviour
{
    [Header("Pengaturan Trigger")]
    [Tooltip("Tag dari objek yang bisa mengaktifkan trigger ini (misalnya 'Player'). Biarkan kosong untuk mengizinkan objek apa saja.")]
    public string targetTag = "Player";

    [Tooltip("Aksi yang akan dijalankan sekali ketika trigger ini aktif.")]
    public UnityEvent onTriggeredOnce;

    public GameObject UITutor;

    private bool hasBeenTriggered = false;

    // Pastikan GameObject ini memiliki Collider (misalnya BoxCollider, SphereCollider)
    // dan centang 'Is Trigger' pada Collider tersebut di Inspector.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Jika sudah pernah terpicu, jangan lakukan apa-apa lagi.
        if (hasBeenTriggered == true)
        {
            return;
        }

        if (other.CompareTag(targetTag))
        {
            AudioManager.Instance.PlaySFX("UI",3);
            hasBeenTriggered = true; // Tandai bahwa trigger sudah pernah terpicu
            UITutor.SetActive(true); // Aktifkan UI Tutor

        }
    }




}