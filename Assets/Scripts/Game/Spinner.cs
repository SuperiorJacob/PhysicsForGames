using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 2f;
    public bool dontDamage = false;

    private bool finished = false;
    private int bounces = 3;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!finished)
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (dontDamage || finished) return;

        if (collision.gameObject.layer == 8)
        {
            EnemyController e;
            string n = collision.transform.name;

            RaycastHit f = new RaycastHit();
            f.point = collision.contacts[0].point;
            f.normal = collision.contacts[0].normal;

            if (collision.transform.root.TryGetComponent(out e))
            {
                if (e.TakeDamage(200, n, f)
                    || n == "LeftArm"
                    || n == "RightArm"
                    || n == "LeftLeg"
                    || n == "LeftUpLeg"
                    || n == "RightLeg"
                    || n == "RightUpLeg")
                {
                    CharacterJoint c;
                    if (collision.transform.TryGetComponent(out c))
                    {
                        collision.transform.parent = null;
                        Destroy(c);
                    }
                }
            }
        }
        else if (bounces <= 0)
        {
            Destroy(GetComponent<Rigidbody>());
            finished = true;
        }
        
        if (bounces > 0)
        {
            bounces -= 1;

            //rb.AddForce(collision.contacts[0].normal * -50f);
        }
    }
}
