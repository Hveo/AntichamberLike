using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FractalExplorer : MonoBehaviour
{
    public Material Mat;
    public Vector2 Position;
    public float Scale;
    public float Angle;

    private float m_AspectRatio;

    private void Start()
    {
        m_AspectRatio = (float)transform.localScale.x / (float)transform.localScale.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
            Scale *= 0.99f;
        if (Input.GetKey(KeyCode.KeypadMinus))
            Scale *= 1.01f;

        Vector2 dir = new Vector2(0.01f * Scale, 0);
        float s = Mathf.Sin(Angle);
        float c = Mathf.Cos(Angle);
        dir = new Vector2(dir.x * c, dir.x * s);

        if (Input.GetKey(KeyCode.Keypad4))
            Position += dir;
        if (Input.GetKey(KeyCode.Keypad6))
            Position -= dir;

        dir = new Vector2(-dir.y, dir.x);

        if (Input.GetKey(KeyCode.Keypad8))
            Position -= dir;
        if (Input.GetKey(KeyCode.Keypad5))
            Position += dir;
        
        float scaleX = Scale;
        float scaleY = Scale;

        if (m_AspectRatio > 1.0f)
            scaleY /= m_AspectRatio;
        else
            scaleX *= m_AspectRatio;

        Mat.SetVector("_Area", new Vector4(Position.x, Position.y, scaleX, scaleY));
        Mat.SetFloat("_Angle", Angle);
    }
}
