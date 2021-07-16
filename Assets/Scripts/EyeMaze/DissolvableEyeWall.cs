using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvableEyeWall : MonoBehaviour
{
    Coroutine runningCrt;

    public void DissolveWall(bool Disappear)
    {
        if (runningCrt != null)
            StopCoroutine(runningCrt);

        runningCrt = StartCoroutine(DissolveRoutine(Disappear));  
    }

    IEnumerator DissolveRoutine(bool Disappear)
    {
        MeshRenderer rend = transform.GetChild(0).GetComponent<MeshRenderer>();

        Color col = rend.material.GetColor("_Color");

        if (Disappear)
        {
            float alphaGoal = 0.0f;

            while (col.a > alphaGoal)
            {
                col.a = Mathf.MoveTowards(col.a, alphaGoal, Time.deltaTime * 2.0f);
                rend.material.SetColor("_Color", col);
                yield return null;
            }

            yield return GameUtilities.DissolveMesh(GetComponent<Renderer>(), Disappear, 0.75f);
        }
        else
        {
            yield return GameUtilities.DissolveMesh(GetComponent<Renderer>(), Disappear, 0.75f);

            float alphaGoal = 1.0f;

            while (col.a < alphaGoal)
            {
                col.a = Mathf.MoveTowards(col.a, alphaGoal, Time.deltaTime * 2.0f);
                rend.material.SetColor("_Color", col);
                yield return null;
            }
        }

        GetComponent<Collider>().enabled = !Disappear;
    }
}
