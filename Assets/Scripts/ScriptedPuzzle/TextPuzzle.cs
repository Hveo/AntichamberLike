using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TextPuzzle : MonoBehaviour
{
    public Door DoorToOpen;

    string m_RealText;
    TextMesh m_TextMesh;
    Material m_TextMat;
    bool m_IsSwitch = false;
    bool m_Toggled = false;

    private void Start()
    {
        m_RealText = "Hello " + System.Environment.UserName;
        m_TextMesh = GetComponent<TextMesh>();
        m_TextMesh.text = m_RealText;
        m_TextMesh.text = new string(m_RealText.ToCharArray().OrderBy(x => Random.value).ToArray());
        m_TextMat = GetComponent<Renderer>().material;
    }

    private void OnBecameInvisible()
    {
        if (!m_IsSwitch)
        {
            m_IsSwitch = true;
            m_TextMesh.text = m_RealText;
        }
    }

    private void OnBecameVisible()
    {
        if (m_IsSwitch && !m_Toggled)
            StartCoroutine(SwapColor());
    }

    IEnumerator SwapColor()
    {
        m_Toggled = true;
        float Timer = 2.0f;

        while (Timer > 0.0f)
        {
            //The 3D Shader is kind of annoying when this is about swapping color so I'm changing both to make it visible

            Color swapColor = m_TextMat.GetColor("_Color");
            swapColor = Color.Lerp(m_TextMesh.color, Color.blue, Time.deltaTime);
            m_TextMat.SetColor("_Color", swapColor);
            m_TextMesh.color = swapColor;

            Timer -= Time.deltaTime;

            yield return null;
        }

        if (DoorToOpen != null)
        {
            DoorToOpen.Unlock();

            yield return new WaitForSeconds(1.0f);

            DoorToOpen.Open();
        }
    }
}
