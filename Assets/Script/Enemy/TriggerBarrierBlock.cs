using UnityEngine;

public class TriggerBarrierBlocker : MonoBehaviour
{
    public string tagPlayer; // Tag untuk player yang akan didorong mundur

    public BoxCollider2D barrierCollider; // Collider untuk barrier

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagPlayer))
        {
            barrierCollider.enabled = true; // Aktifkan collider barrier saat player masuk

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagPlayer))
        {
            barrierCollider.enabled = false; // Nonaktifkan collider barrier saat player keluar

                }

       

    }




}