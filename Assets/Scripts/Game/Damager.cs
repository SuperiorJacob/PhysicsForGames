using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerController p;
        if (other.transform.root.TryGetComponent(out p))
        {
            p.TakeDamage(10);

            Vector3 dir = (transform.position - p.transform.position).normalized;
            dir.y = 0;
            p.transform.GetComponent<CharacterController>().Move(-dir * 0.5f);
        }
    }
}
