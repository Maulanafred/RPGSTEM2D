using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimationPlayer : MonoBehaviour
{

    public Animator animator; // Animator yang akan diaktifkan

    void Start()
    {
        animator.SetTrigger("walkdownright"); // Memanggil trigger "enableAnimation" pada Animator
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void EnableAnimation()
    {
       
    }
}
