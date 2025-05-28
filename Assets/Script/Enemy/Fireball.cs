using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 10;

    public GameObject enemyHitSoundEffect; // Prefab untuk efek suara saat mengenai musuh


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slime"))
        {
            EnemySlimeAI enemy = collision.GetComponent<EnemySlimeAI>();
            if (enemy != null)
            {
                GameObject soundEffect = Instantiate(enemyHitSoundEffect, transform.position, Quaternion.identity);
                Destroy(soundEffect, 2f); // Hancurkan sound effect setelah 2 detik
                enemy.TakeDamage(damage);
            }

            Debug.Log("Fireball mengenai Slime!");

            // Hancurkan fireball setelah kena musuh
            Destroy(gameObject);
        }

        if (collision.CompareTag("Mushroom"))
        {
            EnemyMushAI enemy = collision.GetComponent<EnemyMushAI>();
            if (enemy != null)
            {
                GameObject soundEffect = Instantiate(enemyHitSoundEffect, transform.position, Quaternion.identity);
                Destroy(soundEffect, 2f); // Hancurkan sound effect setelah 2 detik
                enemy.TakeDamage(damage);
            }

            Debug.Log("Fireball mengenai Slime!");

            // Hancurkan fireball setelah kena musuh
            Destroy(gameObject);
        }



    }
}
