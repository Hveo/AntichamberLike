using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class MenuIntro : MonoBehaviour
{
    public float FadeTime;
    public AudioClip MainMenuTheme;
    public Image PanelImage;
    public GameObject ButtonLayout;
    public GameObject Generator;
    public TextMeshProUGUI[] Texts;
    public MainMenuHandler MainMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeAndEnableMenu());
    }

    IEnumerator FadeAndEnableMenu()
    {
        AudioSource src = Generator.GetComponent<AudioSource>();

        Color transp = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        float rate = 1.0f / FadeTime;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            PanelImage.color = Color.Lerp(Color.black, transp, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }


        LeanTween.value(0.0f, 10.0f, 1.0f).setEaseInOutBack().setLoopPingPong(1).setOnUpdate(setIntensity);
        yield return new WaitForSeconds(0.5f);
        src.Play();

        GameObject cam = Camera.main.gameObject;
        LeanTween.moveX(cam, 20, 1.5f).setDelay(1.0f);
        LeanTween.moveY(cam, -10, 1.5f).setDelay(1.0f);
        LeanTween.moveZ(cam, -20, 1.5f).setDelay(1.0f);

        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < Texts.Length; ++i)
        {
            RectTransform rect = Texts[i].GetComponent<RectTransform>();
            LeanTween.moveX(rect, 0.0f, 1.0f).setFrom(100.0f);
        }
        LeanTween.value(0.0f, 1.0f, 1.0f).setOnUpdate(setAlpha);

        yield return new WaitForSeconds(1.0f);

        MainMenu.EnableInput();
        UISystem.instance.NewFocusedWindow(MainMenu.gameObject);
        
        AudioMgr.PlayMusic(MainMenuTheme, true);
        yield return StartCoroutine(AudioMgr.FadeIn(Time.deltaTime * 0.5f, 1.0f));
    }

    public void setIntensity(float intensity)
    {
        Generator.GetComponent<Light>().intensity = intensity;
    }

    public void setAlpha(float a)
    {
        Color col = Texts[0].color;
        for (int i = 0; i < Texts.Length; ++i)
        {
            Texts[i].color = new Color(col.r, col.g, col.b, a);
        }
    }
}
