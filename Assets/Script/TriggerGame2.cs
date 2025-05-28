using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGame2 : MonoBehaviour
{

    public GameObject resultPanel; // Referensi ke panel hasil akhir
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            resultPanel.SetActive(true); // Aktifkan panel hasil akhir saat pemain masuk ke trigger
            ScoreManager.Instance.TampilkanHasilAkhir(); // Panggil method untuk menampilkan hasil akhir

        }
    }
}



