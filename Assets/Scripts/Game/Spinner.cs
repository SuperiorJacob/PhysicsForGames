using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 2f;

    private bool finished = false;

    // Update is called once per frame
    void Update()
    {
        if (!finished)
            transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (finished) return;

        if (collision.gameObject.layer == 8)
        {
            EnemyController e;
            if (collision.transform.root.TryGetComponent(out e))
            {
                e.Kill();
            }

            CharacterJoint c;
            if (collision.transform.TryGetComponent(out c))
            {
                collision.transform.parent = null;
                Destroy(c);
            }
        }
        else
        {
            Destroy(GetComponent<Rigidbody>());
            finished = true;
        }

    }
}
