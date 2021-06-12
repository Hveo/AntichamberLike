using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public static class InterfaceUtilities
{
    public enum AnchorPreset
    {
        TOPLEFT,
        TOP,
        TOPRIGHT,
        LEFT,
        CENTER,
        RIGHT,
        BOTTOMLEFT,
        BOTTOM,
        BOTTOMRIGHT,
    }

    public static GameObject CanvasInstance;
    static Canvas m_Canvas;
    static GameObject m_CanvasResource;

    public static IEnumerator InitResources()
    {
        ResourceRequest res = Resources.LoadAsync("UI/StandardCanvas");

        while (!res.isDone)
            yield return null;

        m_CanvasResource = res.asset as GameObject;
    }

    static void EnsureInterfacePresence()
    {
        if (CanvasInstance == null)
            CanvasInstance = GameObject.Instantiate(m_CanvasResource);

        m_Canvas = CanvasInstance.GetComponent<Canvas>();
    }

    public static void AddCaption(string content, Vector2 size, float fontSize, Color txtColor, bool fade, bool revertFade, float fadeTime, AnchorPreset preset = AnchorPreset.CENTER)
    {
        EnsureInterfacePresence();

        GameObject TxtObject = new GameObject("Caption");
        TxtObject.transform.SetParent(CanvasInstance.transform);
        RectTransform rect = TxtObject.AddComponent<RectTransform>();
        Vector2 Placement = new Vector2(0.0f, 0.0f);
        
        TextMeshProUGUI tmp = TxtObject.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = fontSize;
        tmp.enableAutoSizing = true;
        tmp.color = txtColor;
        rect.sizeDelta = size;
        
        //Adapt anchor for the given preset
        switch (preset)
        {
            case AnchorPreset.TOPLEFT:
            {
                rect.anchorMin = new Vector2(0.0f, 1.0f);
                rect.anchorMax = new Vector2(0.0f, 1.0f);
                Placement = new Vector2(rect.sizeDelta.x * 0.55f, -(rect.sizeDelta.y * 1.05f)); 
                break;
            }
            case AnchorPreset.TOP:
            {
                rect.anchorMin = new Vector2(0.5f, 1.0f);
                rect.anchorMax = new Vector2(0.5f, 1.0f);
                Placement = new Vector2(0.0f, -(rect.sizeDelta.y * 1.05f));
                break;
            }
            case AnchorPreset.TOPRIGHT:
            {
                rect.anchorMin = new Vector2(1.0f, 1.0f);
                rect.anchorMax = new Vector2(1.0f, 1.0f);
                Placement = new Vector2(-(rect.sizeDelta.x * 0.55f), -(rect.sizeDelta.y * 1.05f));
                break;
            }
            case AnchorPreset.LEFT:
            {
                rect.anchorMin = new Vector2(0.0f, 0.5f);
                rect.anchorMax = new Vector2(0.0f, 0.5f);
                Placement = new Vector2(rect.sizeDelta.x * 0.55f, 0.0f);
                break;
            }
            case AnchorPreset.CENTER:
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                Placement = new Vector2(0.0f, 0.0f);
                break;
            }
            case AnchorPreset.RIGHT:
            {
                rect.anchorMin = new Vector2(1.0f, 0.5f);
                rect.anchorMax = new Vector2(1.0f, 0.5f);
                Placement = new Vector2(-(rect.sizeDelta.x * 0.55f), 0.0f);
                break;
            }
            case AnchorPreset.BOTTOMLEFT:
            {
                rect.anchorMin = new Vector2(0.0f, 0.0f);
                rect.anchorMax = new Vector2(0.0f, 0.0f);
                Placement = new Vector2(rect.sizeDelta.x * 0.55f, rect.sizeDelta.y * 0.55f);
                break;
            }
            case AnchorPreset.BOTTOM:
            {
                rect.anchorMin = new Vector2(0.5f, 0.0f);
                rect.anchorMax = new Vector2(0.5f, 0.0f);
                Placement = new Vector2(0.0f, rect.sizeDelta.y * 0.55f);
                break;
            }
            case AnchorPreset.BOTTOMRIGHT:
            {
                rect.anchorMin = new Vector2(1.0f, 0.0f);
                rect.anchorMax = new Vector2(1.0f, 0.0f);
                Placement = new Vector2(-(rect.sizeDelta.x * 0.55f), rect.sizeDelta.y * 0.55f);
                break;
            }
            default: break;
        }

        rect.anchoredPosition = Placement;

        if (fade)
            Core.instance.StartCoroutine(InitiateFade(tmp, revertFade, fadeTime));
    }

    public static void FadeToBlack(bool Revert, float time)
    {
        EnsureInterfacePresence();

        GameObject ImgObject = new GameObject("BlackScreen");
        ImgObject.transform.SetParent(CanvasInstance.transform);
        RectTransform rect = ImgObject.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0.5f, 0.5f);
        rect.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        rect.sizeDelta = m_Canvas.pixelRect.size;

        Image img = ImgObject.AddComponent<Image>();

        if (!Revert)
            img.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        else
            img.color = Color.black;

        Core.instance.StartCoroutine(InitiateFade(img, Revert, time));
    }

    static IEnumerator InitiateFade(Image img, bool revert, float time)
    {
        Color goal;

        if (revert)
            goal = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        else
            goal = Color.black;

        float rate = 1.0f / time;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            img.color = Color.Lerp(img.color, goal, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }
    }

    static IEnumerator InitiateFade(TextMeshProUGUI txt, bool revert, float time)
    {
        float goal = 1.0f;

        if (revert)
            goal = 0.0f;

        float rate = 1.0f / time;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            txt.alpha = Mathf.Lerp(txt.alpha, goal, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

    }
}
