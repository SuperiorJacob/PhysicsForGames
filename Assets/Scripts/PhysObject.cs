using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VecRot
{
    public Vector3 pos;
    public Quaternion rot;
}

[System.Serializable]
public struct TimeLapse
{
    public GameObject obj;
    public float nextLapse;
    public int currentLapse;
    public List<VecRot> timeArray;
}

[RequireComponent(typeof(Rigidbody))]
public class PhysObject : MonoBehaviour
{
    public Material awakeMat = null;
    public Material sleepingMat = null;

    private Rigidbody rb = null;

    private bool wasAsleep = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        TimeLapse tl = new TimeLapse();
        tl.obj = gameObject;
        tl.nextLapse = 0;
        tl.currentLapse = 0;
        tl.timeArray = new List<VecRot>();

        VecRot a = new VecRot();
        a.pos = tl.obj.transform.position;
        a.rot = tl.obj.transform.rotation;
        tl.timeArray.Add(a);

        ShootTest.lapses.Add(tl);
    }

    void FixedUpdate()
    {
        if (rb.IsSleeping() && !wasAsleep && sleepingMat != null)
        {
            wasAsleep = true;

            GetComponent<MeshRenderer>().material = sleepingMat;
        }
        if (!rb.IsSleeping() & wasAsleep && awakeMat != null)
        {
            wasAsleep = false;

            GetComponent<MeshRenderer>().material = awakeMat;
        }
    }
}
