using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLookAtPosition : MonoBehaviour
{
    Material m_EyeMat;

    public void Awake()
    {
        if (m_EyeMat == null)
            m_EyeMat = GetComponent<Renderer>().material;
    }

    public void LookAtObject(GameObject obj)
    {
        Vector3 dir = (obj.transform.position - transform.position).normalized;
        Vector2 lookPoint;

        if (transform.forward == Vector3.forward)
        {
            lookPoint = new Vector2(dir.x, -dir.z);
        }
        else if (transform.right == Vector3.left)
        {
            if (dir.z < -0.9f || dir.z > 0.9f)
                dir.x = 0.0f;

            lookPoint = new Vector2(dir.x, dir.y);
        }
        else if (transform.right == Vector3.forward)
        {
            if (dir.x < -0.9f || dir.x > 0.9f)
                dir.x = 0.0f;

            lookPoint = new Vector2(-dir.z, dir.y);
        }
        else
        {
            if (dir.x < -0.9f || dir.x > 0.9f)
                dir.x = 0.0f;

            lookPoint = new Vector2(dir.z, dir.x);
        }

        StartCoroutine(SmoothLookAtObject(Mathf.Clamp(lookPoint.x, -0.17f, 0.17f), Mathf.Clamp(lookPoint.y, -0.17f, 0.17f)));
    }

    IEnumerator SmoothLookAtObject(float xVal, float yVal)
    {
        Vector4 offsetValue = m_EyeMat.GetVector("_PupilOffset");

        while (offsetValue.x != xVal || offsetValue.y != yVal)
        {
            offsetValue.x = Mathf.MoveTowards(offsetValue.x, xVal, Time.deltaTime * 0.8f);
            offsetValue.y = Mathf.MoveTowards(offsetValue.y, yVal, Time.deltaTime * 0.8f);
            m_EyeMat.SetVector("_PupilOffset", offsetValue);
            yield return null;
        }
    }
}
