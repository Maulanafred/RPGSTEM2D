using UnityEngine;

public class Tutor : MonoBehaviour
{
    [Header("UI Komponen")]
    public GameObject[] uiTutor; // Referensi ke semua panel UI Tutor kamu

    private int currentIndex = 0; // Untuk melacak UI Tutor mana yang sedang aktif

    void Start()
    {
        AudioManager.Instance.PlaySFX("UI",3);
        gameObject.SetActive(true); // Pastikan Tutor diaktifkan saat mulai
        // Pastikan array uiTutor tidak kosong
        if (uiTutor == null || uiTutor.Length == 0)
        {
            Debug.LogError("Array uiTutor belum diisi atau kosong!");
            gameObject.SetActive(false); // Nonaktifkan Tutor jika tidak ada UI
            return;
        }

        // Nonaktifkan semua UI Tutor terlebih dahulu
        for (int i = 0; i < uiTutor.Length; i++)
        {
            if (uiTutor[i] != null)
            {
                uiTutor[i].SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Elemen uiTutor pada indeks {i} adalah null.");
            }
        }

        // Aktifkan UI Tutor pertama jika ada
        if (uiTutor[0] != null)
        {
            uiTutor[0].SetActive(true);
        }
        else
        {
            // Jika elemen pertama null, coba cari yang valid berikutnya atau matikan tutor
            ShowNextUI();
        }
    }

    void Update()
    {
        // Deteksi sentuhan layar atau klik mouse kiri
        if (Input.GetMouseButtonDown(0)) // Ini juga berfungsi untuk sentuhan di perangkat mobile
        {
            ShowNextUI();
        }
    }

    void ShowNextUI()
    {
        AudioManager.Instance.PlaySFX("UI",1); // Memainkan efek suara klik saat UI Tutor berpindah
        // 1. Nonaktifkan UI Tutor saat ini (jika masih ada dan valid)
        if (currentIndex < uiTutor.Length && uiTutor[currentIndex] != null)
        {
            uiTutor[currentIndex].SetActive(false);
        }

        // 2. Pindah ke indeks berikutnya
        currentIndex++;

        // 3. Cek apakah masih ada UI Tutor berikutnya untuk ditampilkan
        if (currentIndex < uiTutor.Length)
        {
            if (uiTutor[currentIndex] != null)
            {
                uiTutor[currentIndex].SetActive(true);
            }
            else
            {
                // Jika UI berikutnya null, coba lewati ke yang selanjutnya
                Debug.LogWarning($"Elemen uiTutor pada indeks {currentIndex} adalah null, mencoba lanjut.");
                ShowNextUI(); // Rekursif panggil untuk melewati elemen null
            }
        }
        else
        {
            // Jika tidak ada UI Tutor lagi, nonaktifkan GameObject Tutor ini
            Debug.Log("Semua UI Tutor telah ditampilkan. Menonaktifkan Tutor.");
            gameObject.SetActive(false);
        }
    }
}