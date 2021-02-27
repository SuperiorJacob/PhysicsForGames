using System.Collections;
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
            mover.position = Vector3.MoveTowards(mover.position, playerEyes.transform.position, speed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", moving * speed * Time.fixedDeltaTime);
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
    }
}
