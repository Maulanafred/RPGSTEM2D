using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Namespace ini ada di kode Anda, jadi saya biarkan
using UnityEngine;

public class EventSelesai : MonoBehaviour
{
    public Transform player; // Referensi ke objek player
    public UIManagementGame uiManagementGame; // Referensi ke UIManagementGame
    public GameObject penghalang;
    public GameObject mainCamera;
    public GameObject eventCamera; // Referensi ke kamera event
    public GameObject UIBantu;

    public Transform targetCameraPosition; // Transform yang X dan Y nya menjadi tujuan kamera event

    [Tooltip("Durasi animasi perpindahan kamera event dalam detik.")]
    public float cameraAnimationDuration = 2.0f; // Anda bisa atur ini di Inspector

    private bool aksiSudahDilakukan = false;

    // Start is called before the first frame update
    void Start()
    {
        // Pastikan referensi tidak null jika diperlukan
        if (player == null) Debug.LogError("Referensi Player belum di-assign!", this);
        if (uiManagementGame == null) Debug.LogError("Referensi UIManagementGame belum di-assign!", this);
        // if (penghalang == null) Debug.LogError("Referensi Penghalang belum di-assign!", this); // Mungkin opsional
        if (mainCamera == null) Debug.LogError("Referensi MainCamera belum di-assign!", this);
        if (eventCamera == null) Debug.LogError("Referensi EventCamera belum di-assign!", this);
        // if (UIBantu == null) Debug.LogError("Referensi UIBantu belum di-assign!", this); // Mungkin opsional
        if (targetCameraPosition == null) Debug.LogError("Referensi TargetCameraPosition belum di-assign! Kamera event tidak akan tahu harus bergerak ke mana XY-nya.", this);
    }

    void Update()
    {
        if (uiManagementGame == null) return;

        if (!aksiSudahDilakukan && uiManagementGame.misiYangSudahDiselesaikan >= uiManagementGame.totalMisi)
        {
            aksiSudahDilakukan = true;
            uiManagementGame.isGameOver = true;

            if (penghalang != null) penghalang.SetActive(false);
            if (UIBantu != null) UIBantu.SetActive(true);
            
            if (mainCamera != null) mainCamera.SetActive(false); 

            if (eventCamera != null && player != null && targetCameraPosition != null)
            {
                eventCamera.SetActive(true);
                // Set posisi awal kamera event, Z dari sini akan dipertahankan
                eventCamera.transform.position = player.position + new Vector3(0, 5, -10); 
                
                // Mulai coroutine. Parameter pertama (targetCameraPosition.position) akan memberikan target X,Y,Z
                // namun coroutine akan memodifikasi perilakunya untuk Z.
                StartCoroutine(AnimateCameraToPosition(targetCameraPosition.position, cameraAnimationDuration));
            }
            else
            {
                if(targetCameraPosition == null) Debug.LogError("TargetCameraPosition belum di-assign di Inspector. Animasi kamera event tidak bisa dimulai.");
                else if (mainCamera != null) mainCamera.SetActive(true);
            }
        }
    }

    // Coroutine untuk menganimasikan event kamera ke posisi X, Y target dengan halus, Z tetap.
    IEnumerator AnimateCameraToPosition(Vector3 targetDestinationXYZ, float duration)
    {
        if (eventCamera == null)
        {
            if(mainCamera != null) mainCamera.SetActive(true);
            yield break;
        }

        Vector3 startPosition = eventCamera.transform.position; // Posisi awal eventCamera (termasuk Z awalnya)
        float constantZ = startPosition.z; // Simpan nilai Z awal ini, ini yang tidak akan diubah

        // Kita akan menganimasikan menuju X dan Y dari targetDestinationXYZ, tapi menggunakan constantZ
        float targetX = targetDestinationXYZ.x;
        float targetY = targetDestinationXYZ.y;

        float elapsedTime = 0f;

        Debug.Log($"Memulai animasi kamera dari X:{startPosition.x}, Y:{startPosition.y} menuju X:{targetX}, Y:{targetY}. Sumbu Z dipertahankan di: {constantZ}");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            // Interpolasi hanya untuk X dan Y
            float newX = Mathf.Lerp(startPosition.x, targetX, t);
            float newY = Mathf.Lerp(startPosition.y, targetY, t);

            // Terapkan posisi baru dengan Z yang konstan
            eventCamera.transform.position = new Vector3(newX, newY, constantZ);

            // Opsional: LookAt player
            // if (player != null)
            // {
            //     eventCamera.transform.LookAt(player.transform.position + Vector3.up * 1f);
            // }

            yield return null;
        }

        // Pastikan kamera berakhir tepat di posisi X, Y target, dengan Z yang konstan
        eventCamera.transform.position = new Vector3(targetX, targetY, constantZ);
        Debug.Log($"Animasi kamera event selesai di X:{targetX}, Y:{targetY}, Z:{constantZ}.");

        // Logika untuk mengembalikan ke mainCamera
        if (mainCamera != null)
        {
            mainCamera.SetActive(true);
            Debug.Log("Kamera utama diaktifkan.");
        }
        else
        {
            Debug.LogWarning("MainCamera tidak di-assign, tidak bisa kembali ke kamera utama.");
        }

        yield return new WaitForEndOfFrame();

        if (eventCamera != null)
        {
            eventCamera.SetActive(false);
            Debug.Log("Kamera event dinonaktifkan.");
        }
    }
}