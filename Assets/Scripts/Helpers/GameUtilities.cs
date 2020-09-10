using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtilities
{
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
}
