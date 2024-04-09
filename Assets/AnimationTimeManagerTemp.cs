using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AnimationTimeManagerTemp : MonoBehaviour
{

    public Animator animator;
    public AnimationClip clip;
    public float timeToPlay;

    public void Finish()
    {
        Debug.Log("Animation finished");
    }
    
    [Button]
    public void Attack()
    {
        animator.SetBool("IsAttacking", true);
        var totalTime = animator.GetCurrentAnimatorStateInfo(0).length;
        //var mult = totalTime / timeToPlay;
        //animator.SetFloat("AttackSpeed", mult);
        
        Debug.Log("total time: " + totalTime);
    }
    
    [Button]
    public void Run()
    {
        animator.SetBool("IsRunning", true);
        var clipp = animator.GetCurrentAnimatorStateInfo(0);
        var totalTime = clip.length;
        var mult = totalTime / timeToPlay;
        animator.SetFloat("RunSpeed", mult);
        Debug.Log("clip name: " + clip.name);
        Debug.Log("total time: " + totalTime);
        Debug.Log("mult: " + mult);
        Debug.Log("final : " + (mult*totalTime));
    }
    
    [Button]
    public void Stop()
    {
        
    }

    [Button]
    public void GetHit()
    {
        animator.SetTrigger("GetHit");
    }
    
    [Button]
    public void GetTime()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
