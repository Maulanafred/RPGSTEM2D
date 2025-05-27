using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioMixer audioMixer;

    [System.Serializable]
    public class SoundEffectGroup
    {
        public string groupName;
        public AudioSource[] soundEffects;
    }

    [System.Serializable]
    public class BackgroundMusicGroup
    {
        public string groupName;
        public AudioSource[] backgroundMusics;
    }

    public SoundEffectGroup[] audioSFXGroups;
    public BackgroundMusicGroup[] audioBackgroundMusicGroups;

    private const string MasterVolumeKey = "MasterVolume";
    private const string LastMasterVolumeKey = "LastMasterVolume"; // Untuk menyimpan volume sebelum mute
    private const float MutedLinearVolume = 0.0001f; // Nilai linear yang dianggap mute

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        // LoadVolumeSettings dipanggil di Start untuk memastikan semua siap
    }

    private void Start()
    {
        // Pastikan Instance sudah diset dari Awake jika ini adalah instance pertama.
        // Jika tidak, ini bisa jadi masalah jika ada instance lain yang tidak dihancurkan dengan benar.
        if (Instance != this && Instance != null) { /* Sudah ada instance lain */ return; }
        Instance = this; // Pastikan instance diset jika ini yang pertama

        LoadVolumeSettings(); // Muat pengaturan volume saat game dimulai
    }

    public void LoadVolumeSettings()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer belum diassign di AudioManager!");
            return;
        }
        // Dapatkan nilai linear dari PlayerPrefs, default ke 1 (full volume)
        float masterVolLinear = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        SetVolume(MasterVolumeKey, masterVolLinear, false); // Set volume tanpa menyimpan lagi ke PlayerPrefs saat load awal
    }

    // Overload SetVolume untuk kontrol penyimpanan ke PlayerPrefs
    public void SetVolume(string parameter, float linearValue)
    {
        SetVolume(parameter, linearValue, true);
    }
    
    public void SetVolume(string parameter, float linearValue, bool saveToPrefs)
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer belum diassign!");
            return;
        }

        linearValue = Mathf.Clamp(linearValue, MutedLinearVolume, 1f); // Clamp nilai linear

        float volumeDB;
        if (linearValue <= MutedLinearVolume) // Jika nilai linear sangat kecil, anggap mute
        {
            volumeDB = -80f;
        }
        else
        {
            volumeDB = Mathf.Log10(linearValue) * 20f;
        }
        
        audioMixer.SetFloat(parameter, volumeDB);

        if (saveToPrefs)
        {
            PlayerPrefs.SetFloat(parameter, linearValue); // Simpan nilai linear yang digunakan
            PlayerPrefs.Save(); // Simpan perubahan ke disk
            // Debug.Log($"SetVolume: Saved {parameter} to PlayerPrefs with linear value {linearValue} (dB: {volumeDB})");
        }
        // else
        // {
        //     Debug.Log($"SetVolume: Applied {parameter} with linear value {linearValue} (dB: {volumeDB}) WITHOUT saving to PlayerPrefs");
        // }
    }

    public void ToggleMasterMute()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer belum diassign untuk ToggleMasterMute!");
            return;
        }

        // Dapatkan nilai linear saat ini dari PlayerPrefs sebagai basis
        float currentMasterLinear = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);

        if (currentMasterLinear <= MutedLinearVolume) // Jika saat ini dianggap mute (berdasarkan PlayerPrefs)
        {
            Debug.Log("Unmuting Master Volume");
            // Unmute: Kembalikan ke volume terakhir sebelum mute
            float lastVolumeLinear = PlayerPrefs.GetFloat(LastMasterVolumeKey, 1f); // Ambil volume terakhir, default 1 jika tidak ada
            SetVolume(MasterVolumeKey, lastVolumeLinear); // SetVolume akan update mixer & PlayerPrefs[MasterVolumeKey]
        }
        else // Jika saat ini tidak mute
        {
            Debug.Log("Muting Master Volume");
            // Mute: Simpan volume saat ini sebelum di-mute
            PlayerPrefs.SetFloat(LastMasterVolumeKey, currentMasterLinear);
            // Set master volume ke nilai mute (linear kecil), yang akan mengatur mixer ke -80dB
            // dan menyimpan MutedLinearVolume ke PlayerPrefs[MasterVolumeKey]
            SetVolume(MasterVolumeKey, MutedLinearVolume);
        }
        // PlayerPrefs.Save() sudah dihandle di dalam SetVolume jika saveToPrefs true
    }

    public bool IsMasterMuted()
    {
        if (audioMixer == null) {
            Debug.LogWarning("AudioMixer null in IsMasterMuted, returning true (assuming muted).");
            return true; // Default ke muted jika mixer tidak ada
        }

        float currentVolumeDb;
        if (audioMixer.GetFloat(MasterVolumeKey, out currentVolumeDb))
        {
            return currentVolumeDb <= -79f; // Gunakan toleransi sedikit, karena -80dB adalah absolut
        }
        
        // Fallback jika GetFloat gagal: cek berdasarkan PlayerPrefs
        Debug.LogWarning($"Could not get {MasterVolumeKey} from AudioMixer. Checking PlayerPrefs for mute state.");
        return PlayerPrefs.GetFloat(MasterVolumeKey, 1f) <= MutedLinearVolume;
    }

    // ... (Fungsi PlaySFX, PlayBackgroundMusic, dll. tetap sama) ...
    #region Background Music Functions


    public void PlayBackgroundMusic(string groupName, int index)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.backgroundMusics.Length)
        {
            group.backgroundMusics[index].Play();
        }
        else
        {
            Debug.LogWarning("Background music group or index not found.");
        }
    }


    public void StopBackgroundMusic(string groupName, int index)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.backgroundMusics.Length)
        {
            group.backgroundMusics[index].Stop();
        }
    }


    public void PlayBackgroundMusicWithTransition(string groupName, int index, float fadeDuration)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.backgroundMusics.Length)
        {
            StartCoroutine(FadeInAndPlay(group.backgroundMusics[index], fadeDuration));
        }
        else
        {
            Debug.LogWarning("Background music group or index not found.");
        }
    }


    private IEnumerator FadeInAndPlay(AudioSource audioSource, float fadeDuration)
    {
        audioSource.volume = 0f; // Set volume to 0 for fade in
        audioSource.Play();

        float targetVolume = 1f; // Target volume
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
        audioSource.volume = targetVolume; // Ensure volume is set to target
    }
    
    public void PlayBackgroundMusicWithTransition2(string groupName, int index, float fadeDuration, float targetVolume)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.backgroundMusics.Length)
        {
            StartCoroutine(FadeInAndPlay2(group.backgroundMusics[index], fadeDuration, targetVolume));
        }
        else
        {
            Debug.LogWarning("Background music group or index not found.");
        }
    }
    private IEnumerator FadeInAndPlay2(AudioSource audioSource, float fadeDuration, float targetVolume)
    {
        audioSource.volume = 0f; // Set volume to 0 for fade in
        audioSource.Play();
        float currentTime = 0f;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
        audioSource.volume = Mathf.Clamp(targetVolume, 0f, 1f); // Ensure volume is set to target
    }

    public void StopBackgroundMusicWithTransition(string groupName, int index, float fadeOutDuration)
    {
        BackgroundMusicGroup group = System.Array.Find(audioBackgroundMusicGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.backgroundMusics.Length)
        {
            StartCoroutine(FadeOutAndStop(group.backgroundMusics[index], fadeOutDuration));
        }
        else
        {
            Debug.LogWarning("Background music group or index not found.");
        }
    }
    private IEnumerator FadeOutAndStop(AudioSource audioSource, float fadeOutDuration)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }
    #endregion

    #region Sound Effect Functions

    public void PlaySFX(string groupName, int index)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.soundEffects.Length)
        {
            AudioSource source = group.soundEffects[index];
            // AudioClip clip = source.clip; // Tidak perlu jika AudioSource sudah punya clip
            if (source.clip != null)
            {
                source.PlayOneShot(source.clip);
            } else {
                Debug.LogWarning($"SFX {groupName}[{index}] tidak memiliki AudioClip.");
            }
        }
        else
        {
            Debug.LogWarning("Sound effect group or index not found.");
        }
    }

    public void StopSFX(string groupName, int index)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.soundEffects.Length)
        {
            group.soundEffects[index].Stop();
        }
    }
    
    public bool IsSFXPlaying(string groupName, int index)
    {
        SoundEffectGroup group = System.Array.Find(audioSFXGroups, g => g.groupName == groupName);
        if (group != null && index >= 0 && index < group.soundEffects.Length)
        {
            return group.soundEffects[index].isPlaying;
        }
        return false;
    }
    #endregion
}