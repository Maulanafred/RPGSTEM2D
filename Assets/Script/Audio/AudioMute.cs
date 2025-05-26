using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioMuteHandler : MonoBehaviour
{
    public Sprite[] muteSprite; // Sprite untuk mute
    public Image toggleButtonImage; // Referensi ke Image pada tombol


    public TextMeshProUGUI muteText; // Referensi ke TextMeshProUGUI untuk teks mute

    private void Start()
    {
        AudioManager.Instance.LoadVolumeSettings();
        Debug.Log("IsMuted: " + AudioManager.Instance.IsMasterMuted());
        UpdateMuteButtonSprite();
    }


    private void UpdateMuteButtonSprite()
    {
        toggleButtonImage.sprite = AudioManager.Instance.IsMasterMuted() ? muteSprite[1] : muteSprite[0];
        muteText.text = AudioManager.Instance.IsMasterMuted() ? "Suara tidak aktif" : "suara aktif";
        
    }


    
    public void ButtonMute()
    {
        if (toggleButtonImage != null)
        {
            AudioManager.Instance.ToggleMasterMute();

            UpdateMuteButtonSprite();

        }
    }
}