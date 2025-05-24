using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public float speed = 5f;
    [SerializeField] private InputActionReference inputActions;

    public Transform animalPosition;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator animator;
    private string currentTrigger = "";

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ambil input setiap frame
        if (!ControlModeManager.instance.isScopeMode) // hanya gerak jika bukan scope
        {
            moveInput = inputActions.action.ReadValue<Vector2>();

            // Animasi dan arah
            // ... (sama seperti sebelumnya)
        }
        else
        {
            moveInput = Vector2.zero; // reset input player saat scope
        }

        // Animasi
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            string trigger = GetTriggerFromAngle(angle);

            if (trigger != currentTrigger)
            {
                ResetAllTriggers();
                animator.SetTrigger(trigger);
                currentTrigger = trigger;
            }
        }
        else
        {
            if (currentTrigger != "idle")
            {
                ResetAllTriggers();
                animator.SetTrigger("idle");
                currentTrigger = "idle";
            }
        }
    }

    void FixedUpdate()
    {
        // Gerak pakai Rigidbody biar tidak geter
        Vector2 movement = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    string GetTriggerFromAngle(float angle)
    {
        if (angle >= 60 && angle < 120)
            return "walkup";
        else if (angle >= 120 && angle < 180)
            return "walkupleft";
        else if (angle >= -180 && angle < -120)
            return "walkdownleft";
        else if (angle >= -120 && angle < -60)
            return "walkdown";
        else if (angle >= -60 && angle < 0)
            return "walkdownright";
        else // 0 - 60
            return "walkupright";
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("walkup");
        animator.ResetTrigger("walkdown");
        animator.ResetTrigger("walkupleft");
        animator.ResetTrigger("walkdownleft");
        animator.ResetTrigger("walkupright");
        animator.ResetTrigger("walkdownright");
        animator.ResetTrigger("idle");
    }
}
