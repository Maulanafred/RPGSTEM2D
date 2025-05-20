using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{

    public GameObject triggerChat;

    public dialogSystem dialogSystem;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogSystem.playerMovement.enabled = false; // Nonaktifkan PlayerMovement
            dialogSystem.playerMovement.animator.SetTrigger("idle");
            dialogSystem.SetupDialog();
            triggerChat.SetActive(true);

        }
    }
}
