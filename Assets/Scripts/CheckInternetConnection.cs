using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternetConnection : MonoBehaviour
{
    

    public bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
