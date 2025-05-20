using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeFunction : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField] private InputActionReference inputActions; // input dari analog kanan

    [Header("Camera Settings")]
    public Transform cameraTarget; // objek yang akan digerakkan
    public Transform player; // referensi ke posisi player
    public float maxDistance = 5f; // jarak maksimum dari player

    private Vector2 moveInput;

    void Update()
    {
        moveInput = inputActions.action.ReadValue<Vector2>();

        if (cameraTarget != null && player != null)
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f) * speed * Time.deltaTime;

            Vector3 newPosition = cameraTarget.position + move;
            float distance = Vector3.Distance(player.position, newPosition);

            if (distance <= maxDistance)
            {
                cameraTarget.position = newPosition;
            }
            else
            {
                Vector3 direction = (newPosition - player.position).normalized;
                cameraTarget.position = player.position + direction * maxDistance;
            }

            // Pastikan Z tetap 0
            Vector3 fixedPos = cameraTarget.position;
            fixedPos.z = -10f;
            cameraTarget.position = fixedPos;
        }
    }
}
