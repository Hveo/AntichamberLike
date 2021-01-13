using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeParentingPosition : MonoBehaviour
{
    public GameObject ParentToFake;

    Vector3 positionOffset;
    // Start is called before the first frame update
    void Start()
    {
        positionOffset = ParentToFake.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ParentToFake.transform.position - positionOffset;
    }
}
