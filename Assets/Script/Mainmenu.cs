using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    public Button[] button;

    public GameObject[] panels; // Array untuk menyimpan semua panel yang ada

    private bool canLoadScene = true; // Flag untuk mengontrol apakah scene dapat dimuat
    private bool isButtonPressed = false; // Flag untuk mencegah spamming tombol

    public AudioClip clickSound; // Musik latar belakang


    void Start()
    {
        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Mainmenu", 0, 1f, 0.6f); // Memainkan musik latar belakang dengan transisi
        canLoadScene = true; // Pastikan flag diatur ke true saat menu dimulai
    }

    public void LoadScene()
    {
        if (canLoadScene) // Periksa apakah flag mengizinkan pemuatan scene
        {
            AudioManager.Instance.StopBackgroundMusicWithTransition("Mainmenu", 0, 1f); // Hentikan musik latar belakang dengan transisi
            AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
            AudioManager.Instance.PlayBackgroundMusicWithTransition2("Gameplay1", 0, 1f, 0.6f); // Memainkan musik latar belakang untuk scene gameplay
            disableButton(); // Nonaktifkan semua tombol
            canLoadScene = false; // Set flag ke false untuk mencegah pemanggilan ulang
            SceneController.instance.LoadScene("Cutscene"); // Memuat scene "Gameplay"
        }
        else
        {
            Debug.Log("Scene sedang dimuat atau tidak dapat dimuat saat ini.");
        }
    }





    public void disableButton()
    {
        foreach (Button btn in button)
        {
            btn.interactable = false; // Nonaktifkan semua tombol
        }
    }
    

    public void OpenStage()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(0));
        StartCoroutine(ResetButtonFlag());
    }


  
    public void OpenKredit()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(3));
        StartCoroutine(ResetButtonFlag());
    }

    public void OpenPengaturan()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(1));
        StartCoroutine(ResetButtonFlag());
    }

    public void OpenExit()
    {
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        StartCoroutine(PlaySoundAndOpenPanel(2));
        StartCoroutine(ResetButtonFlag());
    }

    public void BackToMenu()
    {
        if (isButtonPressed) return;
        isButtonPressed = true;

        StartCoroutine(BackToMenuWithDelay(0.5f)); // Kembali ke menu dengan delay


    }

    // Kembali ke menu dengan delay
    IEnumerator BackToMenuWithDelay(float delay)
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        yield return new WaitForSeconds(delay);
        panels[0].SetActive(false);
        panels[1].SetActive(false);
        panels[2].SetActive(false);
        panels[3].SetActive(false);
        isButtonPressed = false; // Reset flag setelah kembali ke menu

        
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlaySFX("UI", 1);
        if (isButtonPressed) return; // Cegah spamming tombol
        isButtonPressed = true;

        Application.Quit();
        Debug.Log("Exit Game");

        StartCoroutine(ResetButtonFlag());
    }

    private IEnumerator PlaySoundAndOpenPanel(int panelIndex)
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan sound
        yield return new WaitForSeconds(0.2f); // Tunggu durasi sound (sesuaikan durasi ini)
        panels[0].SetActive(panelIndex == 0);
        panels[1].SetActive(panelIndex == 1);
        panels[2].SetActive(panelIndex == 2);
        panels[3].SetActive(panelIndex == 3);
    }

    private IEnumerator ResetButtonFlag()
    {
        yield return new WaitForSeconds(0.5f); // Sesuaikan durasi ini jika diperlukan
        isButtonPressed = false; // Reset flag setelah durasi tertentu
    }

    public void bukaReferensi(){
        Application.OpenURL("https://www.notion.so/References-1f3061b95c7380419ee9ea5207fcd01a");
    }
}


