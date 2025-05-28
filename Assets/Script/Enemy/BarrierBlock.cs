using System.Collections.Generic;
using UnityEngine;

public class BarrierBlocker : MonoBehaviour
{
    public string tagMushroom; // Tag yang akan diblokir

    public string tagSlime; // Tag untuk player yang akan didorong mundur
    public float pushBackForce = 10f; // Gaya mundur saat menyentuh barrier
    public BoxCollider2D barrierCollider; // Collider untuk barrier

    private List<EnemyMushAI> enemiesInBarrier = new List<EnemyMushAI>(); // Daftar musuh di dalam barrier

    private List<EnemySlimeAI> enemiesInBarrierSlime = new List<EnemySlimeAI>(); // Daftar musuh di dalam barrier

    private void Start()
    {
        if (barrierCollider == null)
        {
            barrierCollider = GetComponent<BoxCollider2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagMushroom))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            EnemyMushAI enemyAI = other.GetComponent<EnemyMushAI>();

            if (enemyAI != null)
            {
                if (!enemiesInBarrier.Contains(enemyAI))
                {
                    enemiesInBarrier.Add(enemyAI); // Tambahkan musuh ke daftar
                }

                enemyAI.isBlocked = true; // Set flag untuk menghentikan pergerakan musuh
            }
        }

        if (other.CompareTag(tagSlime))
        {
            EnemySlimeAI enemyAI = other.GetComponent<EnemySlimeAI>();

            if (enemyAI != null)
            {
                if (!enemiesInBarrierSlime.Contains(enemyAI))
                {
                    enemiesInBarrierSlime.Add(enemyAI); // Tambahkan musuh ke daftar
                }

                enemyAI.isBlocked = true; // Set flag untuk menghentikan pergerakan musuh
            }
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagMushroom))
        {
            EnemyMushAI enemyAI = other.GetComponent<EnemyMushAI>();

            if (enemyAI != null)
            {
                enemyAI.isBlocked = false; // Reset flag saat musuh keluar dari barrier
                enemiesInBarrier.Remove(enemyAI); // Hapus musuh dari daftar
            }
        }
        if (other.CompareTag(tagSlime))
        {
            EnemySlimeAI enemyAI = other.GetComponent<EnemySlimeAI>();

            if (enemyAI != null)
            {
                enemyAI.isBlocked = false; // Reset flag saat musuh keluar dari barrier
                enemiesInBarrierSlime.Remove(enemyAI); // Hapus musuh dari daftar
            }
        }
    }

    public void DisableBarrier()
    {
        // Reset semua musuh di dalam barrier
        foreach (EnemyMushAI enemy in enemiesInBarrier)
        {
            if (enemy != null)
            {
                enemy.isBlocked = false; // Reset flag
            }
        }
        foreach (EnemySlimeAI enemy in enemiesInBarrierSlime)
        {
            if (enemy != null)
            {
                enemy.isBlocked = false; // Reset flag
            }
        }

        enemiesInBarrier.Clear(); // Kosongkan daftar musuh
        barrierCollider.enabled = false; // Nonaktifkan collider
    }
}