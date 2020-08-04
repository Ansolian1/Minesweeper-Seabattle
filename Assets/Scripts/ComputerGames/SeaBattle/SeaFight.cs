using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeaFight : MonoBehaviour
{
    [SerializeField]
    private Sprite ship;
    [SerializeField]
    private Sprite brokenShip;
    [SerializeField]
    private Sprite emptyCell;

    [SerializeField]
    private GameObject seaFightCell;
    [SerializeField]
    private Text information;
    [SerializeField]
    private Button readyButton;
    [SerializeField]
    private GameObject playerFieldObject;
    [SerializeField]
    private GameObject enemyFieldObject;
    [SerializeField]
    private char[] playerField;
    [SerializeField]
    private char[] markedPlayerField;
    [SerializeField]
    private char[] enemyField;
    [SerializeField]
    private GameObject shipsPanel;

    public bool isHorisontal = true;
    public bool isPositingMode = true;
    public int shipLength = 3;
    public int oneShipCount = 4;
    public int twoShipCount = 4;
    public int threeShipCount = 4;
    public int fourShipCount = 4;

    [SerializeField]
    private Text oneCount;
    [SerializeField]
    private Text twoCount;
    [SerializeField]
    private Text threeCount;
    [SerializeField]
    private Text fourCount;

    int endPosition = 0;
    bool isEndedGame = false;

    void Start()
    {
        Restart();        
    }

    private void CreateSeaFightField(char[] field, GameObject fieldObject)
    {
        for (int i = 0; i < fieldObject.transform.childCount; i++)
        {
            Destroy(fieldObject.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < field.Length; i++)
        {
            GameObject cell = Instantiate(seaFightCell);
            cell.transform.SetParent(fieldObject.transform);
            cell.transform.localScale = new Vector3(1, 1, 1);
            switch (field[i])
            {
                case 's':
                    cell.GetComponent<Image>().sprite = ship;
                    break;
                case 'b':
                    cell.GetComponent<Image>().sprite = brokenShip;
                    break;
                case 'c':
                    cell.GetComponent<Image>().sprite = emptyCell;
                    break;
                default:
                    break;
            }
        }
    }
    private void RepaintSeaFightField(char[] field, GameObject fieldObject)
    {
        for (int i = 0; i < field.Length; i++)
        {
            switch (field[i])
            {
                case 'i':
                    fieldObject.transform.GetChild(i).GetComponent<Image>().color = Color.red;
                    break;
                case 'b':
                    fieldObject.transform.GetChild(i).GetComponent<Image>().sprite = brokenShip;
                    break;
                case 'c':
                    fieldObject.transform.GetChild(i).GetComponent<Image>().sprite = emptyCell;
                    break;
                default:
                    fieldObject.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                    break;
            }
        }
    }


    public void ClickOnPlayerCell(int position)
    {
        if (isPositingMode)
        {
            if (IsCanPlaceShip(position, playerField, shipLength, isHorisontal))
            {
                information.text = "";
                PlaceShip(position, playerField, shipLength, 's', isHorisontal);
                MarkAroundShip(position, playerField, shipLength, isHorisontal, 'm');
                markedPlayerField = new char[100];
                switch (shipLength)
                {
                    case 1:
                        oneShipCount--;
                        break;
                    case 2:
                        twoShipCount--;
                        break;
                    case 3:
                        threeShipCount--;
                        break;
                    case 4:
                        fourShipCount--;
                        break;
                    default:
                        break;
                }
                UpdateShipLength(shipLength);
                UpdateShipCounts();
                CreateSeaFightField(playerField, playerFieldObject);
                ShowReadyButton();
            }
            else
            {
                information.text = "Cant place ship here";
            }
        }        
    }
    public void ClickOnEnemyCell(int position)
    {
        if (!isEndedGame)
        {
            if (enemyField[position] == 'e')
            {
                enemyField[position] = 'b';

                if (IsDestroyed(position, enemyField))
                {
                    bool hor = IsHorisontal(position, enemyField);
                    Debug.Log(hor);
                    if (hor)
                    {
                        MarkAroundShip(DefineStartPosition(position, enemyField), enemyField, endPosition - DefineStartPosition(position, enemyField) + 1, hor, 'c');

                    }
                    else
                    {
                        MarkAroundShip(DefineStartPosition(position, enemyField), enemyField, (endPosition - DefineStartPosition(position, enemyField)) / 10 + 1, hor, 'c');
                    }
                }
            }
            else if (enemyField[position] == ' ' || enemyField[position] == 'm')
            {
                enemyField[position] = 'c';
            }
            RepaintSeaFightField(enemyField, enemyFieldObject);
            IsGameEnded();
            EnemyTurn();
            if (!isEndedGame)
                IsGameEnded();
        }       
    }
    public void EnterOnCell(int position)
    {
        if (isPositingMode){

            PlaceShip(position, markedPlayerField, shipLength, 'i', isHorisontal);
            RepaintSeaFightField(markedPlayerField, playerFieldObject);
        }
    }
    public void ExitOnCell(int position)
    {
        if (isPositingMode)
        {

            PlaceShip(position, markedPlayerField, shipLength, ' ', isHorisontal);
            RepaintSeaFightField(markedPlayerField, playerFieldObject);
        }
    }

    private void IsGameEnded()
    {
        for (int i = 0; i < 100; i++)
        {
            if (playerField[i] == 's')
            {
                break;
            }
            if (i == 99)
            {
                isEndedGame = true;
                information.GetComponent<Text>().text = "You are lose";
            }
        }
        for (int i = 0; i < 100; i++)
        {
            if (enemyField[i] == 'e')
            {
                break;
            }
            if (i == 99)
            {
                isEndedGame = true;
                information.GetComponent<Text>().text = "You are win";
            }
        }
    }
    private void EnemyTurn()
    {
        int position = Random.Range(0, 100);
        while (playerField[position] == 'c' || playerField[position] == 'b')
        {
            position = Random.Range(0, 100);
        }
        if (playerField[position] == 's')
        {
            playerField[position] = 'b';

            if (IsDestroyed(position, playerField))
            {
                bool hor = IsHorisontal(position, playerField);
                Debug.Log(hor);
                if (hor)
                {
                    MarkAroundShip(DefineStartPosition(position, playerField), playerField, endPosition - DefineStartPosition(position, playerField) + 1, hor, 'c');

                }
                else
                {
                    MarkAroundShip(DefineStartPosition(position, playerField), playerField, (endPosition - DefineStartPosition(position, playerField)) / 10 + 1, hor, 'c');
                }
            }

        }
        else if (playerField[position] == ' ' || playerField[position] == 'm')
        {
            playerField[position] = 'c';
        }
        RepaintSeaFightField(playerField, playerFieldObject);
    }
    private bool IsHorisontal(int position, char[] field)
    {
        if (position == 0)
        {
            if (field[position + 1] == 's' || field[position + 1] == 'e' || field[position + 1] == 'b')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (position == 9)
        {
            if (field[position - 1] == 's' || field[position - 1] == 'e' || field[position - 1] == 'b')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (position == 90)
        {
            if (field[position - 10] == 's' || field[position - 10] == 'e' || field[position - 10] == 'b')
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (position == 99)
        {
            if (field[position - 1] == 's' || field[position - 1] == 'e' || field[position - 1] == 'b')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if(position < 10 || position > 89)
        {
            if (field[position + 1] == 's' || field[position + 1] == 'e' || field[position + 1] == 'b' || field[position - 1] == 's' || field[position - 1] == 'e' || field[position - 1] == 'b')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (field[position + 10] == 's' || field[position + 10] == 'e' || field[position + 10] == 'b' || field[position - 10] == 'b' || field[position - 10] == 's' || field[position - 10] == 'e')
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    private int DefineStartPosition(int position, char[] field)
    {
        bool isHorisontal = IsHorisontal(position, field);
        
         if (isHorisontal)
         {
            if (position % 10 == 0)
            {
                return position;
            }
            while (position%10>0)
                {
                    if(field[position] != 'e' && field[position] != 's' && field[position] != 'b')
                    {
                        return ++position;
                    }
                    position--;
                }
            return position;
         }
         else
         {
            if (position < 10)
            {
                return position;
            }
            while (position > 9)
                {
                    if (field[position] != 'e' && field[position] != 's' && field[position] != 'b')
                    {
                        return position+10;
                    }
                    position-=10;
                }
                return position;
         }
    }
    private bool IsDestroyed(int position, char[] field)
    {
        bool isHorisontal = IsHorisontal(position, field);
        int beginPosition = DefineStartPosition(position, field);
        if (isHorisontal)
        {
            while (position % 10 != 9 && field[beginPosition] != 'c' && field[beginPosition] != ' ' && field[beginPosition] != 'm')
            {
                if(field[beginPosition] != 'b')
                {
                    return false;
                }
                beginPosition++;
                if (beginPosition > 99)
                    break;
            }
            endPosition = beginPosition - 1;
            return true;
        }
        else
        {
            while (position <= 99 && field[beginPosition] != 'c' && field[beginPosition] != ' ' && field[beginPosition] != 'm')
            {
                if (field[beginPosition] != 'b')
                {
                    return false;
                }
                beginPosition+=10;
                if (beginPosition > 99)
                    break;
            }
            endPosition = beginPosition - 10;
            return true;
        }
    }
    private int TakeStartPosition(int position, int shipLength, bool isHorisontal)
    {
        if (isHorisontal)
        {
            if ((10 - position % 10) < shipLength)
            {
                int newPosition = position - (shipLength - (10 - position % 10));
                return newPosition;
            }
            else return position;
        }
        else
        {
            if ((10 - position / 10) < shipLength)
            {
                int newPosition = position - 10 * (shipLength - (10 - position / 10));
                return newPosition;
            }
            else return position;
        }
    }
    private void PlaceShip(int position, char[] field, int shipLength, char typeOfCell, bool isHorisontal)
    {
        if (isHorisontal)
        {
            int newPosition = TakeStartPosition(position, shipLength, isHorisontal);
            for (int i = 0; i < shipLength; i++)
            {
                field[newPosition + i] = typeOfCell;
            }
        }
        else
        {
            int newPosition = TakeStartPosition(position, shipLength, isHorisontal);
            for (int i = 0; i < shipLength; i++)
            {
                field[newPosition + i * 10] = typeOfCell;
            }
        }
    }
    private void MarkAroundShip(int position, char[] field, int shipLength, bool isHorisontal, char marker)
    {
        position = TakeStartPosition(position, shipLength, isHorisontal);
        if (isHorisontal)
        {
            for (int i = 0; i < shipLength; i++)
            {
                if (i == 0)
                {
                    if ((position) % 10 > 0)
                    {
                        field[position - 1] = marker;
                    }
                    if ((position) / 10 < 9 && (position) % 10 > 0)
                    {
                        field[position + 9] = marker;
                    }
                    if ((position) / 10 > 0 && (position) % 10 > 0)
                    {
                        field[position - 11] = marker;
                    }

                }
                if ((position + i) / 10 > 0)
                {
                    field[position - 10 + i] = marker;
                }
                if ((position + i) / 10 < 9)
                {
                    field[position + 10 + i] = marker;
                }
                if (i == (shipLength - 1))
                {
                    if ((position + i) % 10 < 9)
                    {
                        field[position + 1 + i] = marker;
                    }
                    if ((position + i) / 10 > 0 && (position + i) % 10 < 9)
                    {
                        field[position - 9 + i] = marker;
                    }
                    if ((position + i) / 10 < 9 && (position + i) % 10 < 9)
                    {
                        field[position + 11 + i] = marker;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < shipLength; i++)
            {
                if (i == 0)
                {
                    if (position / 10 > 0 && position % 10 > 0)
                    {
                        field[position - 11] = marker;
                    }                    
                    if (position / 10 > 0)
                    {
                        field[position - 10] = marker;
                    }
                    if (position / 10 > 0 && position % 10 < 9)
                    {
                        field[position - 9] = marker;
                    }
                }
                if ((position + i * 10) % 10 > 0 && (position + i * 10 < 99))
                {
                    field[position + i * 10 - 1] = marker;
                }
                if ((position + i * 10) % 10 < 9 && (position + i * 10 < 99))
                {
                    field[position + i * 10 + 1] = marker;
                }
                if (i == (shipLength - 1))
                {
                    if (((position + i * 10) / 10) < 9 && (position + i * 10) % 10 > 0)
                    {
                        field[position + 9 + i * 10] = marker;
                    }
                    if (((position + i * 10) / 10) < 9 && (position + i * 10) % 10 < 9)
                    {
                        field[position + 11 + i * 10] = marker;
                    }
                    if (((position + i * 10) / 10) < 9)
                    {
                        field[position + 10 + i * 10] = marker;
                    }
                }
            }
        }
    }
    private bool IsCanPlaceShip(int position, char[] field, int shipLength, bool isHorisontal)
    {
        int newPosition = TakeStartPosition(position, shipLength, isHorisontal);
        if (isHorisontal)
        {
            for (int i = 0; i < shipLength; i++)
            {
                if (field[newPosition + i] != ' ')
                {
                    return false;
                }
            }
        }
        else
        {
            for (int i = 0; i < shipLength; i++)
            {
                if (field[newPosition + i * 10] != ' ')
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void PlaceEnemyShips(char[] field)
    {
        int position;
        bool isHorisontal;
        int shipLength = 4;
        for (int i = 0; i < 10; i++)
        {            
            position = Random.Range(0, 100);
            if (Random.Range(0, 2) == 1)
            {
                isHorisontal = true;
            }
            else
            {
                isHorisontal = false;
            }
            while (!IsCanPlaceShip(position, field, shipLength, isHorisontal))
            {
                position = Random.Range(0, 100);
                if (Random.Range(0, 2) == 1)
                {
                    isHorisontal = true;
                }
                else
                {
                    isHorisontal = false;
                }
            }
            PlaceShip(position, field, shipLength, 'e', isHorisontal);
            MarkAroundShip(position, field, shipLength, isHorisontal, 'm');
            if (i==0 || i == 2 || i == 5)
            {
                shipLength--;
            }
        }
            
    }
    private void MarkFieldAsEnemy(GameObject enemyFieldObject)
    {
        for (int i = 0; i < enemyFieldObject.transform.childCount; i++)
        {
            enemyFieldObject.transform.GetChild(i).gameObject.GetComponent<SeaFightCell>().isEnemyCell = true;
        }
    }

    private void UpdateShipCounts()
    {
        oneCount.text = "x " + oneShipCount;
        twoCount.text = "x " + twoShipCount;
        threeCount.text = "x " + threeShipCount;
        fourCount.text = "x " + fourShipCount;
    }
    public void UpdateShipLength(int shipLength)
    {
        switch (shipLength)
        {
            case 1:
                if (oneShipCount > 0)
                {
                    this.shipLength = shipLength;
                }
                else
                {
                    this.shipLength = 0;
                    RepaintSeaFightField(playerField, playerFieldObject);
                }
                break;
            case 2:
                if (twoShipCount > 0)
                {
                    this.shipLength = shipLength;
                }
                else
                {
                    this.shipLength = 0;
                    RepaintSeaFightField(playerField, playerFieldObject);
                }
                break;
            case 3:
                if (threeShipCount > 0)
                {
                    this.shipLength = shipLength;
                }
                else
                {
                    this.shipLength = 0;
                    RepaintSeaFightField(playerField, playerFieldObject);
                }
                break;
            case 4:
                if (fourShipCount > 0)
                {
                    this.shipLength = shipLength;
                }
                else
                {

                    this.shipLength = 0;
                    RepaintSeaFightField(playerField, playerFieldObject);
                }
                break;
            default:
                break;
        }
    }
    private void ShowReadyButton()
    {
        if ((oneShipCount + twoShipCount + threeShipCount + fourShipCount) < 1)
        {
            readyButton.gameObject.SetActive(true);
        }
    }
    public void Rotate()
    {
        isHorisontal = !isHorisontal;
    }
    public void Restart()
    {
        playerField = new char[100];
        markedPlayerField = new char[100];
        enemyField = new char[100];
        for (int i = 0; i < 100; i++)
        {
            playerField[i] = ' ';
            markedPlayerField[i] = ' ';
            enemyField[i] = ' ';
        }
        readyButton.gameObject.SetActive(false);
        shipsPanel.gameObject.SetActive(true);
        information.text = "";
        isPositingMode = true;
        isHorisontal = true;
        isEndedGame = false;
        shipLength = 0;
        oneShipCount = 4;
        twoShipCount = 3;
        threeShipCount = 2;
        fourShipCount = 1;
        UpdateShipCounts();
        CreateSeaFightField(playerField, playerFieldObject);
    }
    public void ClickReadyButton()
    {
        shipsPanel.gameObject.SetActive(false);
        PlaceEnemyShips(enemyField);
        CreateSeaFightField(enemyField, enemyFieldObject);
        MarkFieldAsEnemy(enemyFieldObject);
    }
}

