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

    public static void Clear()
    {
        GameObject.Destroy(CanvasInstance);
        m_Canvas = null;
    }

    static void EnsureInterfacePresence()
    {
        if (CanvasInstance == null)
            CanvasInstance = GameObject.Instantiate(m_CanvasResource);

        m_Canvas = CanvasInstance.GetComponent<Canvas>();
    }

    static Vector2 AdaptUIElementToAnchor(AnchorPreset preset, RectTransform rect)
    {
        //Adapt anchor for the given preset
        switch (preset)
        {
            case AnchorPreset.TOPLEFT:
            {
                rect.anchorMin = new Vector2(0.0f, 1.0f);
                rect.anchorMax = new Vector2(0.0f, 1.0f);
                return new Vector2(rect.sizeDelta.x * 0.55f, -(rect.sizeDelta.y * 1.05f));
            }
            case AnchorPreset.TOP:
            {
                rect.anchorMin = new Vector2(0.5f, 1.0f);
                rect.anchorMax = new Vector2(0.5f, 1.0f);
                return new Vector2(0.0f, -(rect.sizeDelta.y * 1.05f));
            }
            case AnchorPreset.TOPRIGHT:
            {
                rect.anchorMin = new Vector2(1.0f, 1.0f);
                rect.anchorMax = new Vector2(1.0f, 1.0f);
                return new Vector2(-(rect.sizeDelta.x * 0.55f), -(rect.sizeDelta.y * 1.05f));
            }
            case AnchorPreset.LEFT:
            {
                rect.anchorMin = new Vector2(0.0f, 0.5f);
                rect.anchorMax = new Vector2(0.0f, 0.5f);
                return new Vector2(rect.sizeDelta.x * 0.55f, 0.0f);
            }
            case AnchorPreset.CENTER:
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                return new Vector2(0.0f, 0.0f);
            }
            case AnchorPreset.RIGHT:
            {
                rect.anchorMin = new Vector2(1.0f, 0.5f);
                rect.anchorMax = new Vector2(1.0f, 0.5f);
                return new Vector2(-(rect.sizeDelta.x * 0.55f), 0.0f);
            }
            case AnchorPreset.BOTTOMLEFT:
            {
                rect.anchorMin = new Vector2(0.0f, 0.0f);
                rect.anchorMax = new Vector2(0.0f, 0.0f);
                return new Vector2(rect.sizeDelta.x * 0.55f, rect.sizeDelta.y * 0.55f);
            }
            case AnchorPreset.BOTTOM:
            {
                rect.anchorMin = new Vector2(0.5f, 0.0f);
                rect.anchorMax = new Vector2(0.5f, 0.0f);
                return new Vector2(0.0f, rect.sizeDelta.y * 0.55f);
            }
            case AnchorPreset.BOTTOMRIGHT:
            {
                rect.anchorMin = new Vector2(1.0f, 0.0f);
                rect.anchorMax = new Vector2(1.0f, 0.0f);
                return new Vector2(-(rect.sizeDelta.x * 0.55f), rect.sizeDelta.y * 0.55f);
            }
            default: return Vector2.zero;
        }
    }

    public static void AddCaption(string content, Vector2 size, float fontSize, Color txtColor, bool fade, bool revertFade, float fadeTime, AnchorPreset preset = AnchorPreset.CENTER)
    {
        EnsureInterfacePresence();

        GameObject TxtObject = new GameObject("Caption");
        TxtObject.transform.SetParent(CanvasInstance.transform);
        RectTransform rect = TxtObject.AddComponent<RectTransform>();
        
        TextMeshProUGUI tmp = TxtObject.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = fontSize;
        tmp.enableAutoSizing = true;
        tmp.color = txtColor;
        rect.sizeDelta = size;
        
        rect.anchoredPosition = AdaptUIElementToAnchor(preset, rect);

        if (fade)
            Core.instance.StartCoroutine(InitiateFade(tmp, revertFade, fadeTime));
    }

    public static Image AddSprite(Sprite spr, Vector2 size, float xOffset = 0.0f, float yOffset = 0.0f, AnchorPreset preset = AnchorPreset.CENTER)
    {
        EnsureInterfacePresence();

        GameObject ImgObj = new GameObject("Sprite");
        ImgObj.transform.SetParent(CanvasInstance.transform);
        RectTransform rect = ImgObj.AddComponent<RectTransform>();
        rect.anchoredPosition = AdaptUIElementToAnchor(preset, rect);
        rect.sizeDelta = size;
        rect.position += new Vector3(xOffset, yOffset, 0.0f);
        Image img = ImgObj.AddComponent<Image>();
        img.sprite = spr;
        return img; 
    }

    public static void DisplayAction(string locaEntry, bool isComposite = false)
    {
        AddCaption(LocalizationSystem.GetEntry(locaEntry), new Vector2(300.0f, 40.0f), 15.0f, Color.white, false, false, -1.0f, AnchorPreset.BOTTOM);

        if (isComposite)
        {         
            Sprite[] sprites = InputHandler.GetIconsForComposite(locaEntry, true);
            GameObject Holder = new GameObject("SpritesGroup");
            RectTransform rect = Holder.AddComponent<RectTransform>();
            rect.anchoredPosition = AdaptUIElementToAnchor(AnchorPreset.BOTTOM, rect);
            rect.sizeDelta = new Vector2(70.0f * sprites.Length, 70.0f);
            rect.position = new Vector3(Screen.width / 2.0f, 70.0f, 0.0f);

            for (int i = 0; i < sprites.Length; ++i)
            {
                Image img = AddSprite(sprites[i], new Vector2(70.0f, 70.0f), 0.0f, 0.0f, AnchorPreset.BOTTOM);
                Holder.transform.parent = img.transform.parent;
                img.transform.parent = Holder.transform;
            }

            Holder.AddComponent<HorizontalLayoutGroup>();
            InputActionCompositeDisplay display = Holder.AddComponent<InputActionCompositeDisplay>();
            display.ActionName = locaEntry;
        }
        else
        {
            Image img = AddSprite(InputHandler.GetIconForAction(locaEntry), new Vector2(70.0f, 70.0f), 0.0f, 20.0f, AnchorPreset.BOTTOM);
            InputActionDisplay actionDisplay = img.gameObject.AddComponent<InputActionDisplay>();
            actionDisplay.ActionName = locaEntry;
        }

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
