using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModulPembelajaran : MonoBehaviour
{
    public GameObject modulPanel;
    public TMP_Text judulText;
    public TMP_Text isiText;

    [System.Serializable]
    public class Materi
    {
        public string judul;
        [TextArea(5, 15)] public string isi;
    }

    public Materi[] daftarMateri;
    public Button[] tombolTopik;

    void Start()
    {
        modulPanel.SetActive(false);

        for (int i = 0; i < tombolTopik.Length; i++)
        {
            int index = i;
            tombolTopik[i].onClick.AddListener(() => TampilkanMateri(index));
        }

        TampilAwal(0);
    }

    public void TampilkanModul(bool status)
    {

        modulPanel.SetActive(status);
    }

    void TampilkanMateri(int index)
    {
        AudioManager.Instance.PlaySFX("UI", 1);

         judulText.text = daftarMateri[index].judul;
        isiText.text = daftarMateri[index].isi;
      
    }

    public void TampilAwal(int index)
    {
        judulText.text = daftarMateri[index].judul;
        isiText.text = daftarMateri[index].isi;
    }
}
