using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Objek player
    public Vector3 offset;           // Jarak offset kamera dari player

    void LateUpdate() // tetap pakai LateUpdate untuk sinkronisasi yang halus
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z // biar z kamera tetap
        );
    }
}
