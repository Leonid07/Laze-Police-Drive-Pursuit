using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float[] targetXPositions = { -5f, 5f, 15f, 25f }; // ������� �� X
    public float moveSpeed = 5f;  // �������� �����������
    public float forwardSpeed = 5f;  // �������� �������� �� ��� Z

    public TMP_Text textCounter;
    public int counter;

    private Vector2 startTouchPosition, endTouchPosition; // ��� ������������ ������
    public int currentTargetIndex = 0;  // ������ ������� ������� �����
    private Coroutine coroutineStartValue;

    [Header("������������ �������� ������� ����� ����������")]
    public int maxMiles = 1000;

    void Update()
    {
        if (DataManager.InstanceData.isGameOver == false)
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            // ��������� ����� � ���������� ��� ������������ �� ��
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToNextPoint(1);  // ��������� ������
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToNextPoint(-1);  // ��������� �����
            }

            // ����������� ������ �� ����������
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

            // ������ ���������� ������ � ������� ����� �� ��� X
            Vector3 targetPosition = new Vector3(targetXPositions[currentTargetIndex], transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // ������ ��������
    public void StartCounter()
    {
        counter = 0;
        textCounter.text = $"0 mi/{maxMiles} MI";
        coroutineStartValue = StartCoroutine(Counter());
    }

    // ����� ��� ����������� ����������� ������
    void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        // ���������� �������������� �����
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0)  // ����� ������
                MoveToNextPoint(1);
            else  // ����� �����
                MoveToNextPoint(-1);
        }
    }

    // ����� ��� �������� � ��������� �����
    void MoveToNextPoint(int direction)
    {
        currentTargetIndex += direction;

        // ������������ ������, ����� �� ����� �� ������� ������� �����
        currentTargetIndex = Mathf.Clamp(currentTargetIndex, 0, targetXPositions.Length - 1);
    }

    public IEnumerator Counter()
    {
        while (true)
        {
            counter += 1;
            textCounter.text = $"{counter} mi/{maxMiles} MI";

            if (counter >= maxMiles) // ��������� ���������� 1000
            {
                Debug.Log("������� ������ 1000! ��������� ��������.");
                PanelManager.InstancePanel.StartCoroutineFadeOff(PanelManager.InstancePanel.panelWin);
                DataManager.InstanceData.countLevel++;
                DataManager.InstanceData.SaveLevelProgress();
                DataManager.InstanceData.ApplyCountLevelProgress();
                yield return new WaitForSeconds(0.5f);
                DataManager.InstanceData.isGameOver = true;
                yield break; // ��������� ��������
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
            //������� ����� ������ ��������� � ��������
        }
    }
}
