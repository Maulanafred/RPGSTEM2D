using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainmenu : MonoBehaviour
{
    private bool canLoadScene = true; // Flag untuk mengontrol apakah scene dapat dimuat

    // Start is called before the first frame update
    void Start()
    {
        canLoadScene = true; // Pastikan flag diatur ke true saat menu dimulai
    }

    public void LoadScene()
    {
        if (canLoadScene) // Periksa apakah flag mengizinkan pemuatan scene
        {
            canLoadScene = false; // Set flag ke false untuk mencegah pemanggilan ulang
            SceneController.instance.LoadScene("Cutscene"); // Memuat scene "Gameplay"
        }
        else
        {
            Debug.Log("Scene sedang dimuat atau tidak dapat dimuat saat ini.");
        }
    }
}