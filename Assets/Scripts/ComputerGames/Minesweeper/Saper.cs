using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saper : MonoBehaviour
{
    public GameObject saperCell;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;
    public Sprite five;
    public Sprite six;
    public Sprite seven;
    public Sprite eight;
    public Sprite flag;
    public Sprite bomb;
    public Text information;
    [SerializeField]
    private char[] saperField;
    [SerializeField]
    private int[] bombPositions;
    public int bombCountOnField = 10;
    private bool isGameEnded = false;

    private bool isLMB = false;
    private bool isRMB = false;

    void Start()
    {
        saperField = new char[100];
        RestartGame();
        CreateSaperField();
    }

    private void ClearField()
    {
        for (int i = 0; i < saperField.Length; i++)
        {
            saperField[i] = ' ';
        }
    }

    public void RestartGame()
    {
        information.GetComponent<Text>().text = "";
        isGameEnded = false;
        ClearField();
        PlaceBombsOnField(bombCountOnField);
        CreateSaperField();
    }

    private void CreateSaperField()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < saperField.Length; i++)
        {
            GameObject cell = Instantiate(saperCell);
            cell.transform.SetParent(transform);
            cell.transform.localScale = new Vector3(1, 1, 1);
            switch (saperField[i])
            {
                case '0':
                    cell.GetComponent<Image>().color = Color.gray;
                    break;
                case '1':
                    cell.GetComponent<Image>().sprite = one;
                    break;
                case '2':
                    cell.GetComponent<Image>().sprite = two;
                    break;
                case '3':
                    cell.GetComponent<Image>().sprite = three;
                    break;
                case '4':
                    cell.GetComponent<Image>().sprite = four;
                    break;
                case '5':
                    cell.GetComponent<Image>().sprite = five;
                    break;
                case '6':
                    cell.GetComponent<Image>().sprite = six;
                    break;
                case '7':
                    cell.GetComponent<Image>().sprite = seven;
                    break;
                case '8':
                    cell.GetComponent<Image>().sprite = eight;
                    break;
                case 'f':
                    cell.GetComponent<Image>().sprite = flag;
                    break;
                case '*':
                    cell.GetComponent<Image>().sprite = bomb;
                    break;
                default:
                    break;
            }
        }
    }

    private bool isWinning()
    {
        bool isWin = true;
        for (int i = 0; i < saperField.Length; i++)
        {
            if(saperField[i]==' ' && !IsBombInPosition(i))
            {
                isWin = false;
                break;
            }
        }
        return isWin;
    }

    private void ShowBombsOnField()
    {
        for (int i = 0; i < bombPositions.Length; i++)
        {
            saperField[bombPositions[i]] = '*';
        }
    }

    private void PlaceBombsOnField(int bombCount)
    {
        bombPositions = new int[bombCount];
        int bombPosition;
        for (int i = 0; i < bombCount; i++)
        {
            bombPosition = Random.Range(0, 99);
            bombPositions[i] = bombPosition;
            for (int j = 0; j < i; j++)
            {
                if (bombPositions[i] == bombPositions[j])
                {
                    i--;
                    break;
                }                
            }            
        }
    }

    private int TakeCountBombAround(int position)
    {
        int bombCount = 0;
        if(position < 10) // первая строка
        {
            if (IsBombInPosition(position + 10))
                bombCount++;
            if (position != 0) //если не первая ячейка
            {
                if (IsBombInPosition(position - 1))
                    bombCount++;
                if (IsBombInPosition(position + 9))
                    bombCount++;
            }
            if (position != 9) //если не последняя ячейка первой строки
            {
                if (IsBombInPosition(position + 1))
                    bombCount++;
                if (IsBombInPosition(position + 11))
                    bombCount++;
            }
        }
        else if (position > 89) // последняя строка
        {
            if (IsBombInPosition(position - 10))
                bombCount++;
            if (position % 10 != 0) // если не первая ячейка последней строки
            {
                if (IsBombInPosition(position - 11))
                    bombCount++;
                if (IsBombInPosition(position - 1))
                    bombCount++;
            }
            if (position % 10 != 9) // если не последняя ячейка последней строки
            {
                if(IsBombInPosition(position - 9))
                    bombCount++;
                if (IsBombInPosition(position + 1))
                    bombCount++;
            }
        }
        else if (position % 10 == 0) // первый столбец
        {
            if (IsBombInPosition(position - 10))
                bombCount++;
            if (IsBombInPosition(position - 9))
                bombCount++;
            if (IsBombInPosition(position + 1))
                bombCount++;
            if (IsBombInPosition(position + 10))
                bombCount++;
            if (IsBombInPosition(position + 11))
                bombCount++;
        }
        else if (position % 10 == 9) // последний столбец
        {
            if (IsBombInPosition(position - 11))
                bombCount++;
            if (IsBombInPosition(position - 10))
                bombCount++;
            if (IsBombInPosition(position - 1))
                bombCount++;
            if (IsBombInPosition(position + 9))
                bombCount++;
            if (IsBombInPosition(position + 10))
                bombCount++;
        }
        else // все остальное
        {
            if (IsBombInPosition(position - 11))
                bombCount++;
            if (IsBombInPosition(position - 10))
                bombCount++;
            if (IsBombInPosition(position - 9))
                bombCount++;
            if (IsBombInPosition(position - 1))
                bombCount++;
            if (IsBombInPosition(position + 1))
                bombCount++;
            if (IsBombInPosition(position + 9))
                bombCount++;
            if (IsBombInPosition(position + 10))
                bombCount++;
            if (IsBombInPosition(position + 11))
                bombCount++;
        }
        return bombCount;
    }

    private int TakeCountZeroAround(int position)
    {
        int zeroCount = 0;
        if (position < 10) // первая строка
        {
            if (saperField[position + 10] == '0')
                zeroCount++;
            if (position != 0) //если не первая ячейка
            {
                if (saperField[position - 1] == '0')
                    zeroCount++;
                if (saperField[position + 9] == '0')
                    zeroCount++;
            }
            if (position != 9) //если не последняя ячейка первой строки
            {
                if (saperField[position + 1] == '0')
                    zeroCount++;
                if (saperField[position + 11] == '0')
                    zeroCount++;
            }
        }
        else if (position > 89) // последняя строка
        {
            if (saperField[position - 10] == '0')
                zeroCount++;
            if (position % 10 != 0) // если не первая ячейка последней строки
            {
                if (saperField[position - 11] == '0')
                    zeroCount++;
                if (saperField[position - 1] == '0')
                    zeroCount++;
            }
            if (position % 10 != 9) // если не последняя ячейка последней строки
            {
                if (saperField[position - 9] == '0')
                    zeroCount++;
                if (saperField[position + 1] == '0')
                    zeroCount++;
            }
        }
        else if (position % 10 == 0) // первый столбец
        {
            if (saperField[position - 10] == '0')
                zeroCount++;
            if (saperField[position - 9] == '0')
                zeroCount++;
            if (saperField[position + 1] == '0')
                zeroCount++;
            if (saperField[position + 10] == '0')
                zeroCount++;
            if (saperField[position + 11] == '0')
                zeroCount++;
        }
        else if (position % 10 == 9) // последний столбец
        {
            if (saperField[position - 11] == '0')
                zeroCount++;
            if (saperField[position - 10] == '0')
                zeroCount++;
            if (saperField[position - 1] == '0')
                zeroCount++;
            if (saperField[position + 9] == '0')
                zeroCount++;
            if (saperField[position + 10] == '0')
                zeroCount++;
        }
        else // все остальное
        {
            if (saperField[position - 11] == '0')
                zeroCount++;
            if (saperField[position - 10] == '0')
                zeroCount++;
            if (saperField[position - 9] == '0')
                zeroCount++;
            if (saperField[position - 1] == '0')
                zeroCount++;
            if (saperField[position + 1] == '0')
                zeroCount++;
            if (saperField[position + 9] == '0')
                zeroCount++;
            if (saperField[position + 10] == '0')
                zeroCount++;
            if (saperField[position + 11] == '0')
                zeroCount++;
        }
        return zeroCount;
    }

    private bool IsBombInPosition(int position)
    {
        for (int i = 0; i < bombPositions.Length; i++)
        {
            if (bombPositions[i] == position)
            {
                return true;
            }            
        }
        return false;
    }

    private void PaintField()
    {
        for (int i = 0; i < saperField.Length; i++)
        {
            if(saperField[i] == ' ')
            {
                if (TakeCountZeroAround(i) > 0)
                {
                    int bombCount = TakeCountBombAround(i);
                    saperField[i] = bombCount.ToString()[0];
                    i = -1;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isLMB = true;
        }
        else
        {
            isLMB = false;
        }

        if (Input.GetMouseButton(1))
        {
            isRMB = true;
        }
        else
        {
            isRMB = false;
        }
    }

    public void ClickOnCell(int position)
    {
        if (isLMB)
        {
            if (saperField[position] == ' ')
            {
                if (!isGameEnded)
                {
                    if (IsBombInPosition(position))
                    {
                        isGameEnded = true;
                        information.GetComponent<Text>().text = "Game Over. You lose";
                        ShowBombsOnField();
                    }
                    else
                    {
                        int bombCount = TakeCountBombAround(position);
                        saperField[position] = bombCount.ToString()[0];
                        if (bombCount == 0)
                        {
                            PaintField();
                        }
                    }
                    CreateSaperField();
                    if (isWinning())
                    {
                        isGameEnded = true;
                        information.GetComponent<Text>().text = "You win. Good job!";
                    }
                }
            }
        }
        if (isRMB)
        {
            if (saperField[position] == ' ')
            {
                saperField[position] = 'f';
            } 
            else if (saperField[position] == 'f') {
                saperField[position] = ' ';
            }
            CreateSaperField();
        }
    }
}
