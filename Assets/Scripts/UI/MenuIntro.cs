using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuIntro : MonoBehaviour
{
    public float FadeTime;
    public Image PanelImage;
    public GameObject ButtonLayout;
    
    Canvas m_Canvas;
    // Start is called before the first frame update
    void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        StartCoroutine(FadeAndEnableMenu());
    }

    IEnumerator FadeAndEnableMenu()
    {
        Color transp = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        float rate = 1.0f / FadeTime;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            PanelImage.color = Color.Lerp(Color.black, transp, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }
}
