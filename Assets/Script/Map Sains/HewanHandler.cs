using UnityEngine;

public class HewanHandler : MonoBehaviour
{
    private bool sudahDiTaruh = false;

    private Transform playerTransform;
    public string jenisHewan;  // Jenis hewan seperti "Burung", "Kucing", dll.
    public HabitatArea habitatTarget;  // Tempat hewan akan diletakkan

    private bool isPicked = false;  // Apakah hewan sedang diambil


    void Start()
    {
        playerTransform = PlayerMovement.instance.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPicked && !sudahDiTaruh &&  collision.gameObject.CompareTag("Player") && !UI_Taruh.SedangBawaHewan )
        {
            // Menampilkan UI untuk ambil hewan
            UI_Taruh.instance.Tampilkan(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPicked && collision.gameObject.CompareTag("Player") )
        {
            // Menyembunyikan UI ambil hewan jika player keluar dari area
            UI_Taruh.instance.Sembunyikan();
        }
    }

    public bool IsPicked()
    {
        return isPicked;
    }


    public void Ambil()
    {
        isPicked = true;

        transform.SetParent(playerTransform);
        // Ganti posisi hewan menjadi di atas pemain
        transform.position = PlayerMovement.instance.animalPosition.transform.position;
    }

    public void Taruh()
    {
        if (habitatTarget != null && habitatTarget.habitatNama == jenisHewan) // Periksa apakah habitat cocok
        {
            transform.position = habitatTarget.titikTaruh.position;  // Letakkan di titik yang sesuai dalam habitat
            isPicked = false;
            transform.SetParent(null);

            sudahDiTaruh = true;
            // Lakukan animasi atau efek setelah hewan ditaruh di habitat
            UI_Taruh.instance.Sembunyikan(); // Sembunyikan tombol setelah hewan ditaruh
            UI_Taruh.instance.AddTotalTaruh();
        }
    }

    // Fungsi untuk memeriksa apakah habitatnya sesuai
    public bool HewanSudahDiHabitat()
    {
        if (habitatTarget != null && habitatTarget.habitatNama == jenisHewan)
        {
            return true;
        }
        return false;
    }
}
