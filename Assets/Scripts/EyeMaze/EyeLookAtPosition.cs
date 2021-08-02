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
        Vector3 lookAt = (0.20f * dir); //calculating position for eye shader iris

        StartCoroutine(SmoothLookAtObject(lookAt.x, lookAt.y));
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
