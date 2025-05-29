using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Diperlukan untuk List<>
using System.Linq; // Opsional, bisa berguna

public class ObjectiveIndicator : MonoBehaviour
{
    [Header("Referensi Objek")]
    public Transform playerTransform;
    // Mengganti target tunggal dengan daftar target
    [Tooltip("Daftar semua target objektif yang aktif. Indikator akan menunjuk ke yang terdekat.")]
    public List<Transform> objectiveTargets = new List<Transform>();

    [Header("UI Indikator")]
    public GameObject arrowContainer;
    public RectTransform arrowRectTransform;
    public TextMeshProUGUI objectiveTextMeshPro;

    [Header("Pengaturan Jarak & Tampilan")]
    public float visibilityDistance = 20f;
    [Tooltip("Pesan umum yang ditampilkan jika ada target aktif. Bisa diubah via SetGenericObjectiveMessage().")]
    public string currentObjectiveMessage = "tempat hewan berada"; // Pesan default
    public float screenBorderPadding = 50f;
    public float arrowRotationOffset = 0f;

    [Header("Pengaturan Pulsa (Reminder Otomatis)")]
    public float pulseInterval = 30f;
    public float pulseDuration = 3f;

    // Variabel internal
    private Transform currentNearestTarget; // Target terdekat saat ini
    private float nextPulseTime = 0f;
    private bool isPulsing = false;
    private float pulseEndTime = 0f;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
            else { Debug.LogError("Player Transform tidak ditemukan! Indikator tidak akan berfungsi."); enabled = false; return; }
        }
        if (arrowContainer == null || arrowRectTransform == null || objectiveTextMeshPro == null)
        { Debug.LogError("Satu atau lebih komponen UI belum di-assign! Indikator tidak akan berfungsi."); enabled = false; return; }

        if (objectiveTextMeshPro != null)
        {
            objectiveTextMeshPro.text = currentObjectiveMessage; // Set pesan awal
        }

        if (arrowContainer != null)
        {
            arrowContainer.SetActive(false);
        }
        nextPulseTime = Time.time; // Inisialisasi untuk pulsa pertama
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 1. Cari Target Terdekat
        FindNearestActiveTarget();

        // 2. Jika Tidak Ada Target Terdekat yang Valid
        if (currentNearestTarget == null)
        {
            if (arrowContainer != null && arrowContainer.activeSelf)
            {
                arrowContainer.SetActive(false);
            }
            isPulsing = false;
            // Opsional: tampilkan pesan "Tidak ada objektif" atau kosongkan teks
            // if (objectiveTextMeshPro != null) objectiveTextMeshPro.text = "";
            return;
        }
        
        // Pastikan pesan di UI sesuai dengan pesan objektif saat ini
        // (Berguna jika pesan diubah secara dinamis)
        if (objectiveTextMeshPro != null && objectiveTextMeshPro.text != currentObjectiveMessage)
        {
            objectiveTextMeshPro.text = currentObjectiveMessage;
        }

        // 3. Cek Jarak Pemain ke Target Terdekat
        float distanceToTarget = Vector3.Distance(playerTransform.position, currentNearestTarget.position);

        // 4. Jika Pemain Dekat
        if (distanceToTarget <= visibilityDistance)
        {
            if (arrowContainer.activeSelf)
            {
                arrowContainer.SetActive(false);
            }
            isPulsing = false;
            return;
        }

        // 5. Jika Pemain Jauh - Logika Pulsa Berlaku untuk currentNearestTarget
        if (isPulsing)
        {
            if (Time.time >= pulseEndTime)
            {
                isPulsing = false;
                nextPulseTime = Time.time + pulseInterval;
                if (arrowContainer != null) arrowContainer.SetActive(false);
            }
            else
            {
                if (arrowContainer != null && !arrowContainer.activeSelf) arrowContainer.SetActive(true);
                UpdateArrowVisuals(currentNearestTarget); // Gunakan target terdekat
            }
        }
        else
        {
            if (Time.time >= nextPulseTime)
            {
                isPulsing = true;
                pulseEndTime = Time.time + pulseDuration;
                if (arrowContainer != null) arrowContainer.SetActive(true);
                UpdateArrowVisuals(currentNearestTarget); // Gunakan target terdekat
            }
            else
            {
                if (arrowContainer != null && arrowContainer.activeSelf) arrowContainer.SetActive(false);
            }
        }
    }

    void FindNearestActiveTarget()
    {
        Transform previouslyNearest = currentNearestTarget;
        currentNearestTarget = null;
        if (objectiveTargets == null || objectiveTargets.Count == 0)
        {
            // Jika target sebelumnya ada dan sekarang tidak ada, reset pulsa
            if (previouslyNearest != null) PrimePulseTimers();
            return;
        }

        float minDistance = float.MaxValue;
        bool foundTargetThisFrame = false;

        foreach (Transform target in objectiveTargets)
        {
            if (target == null || !target.gameObject.activeInHierarchy) continue; // Abaikan target null atau tidak aktif

            float distanceToThisTarget = Vector3.Distance(playerTransform.position, target.position);
            if (distanceToThisTarget < minDistance)
            {
                minDistance = distanceToThisTarget;
                currentNearestTarget = target;
                foundTargetThisFrame = true;
            }
        }
        
        // Jika target terdekat berubah dari frame sebelumnya (atau dari null ke ada target, atau sebaliknya)
        if (currentNearestTarget != previouslyNearest)
        {
            PrimePulseTimers(); // Reset timer pulsa agar merespons perubahan target dengan segar
        }

        if (!foundTargetThisFrame && previouslyNearest != null)
        {
             // Semua target mungkin telah di-remove atau menjadi tidak aktif
             PrimePulseTimers();
        }
    }
    
    void PrimePulseTimers()
    {
        nextPulseTime = Time.time;
        isPulsing = false;
    }

    void UpdateArrowVisuals(Transform target) // Sekarang menerima target sebagai parameter
    {
        if (target == null || playerTransform == null || arrowRectTransform == null || Camera.main == null) return;

        Vector3 directionToTargetWorld = (target.position - playerTransform.position).normalized;
        float angle = Mathf.Atan2(directionToTargetWorld.y, directionToTargetWorld.x) * Mathf.Rad2Deg;
        arrowRectTransform.eulerAngles = new Vector3(0, 0, angle + arrowRotationOffset);

        Vector3 targetViewportPos = Camera.main.WorldToViewportPoint(target.position);
        bool isTargetOnScreen = targetViewportPos.z > 0 && targetViewportPos.x > 0 && targetViewportPos.x < 1 && targetViewportPos.y > 0 && targetViewportPos.y < 1;

        if (isTargetOnScreen) {
            if (arrowRectTransform.parent == arrowContainer.transform) {
                arrowRectTransform.anchoredPosition = Vector2.zero;
            }
        } else {
            // ... (Logika posisi tepi layar sama seperti sebelumnya, menggunakan targetViewportPos dari 'target') ...
            Vector3 screenEdgePosition; Vector2 arrowViewportPositionOnEdge;
            if (targetViewportPos.z < 0) { targetViewportPos = -targetViewportPos; targetViewportPos.z = Mathf.Max(0.001f, targetViewportPos.z); }
            Vector2 fromViewportCenterToTarget = new Vector2(targetViewportPos.x - 0.5f, targetViewportPos.y - 0.5f);
            float maxComponent = Mathf.Max(Mathf.Abs(fromViewportCenterToTarget.x), Mathf.Abs(fromViewportCenterToTarget.y));
            if (maxComponent < 0.0001f) maxComponent = 0.0001f;
            Vector2 normalizedDirection = fromViewportCenterToTarget / maxComponent;
            float viewportPaddingX = screenBorderPadding / Screen.width; float viewportPaddingY = screenBorderPadding / Screen.height;
            float paddedViewportExtentX = Mathf.Max(0.001f, 0.5f - viewportPaddingX); float paddedViewportExtentY = Mathf.Max(0.001f, 0.5f - viewportPaddingY);
            arrowViewportPositionOnEdge = new Vector2(0.5f + normalizedDirection.x * paddedViewportExtentX, 0.5f + normalizedDirection.y * paddedViewportExtentY);
            screenEdgePosition = Camera.main.ViewportToScreenPoint(arrowViewportPositionOnEdge);
            arrowRectTransform.position = screenEdgePosition;
        }
    }

    // --- Fungsi Publik untuk Mengelola Daftar Target ---
    public void AddObjectiveTarget(Transform newTarget)
    {
        if (newTarget != null && !objectiveTargets.Contains(newTarget))
        {
            objectiveTargets.Add(newTarget);
            // Tidak perlu langsung FindNearestActiveTarget() di sini, Update() akan menanganinya.
            // Perubahan target terdekat akan otomatis mereset timer pulsa.
        }
    }

    public void RemoveObjectiveTarget(Transform targetToRemove)
    {
        if (targetToRemove != null && objectiveTargets.Contains(targetToRemove))
        {
            objectiveTargets.Remove(targetToRemove);
            // Jika target yang dihapus adalah target terdekat saat ini, Update() berikutnya akan
            // mencari yang baru atau tidak menemukan target, dan timer pulsa akan direset.
        }
    }

    public void ClearAllObjectiveTargets()
    {
        objectiveTargets.Clear();
        // currentNearestTarget akan menjadi null di FindNearestActiveTarget() berikutnya.
        // Timer pulsa akan direset.
    }

    /// <summary>
    /// Mengatur pesan objektif umum yang ditampilkan.
    /// </summary>
    public void SetGenericObjectiveMessage(string newMessage)
    {
        currentObjectiveMessage = newMessage;
        if (objectiveTextMeshPro != null)
        {
            objectiveTextMeshPro.text = currentObjectiveMessage;
        }
    }
}