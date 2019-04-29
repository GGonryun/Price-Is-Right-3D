using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour
{
    public GameObject reciever;

    public void SendToReciever(string methodName)
    {
        reciever.SendMessage(name);
    }
}
