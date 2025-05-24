using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;

public class UIManagementGame : MonoBehaviour
{


    public GameObject[] UIScope;

    public GameObject[] UIGame;

    public Transform player;

    public Transform ScopePosition;


    public PlayerMovement playerMovement;

    public GameObject UiObjective;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowObjective()
    {
        UiObjective.SetActive(true);
    }

    public void HideObjective()
    {
        UiObjective.SetActive(false);
    }

    public void ActivateScope()
    {
        playerMovement.animator.SetTrigger("idle"); // mengatur animasi idle player
        // z nya tidak berubah, karena scope tidak bisa bergerak maju mundur
        ScopePosition.position = new Vector3(player.position.x, player.position.y, ScopePosition.position.z); // mengatur posisi scope sesuai posisi player
        // mengakfitkan elemen UI scope
        for (int i = 0; i < UIScope.Length; i++)
        {
            UIScope[i].SetActive(true);
        }
        for (int i = 0; i < UIGame.Length; i++)
        {
            UIGame[i].SetActive(false);
        }
        playerMovement.enabled = false;

        ControlModeManager.instance.SetScopeMode(true);



    }

    public void DeactivateScope()
    {
        for (int i = 0; i < UIScope.Length; i++)
        {
            UIScope[i].SetActive(false);
        }
        for (int i = 0; i < UIGame.Length; i++)
        {
            UIGame[i].SetActive(true);
        }
        playerMovement.enabled = true;
        ControlModeManager.instance.SetScopeMode(false);


    }
    





}
