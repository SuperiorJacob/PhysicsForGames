using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    public static List<TimeLapse> lapses = new List<TimeLapse>();

    public GameObject ball;
    public Transform towards;
    public float speed = 100;

    private float nextLapse = 0;

    public bool recalling = false;

    void Update()
    {
        if (!recalling && Input.GetKeyUp(KeyCode.R))
        {
            recalling = true;
            nextLapse = Time.time + 5;
        }

        if (recalling)
        {
                for (int i = 0; i < lapses.Count; i++)
                {
                    TimeLapse lapse = lapses[i];

                    lapse.obj.GetComponent<Rigidbody>().isKinematic = true;
                    lapse.obj.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    lapse.obj.transform.position = Vector3.MoveTowards(lapse.obj.transform.position, lapse.timeArray[lapse.currentLapse].pos, 0.01f);
                    lapse.obj.transform.rotation = Quaternion.Lerp(lapse.obj.transform.rotation, lapse.timeArray[lapse.currentLapse].rot, 0.01f);

                    if (Vector3.Distance(lapse.obj.transform.position, lapse.timeArray[lapse.currentLapse].pos) < 1)
                    {
                        lapse.currentLapse = lapse.currentLapse + 1;
                    }

                    if (lapse.currentLapse > (lapse.timeArray.Count - 1))
                    {
                        lapse.nextLapse = 0;
                        lapse.currentLapse = 0;
                        lapse.timeArray.Clear();

                        VecRot a = new VecRot();
                        a.pos = lapse.obj.transform.position;
                        a.rot = lapse.obj.transform.rotation;
                        lapse.timeArray.Add(a);

                        lapse.obj.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }

            if (Time.time >= nextLapse)
            {
                for (int i = 0; i < lapses.Count; i++)
                {
                    TimeLapse lapse = lapses[i];
                    lapse.nextLapse = 0;
                    lapse.currentLapse = 0;
                    lapse.timeArray.Clear();
                    VecRot a = new VecRot();
                    a.pos = lapse.obj.transform.position;
                    a.rot = lapse.obj.transform.rotation;
                    lapse.timeArray.Add(a);

                    lapse.obj.GetComponent<Rigidbody>().isKinematic = false;
                }

                recalling = false;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GameObject t = Instantiate(ball, transform.position, transform.rotation, null);
                t.transform.LookAt(towards);

                t.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
            }

            Quaternion q = transform.rotation;

            if (Input.GetKey(KeyCode.A)) q.eulerAngles = new Vector3(q.x, q.eulerAngles.y - 0.1f, q.z);
            else if (Input.GetKey(KeyCode.D)) q.eulerAngles = new Vector3(q.x, q.eulerAngles.y + 0.1f, q.z);

            transform.rotation = q;


            if (Time.time >= nextLapse)
            {
                nextLapse = Time.time + 1;

                foreach (TimeLapse lapse in lapses)
                {
                    VecRot a = new VecRot();
                    a.pos = lapse.obj.transform.position;
                    a.rot = lapse.obj.transform.rotation;
                    lapse.timeArray.Add(a);
                    Debug.Log(lapse.obj + " Added lapse. " + lapse.timeArray.Count);
                }
            }
        }
    }
}
