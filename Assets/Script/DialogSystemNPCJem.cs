using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class dialogSystemNPCJem : MonoBehaviour
{
    
    [Header("UI Komponen")]
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_Text nameText;

    [Header("Dialog Sebelum Scope")]
    [SerializeField] private string[] nameLinesBefore;
    [TextArea(3, 5)]
    [SerializeField] private string[] dialogLinesBefore;

    public JembatanManager jembatanManager; // Referensi ke JembatanManager



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
    
    public Button[] button; // Tombol untuk memulai dialog


    public TextMeshProUGUI dialogTextMeshPro; // Referensi ke TextMeshProUGUI

    public GameObject[] objecthehe;

    private string[] activeNameLines;
    private string[] activeDialogLines;

    public string dialogTextString; // Variabel untuk menyimpan teks dialog

    void Start()
    {
        // mengdisable button
        for (int i = 0; i < button.Length; i++)
        {
            button[i].interactable = false;
        }
        playerMovement.animator.SetTrigger("idle");
        playerMovement.enabled = false; // Nonaktifkan PlayerMovement
        SetupDialog();
        ShowNextLine();

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


            dialogTextMeshPro.text = dialogTextString; // Tampilkan teks dialog di TextMeshProUGUI

            for (int i = 0; i < objecthehe.Length; i++)
            {
                objecthehe[i].SetActive(false); // Nonaktifkan objek lain
            }
            currentLine = 0; // Reset currentLine
            playerMovement.enabled = true; // Nonaktifkan PlayerMovement

            jembatanManager.woodCountText.text = $"{jembatanManager.woodCount} / {jembatanManager.maxWoodCount} kayu terkumpul"; // Tampilkan pesan bahwa jembatan sudah dibangun

            
            for (int i = 0; i < button.Length; i++)
            {
                button[i].interactable = true;
            }
            GameObject[] woodObjects = GameObject.FindGameObjectsWithTag("Wood");
                foreach (GameObject wood in woodObjects)
                {
                    BoxCollider2D collider = wood.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        collider.enabled = true; // Aktifkan BoxCollider2D
                    }
                }
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
