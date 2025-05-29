using UnityEngine;

public class PickupKayu : MonoBehaviour
{
    public JembatanManager jembatanManager; // Referensi ke JembatanManager

    private GameObject kayuSaatIni; // Referensi kayu yang disentuhee

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) // Pastikan yang menyentuh adalah pemain
        {
                        jembatanManager.buttonAmbilKayu.SetActive(true);
            kayuSaatIni = this.gameObject; // Simpan kayu yang disentuh player
            jembatanManager.kayuTerdekat = kayuSaatIni; // Kirim ke JembatanManager

        }
    }

   void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) // Pastikan yang menyentuh adalah pemain
        {
            jembatanManager.buttonAmbilKayu.SetActive(false);
            jembatanManager.kayuTerdekat = null;
        }
    }
}