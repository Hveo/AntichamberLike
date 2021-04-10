using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimateEyeShader : MonoBehaviour
{
    Material m_EyeMat;


    private void Start()
    {
        m_EyeMat = GetComponent<Renderer>().material;
        StartCoroutine(Blink());
        StartCoroutine(Look());
    }

    IEnumerator Blink()
    {
        bool close = true;

        float BlinkValue = m_EyeMat.GetFloat("_Blink");

        while (true)
        {
            if (close)
            {
                while (BlinkValue < 1.0f)
                {
                    BlinkValue = Mathf.MoveTowards(BlinkValue, 1.0f, Time.deltaTime * 5.0f);
                    m_EyeMat.SetFloat("_Blink", BlinkValue);
                    yield return null;
                }

                close = false;
                yield return new WaitForSeconds(0.3f);
            }

            if (!close)
            {
                while (BlinkValue > 0.0f)
                {
                    BlinkValue = Mathf.MoveTowards(BlinkValue, 0.0f, Time.deltaTime * 3.0f);
                    m_EyeMat.SetFloat("_Blink", BlinkValue);
                    yield return null;
                }

                close = true;
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    IEnumerator Look()
    {
        bool lookUp = true;

        Vector4 offsetValue = m_EyeMat.GetVector("_PupilOffset");

        while (true)
        {
            if (lookUp)
            {
                while (offsetValue.y < 0.15f)
                {
                    offsetValue.y = Mathf.MoveTowards(offsetValue.y, 0.15f, Time.deltaTime * 0.8f);
                    m_EyeMat.SetVector("_PupilOffset", new Vector4(0.0f, offsetValue.y, 0.0f, 0.0f));
                    yield return null;
                }

                lookUp = false;
                yield return new WaitForSeconds(1.0f);
            }

            if (!lookUp)
            {
                while (offsetValue.y > 0.0f)
                {
                    offsetValue.y = Mathf.MoveTowards(offsetValue.y, 0.0f, Time.deltaTime * 0.8f);
                    m_EyeMat.SetVector("_PupilOffset", new Vector4(0.0f, offsetValue.y, 0.0f, 0.0f));
                    yield return null;
                }

                lookUp = true;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
