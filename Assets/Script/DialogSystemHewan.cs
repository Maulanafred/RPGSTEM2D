using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem.LowLevel;

public class DialogSystemHewan : MonoBehaviour
{

    [Header("UI Komponen")]
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;

    public UIManagementGame uiManagementGame; // Referensi ke UIManagementGame

    [Header("Dialog Sebelum Scope")]
    [SerializeField] private string[] nameLinesBefore;
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLinesBefore;



    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Aksi Selanjutnya")]
    [SerializeField] private GameObject nextGameObject; // Objek yang akan diaktifkan setelah dialog selesai

    [Header("Status")]
    public PlayerMovement playerMovement;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public Button[] button;

    public TextMeshProUGUI dialogTextMeshPro; // Referensi ke TextMeshProUGUI

    public GameObject[] objecthehe;


    [Header("Soal Pilihan Ganda")]

    public TextMeshProUGUI questionText; // Teks pertanyaan

    public String questionTextString; // Variabel untuk menyimpan teks pertanyaan
    public GameObject questionPanel;
    public Button[] answerButtons; // 4 tombol untuk pilihan A, B, C, D
    public string[] answerChoices; // Isi teks pilihan (harus 4)
    public int correctAnswerIndex; // Index jawaban benar, 0 = A, 1 = B, dst
    public TMP_Text resultText;    // Teks hasil benar/salah

    private string[] activeNameLines;
    private string[] activeDialogLines;

     private bool questionShown = false;

    public string dialogTextString; // Variabel untuk menyimpan teks dialog


    public string jawabanbenar;

    void Start()
    {
        questionText.text= questionTextString; // Set teks pertanyaan
        playerMovement.animator.SetTrigger("idle");
        playerMovement.enabled = false; // Nonaktifkan PlayerMovement
        AudioManager.Instance.StopSFX("Player",0); // Mainkan efek suara klik
        SetupDialog();
        ShowNextLine();

        for (int i = 0; i < button.Length; i++)
        {
            button[i].interactable = false;
        }

    }

    public void SetupDialog()
    {

        activeDialogLines = dialogLinesBefore;
        activeNameLines = nameLinesBefore;
    }

    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = activeDialogLines[currentLine];
                nameText.text = activeNameLines[currentLine];
                isTyping = false;
                currentLine++;
            }
            else
            {
                ShowNextLine();
            }
        }
    }



    public void ShowNextLine()
    {
        if (currentLine < activeDialogLines.Length && currentLine < activeNameLines.Length)
        {
            typingCoroutine = StartCoroutine(TypeLine(activeDialogLines[currentLine], activeNameLines[currentLine]));
        }
        else
        {
            if (!questionShown) // Periksa apakah pertanyaan sudah ditampilkan
            {
                ShowQuestion(); // Tampilkan pertanyaan
                questionShown = true; // Tandai bahwa pertanyaan sudah ditampilkan
            }
            return; // Hentikan eksekusi lanjutan sampai jawaban dipilih
        }
    }

    IEnumerator TypeLine(string dialog, string speaker)
    {
        isTyping = true;
        dialogText.text = "";
        nameText.text = speaker;

        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        currentLine++;
    }


    void ShowQuestion()
    {
        questionPanel.SetActive(true);
        resultText.text = "";

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Simpan index ke variabel lokal agar tidak tertukar
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answerChoices[i];
            answerButtons[i].onClick.RemoveAllListeners(); // Bersihkan listener sebelumnya
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex)
        {
            resultText.color = Color.green;
            resultText.text =  jawabanbenar; // Tampilkan jawaban benar

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].interactable = false; // Nonaktifkan semua tombol jawaban
            }

            StartCoroutine(AkhiriDialog()); // Akhiri dialog setelah jawaban benar
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = "❌ Jawaban Salah: Coba lagi ya!";
            return;
        }

    }
    
    IEnumerator AkhiriDialog()
    {
        yield return new WaitForSeconds(2f); // Tunggu 1 detik sebelum mengakhiri dialog
        
        uiManagementGame.UpdateMisiUtama(1); // Update misi utama
        BukuModul.instance.UnlockModul(0); // Unlocked modul hewan pertama
        foreach (var obj in objecthehe)
            obj.SetActive(false); // Nonaktifkan objek lain

        foreach (var btn in button)
            btn.interactable = true;

        currentLine = 0; // Reset currentLine
        playerMovement.enabled = true; // Nonaktifkan PlayerMovement

        if (nextGameObject != null)
            nextGameObject.SetActive(true); // Tampilkan objek berikutnya
    }
}
