using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsController : MonoBehaviour
{
    [Header("Grid settings")]
    public const int gridRows = 2;
    public const int gridColumns = 4;
    public const float offSetX = 4f;
    public const float offSetY = 4.5f;

    [Header("Cards parametrs")]
    [Tooltip("Ссылка на карты в сцене")]
    [SerializeField]
    private MemoryCard _originalCard;
    [Tooltip("Ссылки на картинки карт")]
    [SerializeField]
    private Sprite[] _imCards;

    [Header("Text parameters")]
    [Tooltip("Вывод данных счета")]
    [SerializeField]
    private Text scoreText;

    [Header("Start settings")]
    [Tooltip("Время, на которое карта будет видна в начале игры")]
    [Range(0.5f, 2f)]
    [SerializeField]
    private float visibleTimeCard = 0.5f;

    //Счетчик совпадений
    private int _score = 0;

    //Открыть можно только две карты
    //для этого будет два объекта
    private MemoryCard _firstOpenCard;
    private MemoryCard _secondOpenCard;

    private void Start()
    {
        //Положение первой карты будет точкой отсчета 
        //для остальных карт
        Vector3 startPos = transform.position;

        /*
            Задача игры, найти все парные карты
            для этого создадим целочисленный массив
            с всеми парными индификаторами:
         */
        int[] numID = {0, 0, 1, 1, 2, 2, 3, 3};
        numID = ShuffleArray(numID);//перемашаем этот массив

        //Проходим по сетки, чтобы расположить на ней карты
        for (int i = 0; i< gridColumns; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;//ссылка на карту

                //Проверим, карта исходная (начальная) или нет
                if (i == 0 && j == 0)
                {
                    card = _originalCard;
                }
                else
                {
                    //Иначе, создаем (добавляем) карту 
                    card = Instantiate(_originalCard) as MemoryCard;
                }

                /*
                //Случайно выбираем номер карты и ее картинку
                int rand = Random.Range(0, _imCards.Length);
                */

                //Индификаторы получаем из перемешанного массива
                int index = j * gridColumns + i;
                int id = numID[index];
                card.SetCard(id, _imCards[id]);

                //Задаем позицию карты
                float posX = (offSetX * i) + startPos.x;
                float posY = (offSetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);

                //Покажем карту на некоторое время
                StartCoroutine(ShowCardsCoroutine(visibleTimeCard, card));
            }
        }

    }

    private int[] ShuffleArray(int[] array)
    {
        int[] newArray = array.Clone() as int[];//создаем новый массив - клон текущего

        //Проходим по всем элементам массива
        for(int i = 0; i<newArray.Length; i++)
        {
            int num = newArray[i];//берем число
            int rand = Random.Range(i, newArray.Length);//перем случайное следущее число
            //Меняем их местами
            newArray[i] = newArray[rand];
            newArray[rand] = num;
        }
        return newArray;
    }

    public bool canOpen
    {
        //вернуть true, елси вторая карта открыта
        get { return _secondOpenCard == null; }
    }

    public void CardOpened(MemoryCard card)
    {
        //Проверяем, открыта ли первая карта
        if(_firstOpenCard == null)
        {
            _firstOpenCard = card;
        }
        else
        {
            _secondOpenCard = card;
            //Debug.Log("IDs: " + _firstOpenCard.id + ", " + _secondOpenCard.id);
            StartCoroutine(CheckMatchCoroutine());//запускаем сопрограмму
        }
    }

    private IEnumerator CheckMatchCoroutine()
    {
        //Сопрогрмма для проверки совпадения карт
        if(_firstOpenCard.id == _secondOpenCard.id)
        {
            //при совпадении увеличиваем счетчик
            _score++;
            scoreText.text = "Счет: " + _score;
        }
       else {
            //иначе через время закрываем карты
            yield return new WaitForSeconds(.5f);

            _secondOpenCard.Close();
            _firstOpenCard.Close();
       }

        //очищаем карты вне зависимости от совпадения
        _firstOpenCard = null;
        _secondOpenCard = null;
    }

    private IEnumerator ShowCardsCoroutine(float waitSecond, MemoryCard card)
    {
        //При запуске покажем все карты на время
        card.Open();

        yield return new WaitForSeconds(waitSecond);

        card.Close();
    }
}
