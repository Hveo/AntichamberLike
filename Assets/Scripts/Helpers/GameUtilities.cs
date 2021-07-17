using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public static class GameUtilities
{
    private static GameObject BoxHelpRes;
    private static GameObject BoxHelper;


    public static bool HelperPresence { get; private set; }
    public static Transform HelperTransform { get { return BoxHelper.transform; } private set { } }

    public static void Init()
    {
        BoxHelpRes = Resources.Load<GameObject>("BoxHelper");
    }

    public static bool IsPositionBetweenAB(Vector3 A, Vector3 B, Vector3 PosToTest)
    {
        Vector2 A2D = new Vector2(A.x, A.z);
        Vector2 B2D = new Vector2(B.x, B.z);
        Vector2 C2D = new Vector2(PosToTest.x, PosToTest.z);

        return (Vector2.Dot((B2D - A2D).normalized, (C2D - B2D).normalized) < 0.0f && Vector3.Dot((A2D - B2D).normalized, (C2D - A2D).normalized) < 0.0f);
    }

    public static IEnumerator DissolveMesh(Renderer Rend, bool Disappear, float Speed)
    {
        if (Rend == null || Rend.material == null || !Rend.material.HasProperty("_Amount"))
            yield break;

        float Amount = Rend.material.GetFloat("_Amount");
        float Goal = Disappear ? 1.0f : 0.0f;

        while (Disappear ? Amount < Goal : Amount > Goal)
        {
            Amount = Mathf.MoveTowards(Amount, Goal, Speed * Time.deltaTime);
            Rend.material.SetFloat("_Amount", Amount);
            yield return null;
        }
    }

    public static void TeleportPlayerTo(Vector3 Position, Vector3 forward = default(Vector3))
    {
        LevelMgr.instance.Player.transform.position = Position;
        LevelMgr.instance.Player.transform.rotation = Quaternion.LookRotation(forward);
    }

    public static void DisplayBoxHelper(Vector3 Pos, Quaternion Rot)
    {
        if (BoxHelper == null)
            BoxHelper = GameObject.Instantiate(BoxHelpRes, Pos, Rot);
        else
        {
            BoxHelper.SetActive(true);
            BoxHelper.transform.position = Pos;
            BoxHelper.transform.rotation = Rot;
        }

        HelperPresence = true;
    }

    public static void HideBoxHelper()
    {
        if (BoxHelper != null)
            BoxHelper.SetActive(false);

        HelperPresence = false;
    }
}
