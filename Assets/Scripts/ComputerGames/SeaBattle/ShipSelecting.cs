using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelecting : MonoBehaviour
{
    public SeaFight seaFight;

    public void ShipPanelClick(int shipLength)
    {
        seaFight.UpdateShipLength(shipLength);
    }
}
