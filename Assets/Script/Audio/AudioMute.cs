using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioMute : MonoBehaviour
{
    public Image muteButton; 
    public Sprite muteIcon; 
    public Sprite unmuteIcon; 
    public TextMeshProUGUI buttonText; 
    public AudioManager audioManager; 

    // Key ini bisa jadi opsional jika Anda hanya mengandalkan state dari AudioManager
    private const string MuteButtonStateKey = "MuteButtonState"; 

    private void Start()
    {
        if (audioManager == null)
        {
            audioManager = AudioManager.Instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager tidak ditemukan! Pastikan AudioManager sudah ada di scene.");
                enabled = false; // Nonaktifkan skrip ini jika AudioManager tidak ada
                return;
            }
        }
        // AudioManager.Start() akan memanggil LoadVolumeSettings,
        // jadi mixer seharusnya sudah dalam keadaan yang benar.
        // Kita hanya perlu mengupdate tampilan tombol.

        // Opsi 1: Selalu update tombol berdasarkan state aktual AudioManager
        UpdateButtonText();

        // Opsi 2: Jika Anda ingin tombol mengingat state mute/unmute-nya sendiri
        // secara terpisah dari loading awal volume (misalnya, user klik mute, keluar, masuk lagi, tombol tetap mute)
        // bool wasButtonMuted = PlayerPrefs.GetInt(MuteButtonStateKey, 0) == 1;
        // if (audioManager.IsMasterMuted() != wasButtonMuted) {
        //    // Ada diskrepansi, mungkin karena setting global berubah. Sinkronkan.
        //    // Untuk simple, kita prioritaskan state dari AudioManager
        // }
        // UpdateButtonText();
    }

    // Awake bisa dihapus jika semua inisialisasi penting ada di Start
    // private void Awake()
    // {
    // }

    public void ToggleMute()
    {
        if (audioManager == null) return;

        audioManager.ToggleMasterMute(); // AudioManager akan handle logika mute/unmute dan PlayerPrefs-nya

        // Setelah toggle, update tampilan tombol berdasarkan state aktual dari AudioManager
        UpdateButtonText();

        // Simpan preferensi state tombol (opsional, tapi membantu konsistensi UI)
        PlayerPrefs.SetInt(MuteButtonStateKey, audioManager.IsMasterMuted() ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateButtonText()
    {
        if (audioManager == null || muteButton == null || buttonText == null || muteIcon == null || unmuteIcon == null)
        {
            // Debug.LogWarning("Salah satu komponen UI atau AudioManager belum di-assign di AudioMute.");
            return;
        }

        if (audioManager.IsMasterMuted())
        {
            muteButton.sprite = unmuteIcon; // Tombol menunjukkan aksi "Unmute"
            buttonText.text = "Suara Tidak Aktif"; 
        }
        else
        {
            muteButton.sprite = muteIcon; // Tombol menunjukkan aksi "Mute"
            buttonText.text = "Suara Aktif";
        }
    }
}