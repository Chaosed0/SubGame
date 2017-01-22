using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActionSuccess : MonoBehaviour
{

    public UnityEvent OnSuccessEvents;

    public void OnSuccess()
    {
        OnSuccessEvents.Invoke();
    }
}
