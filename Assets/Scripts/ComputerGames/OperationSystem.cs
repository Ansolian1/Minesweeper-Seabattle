using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationSystem : MonoBehaviour
{
    public void ShowGameWindow(GameObject window)
    {
        window.SetActive(true);
    }

    public void HideGameWindow(GameObject window)
    {
        window.SetActive(false);
    }
}
