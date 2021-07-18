using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCommands : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(DisplayTutorial());
    }

    IEnumerator DisplayTutorial()
    {
        yield return new WaitForSeconds(1.0f);
        InterfaceUtilities.DisplayAction("inputs.look");
        yield return new WaitForSeconds(5.0f);
        InterfaceUtilities.Clear();
        yield return null;
        
        Transform player = LevelMgr.instance.Player.transform;
        Vector3 pos = player.position;

        InterfaceUtilities.DisplayAction("inputs.move", true);       

        while (pos == player.position)
            yield return null;

        yield return new WaitForSeconds(2.0f);
        InterfaceUtilities.Clear();
        Destroy(this);
    }
}
