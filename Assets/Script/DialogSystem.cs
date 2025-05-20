using System.Collections;
using UnityEngine;
using TMPro;

public class dialogSystem : MonoBehaviour
{
    
    [Header("UI Komponen")]
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;

    [Header("Dialog Sebelum Scope")]
    [SerializeField] private string[] nameLinesBefore;
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLinesBefore;

    [Header("Dialog Sesudah Scope")]
    [SerializeField] private string[] nameLinesAfter;
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLinesAfter;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.03f;

    [Header("Aksi Selanjutnya")]
    [SerializeField] private GameObject nextGameObject; // Objek yang akan diaktifkan setelah dialog selesai

    [Header("Status")]
    public bool hasGotScope = false;

    public PlayerMovement playerMovement;

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public GameObject skop;

    public GameObject[] objecthehe;

    private string[] activeNameLines;
    private string[] activeDialogLines;

    void Start()
    {
        playerMovement.animator.SetTrigger("idle");
        playerMovement.enabled = false; // Nonaktifkan PlayerMovement
        SetupDialog();
        ShowNextLine();

    }

    public void SetupDialog()
    {
        if (hasGotScope == false)
        {
            activeDialogLines = dialogLinesBefore;
            activeNameLines = nameLinesBefore;
        }
        else if (hasGotScope == true)
        {
            activeDialogLines = dialogLinesAfter;
            activeNameLines = nameLinesAfter;
                            dialogText.text = activeDialogLines[currentLine];
                nameText.text = activeNameLines[currentLine];
        }
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
            // Jika sudah mencapai akhir dialog ubah bool hasGotScope
            hasGotScope = true;
            skop.SetActive(false); // Aktifkan objek scope
            dialogText.text = "";
            nameText.text = "";
            for (int i = 0; i < objecthehe.Length; i++)
            {
                objecthehe[i].SetActive(false); // Nonaktifkan objek lain
            }
            currentLine = 0; // Reset currentLine
            playerMovement.enabled = true; // Nonaktifkan PlayerMovement
            activeDialogLines = dialogLinesAfter;
            activeNameLines = nameLinesAfter;

            if (nextGameObject != null)
            {
                nextGameObject.SetActive(true); // Tampilkan objek berikutnya
            }
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
}
