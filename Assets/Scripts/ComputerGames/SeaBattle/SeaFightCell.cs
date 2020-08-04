using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFightCell : MonoBehaviour
{
    public bool isEnemyCell = false;
    public void CellClick()
    {
        if (isEnemyCell)
        {
            transform.parent.transform.parent.GetComponent<SeaFight>().ClickOnEnemyCell(transform.GetSiblingIndex());
        }
        else
        {
            transform.parent.transform.parent.GetComponent<SeaFight>().ClickOnPlayerCell(transform.GetSiblingIndex());
        }
    }

    public void CellEnter()
    {
        transform.parent.transform.parent.GetComponent<SeaFight>().EnterOnCell(transform.GetSiblingIndex());
    }
    public void CellExit()
    {
        transform.parent.transform.parent.GetComponent<SeaFight>().ExitOnCell(transform.GetSiblingIndex());
    }
}
