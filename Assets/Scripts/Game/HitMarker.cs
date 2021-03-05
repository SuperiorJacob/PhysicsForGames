using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public int damage = 0;
    public float fadeAfter = 1.0f;
    public float fadeTime = 1.0f;
   
    private Text text;
    private float startTime;
    private Vector3 go;

    public Transform faceTowards;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = damage.ToString();

        startTime = Time.time;

        go = transform.position + Vector3.up * 2;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - faceTowards.position);

        if (Time.time > (startTime + fadeAfter))
        {
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), fadeTime * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, go, fadeTime * Time.deltaTime);

            if (fadeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
