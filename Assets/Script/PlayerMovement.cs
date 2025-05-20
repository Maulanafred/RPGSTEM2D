using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;
    public float speed = 5f;
    [SerializeField] private InputActionReference inputActions;

    public Transform animalPosition;

    private Vector2 moveInput;
    private Animator animator;
    private string currentTrigger = "";

    void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = inputActions.action.ReadValue<Vector2>();




        // Gerak
        transform.Translate(moveInput * speed * Time.deltaTime);

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
        animator.ResetTrigger("idle"); // tambahkan ini
    }
}
