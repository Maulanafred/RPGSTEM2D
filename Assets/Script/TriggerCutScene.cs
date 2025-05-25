using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Memulai coroutine untuk memuat scene "Intro" setelah delay 3 detik
        StartCoroutine(LoadSceneAfterDelay(12f)); // Ganti 3f dengan durasi delay yang diinginkan

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerCutSceneEvent()
    {
        // Memanggil metode untuk memulai cutscene
        SceneController.instance.LoadScene("Intro"); // Ganti "CutScene" dengan nama scene cutscene yang sesuai
    }
    
    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Tunggu selama delay yang ditentukan
        TriggerCutSceneEvent(); // Memanggil metode untuk memulai cutscene
    }
}
