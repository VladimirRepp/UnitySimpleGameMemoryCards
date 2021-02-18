using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [Header("Card param")]
    [Tooltip("Задняя часть карты")]
    [SerializeField]
    private GameObject _cardBack;
    [Tooltip("Изображение карты")]
    [SerializeField]
    private Sprite _imageCard;
    [Tooltip("Сценарий для управления картами")]
    [SerializeField]
    private CardsController _cardsController;

    [SerializeField]
    private int _id = 0;//индификатор текущей карты
    public int id//свойство - которое выводит индификатор из сцнария
    {
        get { return _id; }
    }

    private void OnMouseDown()
    {
        //Проверяем, активен ли объект _cardBack и можно ли открыть карту
        if (_cardBack.activeSelf && _cardsController.canOpen)
        {
            _cardBack.SetActive(false);
            _cardsController.CardOpened(this);
        }
    }

    public void SetCard(int ID, Sprite im)
    {
         //Открытый метод - задать карту
        _id = ID;
        GetComponent<SpriteRenderer>().sprite = im;
        _imageCard = im;
    }

    public void Close()
    {
        _cardBack.SetActive(true);
    }

    public void Open()
    {
        _cardBack.SetActive(false);
    }
}
