using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour
{
    public void OnPushEndButton()
    {
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        UISystem.instance.SetMenuPresence(true);
        InterfaceUtilities.FadeToBlack(false, 2.0f);
        yield return new WaitForSeconds(3.0f);
        InterfaceUtilities.AddCaption(LocalizationSystem.GetEntry("end.thanks"), new Vector2(400.0f, 100.0f), 40.0f, Color.white, true, false, 2.0f);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }
}
