using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float[] targetXPositions = { -5f, 5f, 15f, 25f }; // Позиции по X
    public float moveSpeed = 5f;  // Скорость перемещения
    public float forwardSpeed = 5f;  // Скорость движения по оси Z

    public TMP_Text textCounter;
    public int counter;

    private Vector2 startTouchPosition, endTouchPosition; // Для отслеживания свайпа
    public int currentTargetIndex = 0;  // Индекс текущей целевой точки
    private Coroutine coroutineStartValue;

    [Header("Максимальное значение которое нужно достигнуть")]
    public int maxMiles = 1000;

    void Update()
    {
        if (DataManager.InstanceData.isGameOver == false)
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            // Обработка ввода с клавиатуры для тестирования на ПК
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToNextPoint(1);  // Двигаемся вправо
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToNextPoint(-1);  // Двигаемся влево
            }

            // Отслеживаем свайпы на устройстве
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    endTouchPosition = touch.position;
                    DetectSwipe();
                }
            }

            // Плавно перемещаем объект к целевой точке на оси X
            Vector3 targetPosition = new Vector3(targetXPositions[currentTargetIndex], transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // запуск движения
    public void StartCounter()
    {
        counter = 0;
        textCounter.text = $"0 mi/{maxMiles} MI";
        coroutineStartValue = StartCoroutine(Counter());
    }

    // Метод для определения направления свайпа
    void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        // Определяем горизонтальный свайп
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)  // Свайп вправо
                MoveToNextPoint(1);
            else  // Свайп влево
                MoveToNextPoint(-1);
        }
    }

    // Метод для перехода к следующей точке
    void MoveToNextPoint(int direction)
    {
        currentTargetIndex += direction;

        // Ограничиваем индекс, чтобы не выйти за пределы массива точек
        currentTargetIndex = Mathf.Clamp(currentTargetIndex, 0, targetXPositions.Length - 1);
    }

    public IEnumerator Counter()
    {
        while (true)
        {
            counter += 1;
            textCounter.text = $"{counter} mi/{maxMiles} MI";

            if (counter >= maxMiles) // Проверяем достижение 1000
            {
                Debug.Log("Счётчик достиг 1000! Остановка корутины.");
                PanelManager.InstancePanel.StartCoroutineFadeOff(PanelManager.InstancePanel.panelWin);
                DataManager.InstanceData.countLevel++;
                DataManager.InstanceData.SaveLevelProgress();
                DataManager.InstanceData.ApplyCountLevelProgress();
                yield return new WaitForSeconds(0.5f);
                DataManager.InstanceData.isGameOver = true;
                yield break; // Завершаем корутину
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BAR")
        {
            StopCoroutine(coroutineStartValue);
            //Destroy(gameObject);
            DataManager.InstanceData.isGameOver = true;
            PanelManager.InstancePanel.StartCoroutineFadeOff(PanelManager.InstancePanel.panelLose);
            //Событие когда объект врезается в барикаду
        }
    }
}
