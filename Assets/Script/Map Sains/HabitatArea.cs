using UnityEngine;

public class HabitatArea : MonoBehaviour
{
    public string habitatNama; // Misalnya: "Burung", "Kucing"
    public Transform titikTaruh; // Titik tempat hewan ditaruh

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HewanHandler hewan = collision.gameObject.GetComponent<HewanHandler>();
        if (hewan != null && hewan.IsPicked() && hewan.jenisHewan == habitatNama)
        {
            UI_Taruh.instance.TampilkanTaruh(hewan);
            hewan.habitatTarget = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HewanHandler hewan = collision.gameObject.GetComponent<HewanHandler>();
        if (hewan != null && hewan.IsPicked() && hewan.jenisHewan == habitatNama)
        {
            UI_Taruh.instance.Sembunyikan();
        }
    }

}
