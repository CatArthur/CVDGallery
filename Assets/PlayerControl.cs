using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Camera camera;
    public Transform forward;
    public Transform right;
    public float moveSpeed = 10f;
    public float horizontalRotateSpeed = 10f;
    public float verticalRotateSpeed = 10f;
    private const float maxAngle = 70;
    private CVD cvd;
    public Text anomalyName;
    private Rigidbody rb;
    public Color textColor;
    public float fadingSpeed = 0.1f;
    private string key = "4518161818";
    private Queue<int> seq = new Queue<int>(new int[]{0,0,0,0,0,0,0,0,0,0});

    private void Awake()
    {
        camera.GetComponent<PostProcessVolume>().profile.TryGetSettings(out cvd);
        rb = GetComponent<Rigidbody>();
        ChangeAnomaly(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown("0"))
            ChangeAnomaly(0);
        if (Input.GetKeyDown("1"))
            ChangeAnomaly(1);
        if (Input.GetKeyDown("2"))
            ChangeAnomaly(2);
        if (Input.GetKeyDown("3"))
            ChangeAnomaly(3);
        if (Input.GetKeyDown("4"))
            ChangeAnomaly(4);
        if (Input.GetKeyDown("5"))
            ChangeAnomaly(5);
        if (Input.GetKeyDown("6"))
            ChangeAnomaly(6);
        if (Input.GetKeyDown("7"))
            ChangeAnomaly(7);
        if (Input.GetKeyDown("8"))
            ChangeAnomaly(8);
        if (Input.GetMouseButtonDown(0))
            ChangeAnomaly(-1);
        if (Input.GetMouseButtonDown(1))
            ChangeAnomaly(-2);
        
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var direction = v * forward.position + h * right.position - (v + h) * transform.position;
        transform.position += direction * moveSpeed * Time.deltaTime;
        rb.velocity = Vector3.zero;

        transform.Rotate(0,Input.GetAxis("Mouse X") * horizontalRotateSpeed,0);
        var euler = camera.transform.localEulerAngles;
        euler.x -= Input.GetAxis("Mouse Y") * verticalRotateSpeed;
        if (euler.x is < 180 and > maxAngle)
            euler.x = maxAngle;
        if (euler.x is > 180 and < 360 - maxAngle)
            euler.x = 360 - maxAngle;
        camera.transform.localEulerAngles = euler;

        if (textColor.a > 0)
        {
            textColor.a -= fadingSpeed * Time.deltaTime;
            anomalyName.color = textColor;
        }
    }

    private void ChangeAnomaly(int anomaly)
    {
        switch (anomaly)
        {
            case -1:
                cvd.NextAnomaly();
                break;
            case -2:
                cvd.PrevAnomaly();
                break;
            default:
                cvd.ChangeAnomaly(anomaly);
                seq.Enqueue(anomaly);
                seq.Dequeue();
                break;
        }

        anomalyName.text = cvd.AnomalyName();
        textColor.a = 2;
        anomalyName.color = textColor;
        if (string.Join("", seq) != key) return;
        anomalyName.text = "Despair";
        transform.position = new Vector3(21.2f, 1, 9.92f);
        textColor.a = 2;
        anomalyName.color = textColor;
    }
}
