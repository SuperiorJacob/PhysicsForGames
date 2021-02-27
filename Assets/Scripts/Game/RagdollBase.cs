using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RagdollBase : MonoBehaviour
{
    [HideInInspector] public Animator animator = null;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();
    
    public bool RagdollOn
    {
        get { return !animator.enabled; }
        set
        {
            animator.enabled = !value;
            foreach(Rigidbody r in rigidbodies)
            {
                r.isKinematic = !value;
            }
        }
    }

    [ContextMenu("LoadAllRigidbodies")]
    public void LoadAllRigidbodies()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rigidbodies.Add(rb);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        RagdollOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
