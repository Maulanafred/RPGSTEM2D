using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Transform checkpointPosition; // Posisi checkpoint yang akan disimpan
    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyTriggered) return;

        if (other.CompareTag("Player"))
        {
            UIManagementGame uiManager = FindObjectOfType<UIManagementGame>();
            if (uiManager != null)
            {
                uiManager.SetCheckpoint(checkpointPosition);
            }

            alreadyTriggered = true; // Agar tidak bisa dipicu lagi
        }
    }
}
