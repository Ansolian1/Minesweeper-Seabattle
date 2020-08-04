using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaperCell : MonoBehaviour
{
    public void CellClick()
    {
        transform.parent.GetComponent<Saper>().ClickOnCell(transform.GetSiblingIndex());
    }
}
