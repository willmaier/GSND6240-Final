using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public void Interact()
    {
        //Will hopefully optimize later
        if (GetComponent<DoorController>())
        {
            DoorController DCon = GetComponent<DoorController>();
            DCon.OnInteract();
        }
        else if (GetComponent<TeleporterController>())
        {
            TeleporterController TPCon = GetComponent<TeleporterController>();
            TPCon.OnInteract();
        }
    }
}
