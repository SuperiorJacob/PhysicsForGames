﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)),RequireComponent(typeof(RagdollBase))]
public class EnemyController : MonoBehaviour
{
    public Transform mover;
    public float speed = 80.0f;
    public float turnSpeed = 80.0f;
    public float moving = 1;
    public float maxHealth = 100;
    public GameObject damager;

    private float legs = 1.5f;
    private float arms = 2f;

    private RagdollBase ragdollBase;
    private Animator animator;
    private float health;
    private bool dead = false;
    private Transform playerEyes;

    // Start is called before the first frame update
    void Start()
    {
        playerEyes = Camera.main.transform;
        health = maxHealth;

        animator = GetComponent<Animator>();
        ragdollBase = GetComponent<RagdollBase>();

    }

    // Update is called once per frame
    void Update()
    {
        if (mover != null)
        {
            Vector3 dir = (mover.position - playerEyes.position).normalized;

            Vector3 hold = mover.position - Vector3.up * 1.5f;

            RaycastHit hit;
            if (Physics.Raycast(mover.position, -Vector3.up, out hit, legs + 1, ~(1 << 8)))
            {
                hold = hit.point + Vector3.up * legs;
            }

            float dist = 5f;

            RaycastHit forwardHit;
            if (Physics.Raycast(mover.position, -mover.forward, out forwardHit, 0.1f, ~(1 << 8)))
            {
                dist = -0.3f;
            }

            Vector3 go = mover.forward;
            go.y = 0;

            Debug.DrawLine(mover.position, hold - Vector3.up * legs, Color.black);
            Debug.DrawLine(mover.position, hold + go * -0.1f, Color.blue);

            Vector3 target = hold + go * -dist * (legs + 1f);
            Vector3 moveTowards = Vector3.MoveTowards(mover.position, target, (mover.position.y - target.y > 1f ? 10f : 1f) * speed * Time.fixedDeltaTime);

            mover.rotation = Quaternion.Slerp(mover.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
            mover.position = moveTowards; //playerEyes.transform.position, speed * Time.fixedDeltaTime);
        }

        animator.SetFloat("Speed", moving * speed * Time.fixedDeltaTime);
    }

    public void DeductPart(string part)
    {
        if (part == "Leg" && legs > 0.25f)
        {
            // lazy code x4
            if (legs == 1.5) legs -= 0.5f;
            else
                legs -= 0.75f;
        }
        if (part == "Arm" && arms > 0) arms--;
    }

    public bool IsRagdolled()
    {
        return ragdollBase.RagdollOn;
    }

    // Take damage.
    public bool TakeDamage(int damage, string damageType)
    {
        if (dead) return true;

        int damageMult = 1;

        switch (damageType)
        {
            case "Head":
                damageMult = 5;
                break;
            case "RightLeg":
                damageMult = 2;
                break;
            case "LeftLeg":
                damageMult = 2;
                break;
            default:
                break;
        }

        health -= damage * damageMult;

        if (health <= 0)
        {
            Kill();

            return true;
        }

        return false;
    }

    public void Kill()
    {
        dead = true;
        ragdollBase.RagdollOn = true;
        Destroy(mover.gameObject);
        Destroy(damager);
    }
}
