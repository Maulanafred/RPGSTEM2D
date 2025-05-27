using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.Profiling;

public class UIManagementGame : MonoBehaviour
{

    [Header("UI GAMEOVER")]

    public Transform[] playerTransform;


    public Animator transisiGameOver; // Referensi ke Animator untuk transisi Game Over

    public GameObject gameOverPanel; // Referensi ke UI Game Over Panel

    public bool isGameOver = false; // Status apakah game sudah berakhir atau belum

    [Header("UI Komponen")]

    public GameObject misiUtamaPanel;

    public GameObject syaratMisiPanel;
    public Button scopefunction;

    public TextMeshProUGUI misiUtama;

    public GameObject pauseMenu; // Referensi ke UI Pause Menu



    public int totalMisi = 3; // total misi yang sudah diselesaikan
    public int misiYangSudahDiselesaikan = 0; // misi yang sudah diselesaikan


    public GameObject[] UIScope;

    public GameObject[] UIGame;

    public Transform player;

    public Transform ScopePosition;


    public PlayerMovement playerMovement;

    public GameObject UiObjective;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }


    // Fungsi untuk Pause Game
    public void PauseGame()
    {
        AudioManager.Instance.StopSFX("Player", 0);
        playerMovement.animator.SetTrigger("idle");
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        Time.timeScale = 0f; // Hentikan waktu di game
        pauseMenu.SetActive(true); // Tampilkan menu pause
        playerMovement.enabled = false; // Nonaktifkan pergerakan player
    }

    // Fungsi untuk Resume Game
    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        Time.timeScale = 1f; // Lanjutkan waktu di game
        pauseMenu.SetActive(false); // Sembunyikan menu pause
        playerMovement.enabled = true; // Aktifkan kembali pergerakan player

        Vector2 moveInput = playerMovement.inputActions.action.ReadValue<Vector2>();
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            string trigger = playerMovement.GetTriggerFromAngle(angle);
            playerMovement.animator.SetTrigger(trigger);
        }
        else
        {
            playerMovement.animator.SetTrigger("idle");
        }

    }

    public void ShowObjective()
    {
        AudioManager.Instance.PlaySFX("UI", 2); // Mainkan efek suara klik
        UiObjective.SetActive(true);
    }

    public void HideObjective()
    {
        UiObjective.SetActive(false);
    }

    public void ActivateScope()
    {
        AudioManager.Instance.StopSFX("Player", 0); // Mainkan efek suara klik
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        playerMovement.animator.SetTrigger("idle"); // mengatur animasi idle player
        // z nya tidak berubah, karena scope tidak bisa bergerak maju mundur
        ScopePosition.position = new Vector3(player.position.x, player.position.y, ScopePosition.position.z); // mengatur posisi scope sesuai posisi player
        // mengakfitkan elemen UI scope
        for (int i = 0; i < UIScope.Length; i++)
        {
            UIScope[i].SetActive(true);
        }
        for (int i = 0; i < UIGame.Length; i++)
        {
            UIGame[i].SetActive(false);
        }
        playerMovement.enabled = false;

        ControlModeManager.instance.SetScopeMode(true);



    }

    public void DeactivateScope()
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        for (int i = 0; i < UIScope.Length; i++)
        {
            UIScope[i].SetActive(false);
        }
        for (int i = 0; i < UIGame.Length; i++)
        {
            UIGame[i].SetActive(true);
        }
        playerMovement.enabled = true;
        ControlModeManager.instance.SetScopeMode(false);

        // Periksa input gerakan dan perbarui animasi
        Vector2 moveInput = playerMovement.inputActions.action.ReadValue<Vector2>();
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            string trigger = playerMovement.GetTriggerFromAngle(angle);
            playerMovement.animator.SetTrigger(trigger);
        }
        else
        {
            playerMovement.animator.SetTrigger("idle");
        }
    }

    public void AktifkanScope()
    {


        StartCoroutine(WaitAndActivateScope());

    }

    private IEnumerator WaitAndActivateScope()
    {
        yield return new WaitForSeconds(3.5f); // Tunggu 0.1 detik
        scopefunction.interactable = true; // Nonaktifkan tombol scopefunction
    }

    public void UpdateMisiUtama()
    {
        misiUtama.text = "Temukan Hewan: " + misiYangSudahDiselesaikan + "/" + totalMisi;
    }

    public void UpdateMisiUtama(int misiDiselesaikan)
    {
        misiYangSudahDiselesaikan += misiDiselesaikan;
        UpdateMisiUtama();
    }

    public void MainMenu()
    {
        AudioManager.Instance.StopBackgroundMusicWithTransition("Gameplay1", 0, 1f); // Hentikan musik latar belakang dengan transisi
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        Time.timeScale = 1f; // Lanjutkan waktu di game
        pauseMenu.SetActive(false); // Sembunyikan menu pause
        SceneController.instance.LoadScene("Mainmenu"); // Kembali ke Main Menu
    }


    ///fungsi untuk buka dan tutu
    ///
    public void ToggleMisiUtamaPanel()
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        misiUtamaPanel.SetActive(!misiUtamaPanel.activeSelf); // Toggle visibility of the main mission panel
    }
    public void ToggleSyaratMisiPanel()
    {
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        syaratMisiPanel.SetActive(!syaratMisiPanel.activeSelf); // Toggle visibility of the mission requirements panel
    }


    public void ShowGameOverPanel()
    {
        isGameOver = true; // Tandai bahwa game sudah berakhir

        AudioManager.Instance.StopSFX("Player", 0); // Hentikan efek suara player
        gameOverPanel.SetActive(true); // Tampilkan panel Game Over
        Time.timeScale = 0f; // Hentikan waktu di game
        playerMovement.enabled = false; // Nonaktifkan pergerakan player
    }

    public void RestartGame()
    {
        if (isGameOver == false) return; // Cegah pemanggilan ulang jika game sudah berakhir
        
        Time.timeScale = 1f;
        transisiGameOver.SetTrigger("hitam"); // Panggil trigger animasi Game Over
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik
        playerMovement.animator.SetTrigger("idle");

        gameOverPanel.SetActive(false); // Sembunyikan panel Game Over

        
        
        StartCoroutine(DelayedRestart()); // Mulai coroutine untuk menunggu sebelum restart

    }

    IEnumerator DelayedRestart()
    {
        // mengambil posisi player dengan tag Player

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        yield return new WaitForSeconds(2f);

        playerObject.transform.position = playerTransform[0].position; // mengatur posisi player ke posisi awal

        yield return new WaitForSeconds(1f); // Tunggu 1 detik sebelum restart
        PlayerStats.instance.currentHealth = PlayerStats.instance.maxHealth; // Reset kesehatan player

        transisiGameOver.SetTrigger("putih");
        playerMovement.enabled = true;
        isGameOver = false; // Tandai bahwa game sudah berakhir
        

        
    }
    





}
