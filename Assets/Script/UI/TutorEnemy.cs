using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorEnemy : MonoBehaviour
{

    public GameObject uiElements; // Array untuk menyimpan elemen UI yang akan ditampilkan
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Ini juga berfungsi untuk sentuhan di perangkat mobile
        {
            AudioManager.Instance.PlaySFX("UI",1);
            uiElements.SetActive(false); // Nonaktifkan elemen UI saat pemain mengklik atau menyentuh layar
        }
    }
}
