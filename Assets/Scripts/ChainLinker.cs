using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class ChainLinker : MonoBehaviour
{
    public int chainCount = 1;
    public float chainHeight = 0.25f;

    public GameObject inst;
    public Transform startPos;

    [ContextMenu("CreateChains")]
    public void CreateChains()
    {
        HingeJoint prev = null;
        
        for (int i = 0; i < chainCount + 1; i++)
        {
            GameObject obj = Instantiate(inst, transform);

            Vector3 scale = obj.transform.localScale;
            scale.y = chainHeight;

            obj.transform.localScale = scale;
            obj.transform.position = startPos.position + (transform.up * (chainHeight * 2) * i) + transform.up * chainHeight;

            if (prev)
            {
                Rigidbody self = obj.GetComponent<Rigidbody>();

                prev.connectedBody = self;
            }

            prev = obj.GetComponent<HingeJoint>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
