using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform Destination;
    public CharacterController player;
    bool TeleportPlayer;
    Vector3 previousOffset;
    //Transform ParentObject;

    private void Start()
    {
        player = GameMgr.instance.Player;
        //ParentObject = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 portalToPlayer = player.transform.position - transform.position;

        float rotationDiff = Quaternion.Angle(player.transform.rotation, Destination.rotation);
        float YAngle = Vector3.Angle(player.transform.forward, transform.root.forward);

        YAngle *= Vector3.Dot(player.transform.forward, transform.root.right) < 0.0f ? -1.0f : 1.0f;
        rotationDiff += YAngle;

        Vector3 posOffset = Quaternion.Euler(0.0f, rotationDiff, 0.0f) * portalToPlayer;
        player.enabled = false;
        player.transform.rotation = Quaternion.LookRotation(Destination.forward) * Quaternion.AngleAxis(YAngle, Vector3.up);//Rotate(Vector3.up, rotationDiff);
        player.transform.position = Destination.position + posOffset;
        player.enabled = true;

        //previousOffset = player.transform.position - ParentObject.position;
        //TeleportPlayer = true;
    }

    private void LateUpdate()
    {
        //if (!TeleportPlayer)
        //    return;

        //var m = Destination.localToWorldMatrix * ParentObject.worldToLocalMatrix * player.transform.localToWorldMatrix;

        //Vector3 offsetFromPortal = player.transform.position - ParentObject.position;
        //int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, ParentObject.forward));
        //int portalSideOld = System.Math.Sign(Vector3.Dot(previousOffset, ParentObject.forward));

        //if (portalSide == portalSideOld)
        //{
        //    player.enabled = false;
        //    Vector3 oldPos = player.transform.position;
        //    Quaternion oldRot = player.transform.rotation;

        //    player.transform.position = m.GetColumn(3);
        //    player.transform.rotation = m.rotation;
        //    player.enabled = true;
        //}
        //else
        //{
        //    previousOffset = offsetFromPortal;
        //}

        //TeleportPlayer = false;
    }
}
