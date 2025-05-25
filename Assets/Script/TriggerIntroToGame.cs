using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerIntroToGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // Memulai coroutine untuk memuat scene "Gameplay" setelah delay 3 detik
        StartCoroutine(LoadSceneAfterDelay(8f)); // Ganti 3f dengan durasi delay yang diinginkan

    }

    // Update is called once per frame
    void Update()
    {

    }



    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Tunggu selama delay yang ditentukan
        SceneController.instance.LoadScene("Gameplay"); // Memuat scene "Game"
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneController.instance.LoadScene("Gameplay2");
        }
    }


}
