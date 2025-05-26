using UnityEngine;
using UnityEngine.UI;

public class AudioMute : MonoBehaviour
{
    public Sprite muteSprite; // Sprite untuk mute
    public Sprite unmuteSprite; // Sprite untuk unmute
    public Image toggleButtonImage; // Referensi ke Image pada tombol

    public void ToggleMute()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance tidak ditemukan!");
            return;
        }
        AudioManager.Instance.PlaySFX("UI", 1); // Mainkan efek suara klik

        // Periksa apakah audio sedang mute
        if (AudioManager.Instance.IsMasterMuted())
        {
            AudioManager.Instance.ToggleMasterMute(); // Unmute audio
            UpdateButtonSprite(false); // Ganti sprite ke unmute
            Debug.Log("Audio di-unmute.");
        }
        else
        {
            AudioManager.Instance.ToggleMasterMute(); // Mute audio
            UpdateButtonSprite(true); // Ganti sprite ke mute
            Debug.Log("Audio di-mute.");
        }
    }

    private void UpdateButtonSprite(bool isMuted)
    {
        if (toggleButtonImage == null)
        {
            Debug.LogError("ToggleButtonImage belum diassign!");
            return;
        }

        // Ganti sprite berdasarkan status mute
        toggleButtonImage.sprite = isMuted ? muteSprite : unmuteSprite;
    }

    private void Start()
    {
        // Set sprite awal berdasarkan status mute
        if (AudioManager.Instance != null)
        {
            UpdateButtonSprite(AudioManager.Instance.IsMasterMuted());
        }
    }
}