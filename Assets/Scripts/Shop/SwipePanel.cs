using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipePanel : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage;
    public int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime = 0.3f;
    float dragThreshould;

    public RectTransform[] imageRectTransform;

    [Header("Точки")]
    public Transform middlePoint;
    public Transform rightPoint;
    public Transform UnderRightPoint;
    public Transform leftPoint;
    public Transform UnderLeftPoint;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;

        imageRectTransform[0].localPosition = middlePoint.localPosition;
        imageRectTransform[1].localPosition = rightPoint.localPosition;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            int count = currentPage;
            DataManager.InstanceData.shop.Change(count);
            currentPage++;

            switch (currentPage)
            {
                case 2:
                    imageRectTransform[0].localPosition = leftPoint.localPosition;
                    imageRectTransform[1].localPosition = middlePoint.localPosition;
                    imageRectTransform[2].localPosition = rightPoint.localPosition;
                    break;
                case 3:
                    imageRectTransform[0].localPosition = UnderLeftPoint.position + new Vector3(5000,0,0);
                    imageRectTransform[1].localPosition = leftPoint.localPosition;
                    imageRectTransform[2].localPosition = middlePoint.localPosition;
                    imageRectTransform[3].localPosition = rightPoint.localPosition;
                    break;
                case 4:
                    imageRectTransform[1].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[2].localPosition = leftPoint.localPosition;
                    imageRectTransform[3].localPosition = middlePoint.localPosition;
                    imageRectTransform[4].localPosition = rightPoint.localPosition;
                    break;
                case 5:
                    imageRectTransform[2].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[3].localPosition = leftPoint.localPosition;
                    imageRectTransform[4].localPosition = middlePoint.localPosition;
                    imageRectTransform[5].localPosition = rightPoint.localPosition;
                    break;
                case 6:
                    imageRectTransform[3].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[4].localPosition = leftPoint.localPosition;
                    imageRectTransform[5].localPosition = middlePoint.localPosition;
                    imageRectTransform[6].localPosition = rightPoint.localPosition;
                    break;
                case 7:
                    imageRectTransform[4].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[5].localPosition = leftPoint.localPosition;
                    imageRectTransform[6].localPosition = middlePoint.localPosition;
                    imageRectTransform[7].localPosition = rightPoint.localPosition;
                    break;
                case 8:
                    imageRectTransform[5].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[6].localPosition = leftPoint.localPosition;
                    imageRectTransform[7].localPosition = middlePoint.localPosition;
                    imageRectTransform[8].localPosition = rightPoint.localPosition;
                    break;
                case 9:
                    imageRectTransform[6].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[7].localPosition = leftPoint.localPosition;
                    imageRectTransform[8].localPosition = middlePoint.localPosition;
                    imageRectTransform[9].localPosition = rightPoint.localPosition;
                    break;
                case 10:
                    imageRectTransform[7].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[8].localPosition = leftPoint.localPosition;
                    imageRectTransform[9].localPosition = middlePoint.localPosition;
                    imageRectTransform[10].localPosition = rightPoint.localPosition;
                    break;
                case 11:
                    imageRectTransform[8].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    imageRectTransform[9].localPosition = leftPoint.localPosition;
                    imageRectTransform[10].localPosition = middlePoint.localPosition;
                    break;
            }
            targetPos += pageStep;
            StartCoroutine(MovePage());
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            int count = currentPage;
            count-=2;
            DataManager.InstanceData.shop.Change(count);
            currentPage--;
            targetPos -= pageStep;

            switch (currentPage)
            {
                case 1:
                    imageRectTransform[0].localPosition = middlePoint.localPosition;
                    imageRectTransform[1].localPosition = rightPoint.localPosition;
                    imageRectTransform[2].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 2:
                    imageRectTransform[0].localPosition = leftPoint.localPosition;
                    imageRectTransform[1].localPosition = middlePoint.localPosition;
                    imageRectTransform[2].localPosition = rightPoint.localPosition;
                    imageRectTransform[3].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 3:
                    imageRectTransform[1].localPosition = leftPoint.localPosition;
                    imageRectTransform[2].localPosition = middlePoint.localPosition;
                    imageRectTransform[3].localPosition = rightPoint.localPosition;
                    imageRectTransform[4].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 4:
                    imageRectTransform[2].localPosition = leftPoint.localPosition;
                    imageRectTransform[3].localPosition = middlePoint.localPosition;
                    imageRectTransform[4].localPosition = rightPoint.localPosition;
                    imageRectTransform[5].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 5:
                    imageRectTransform[3].localPosition = leftPoint.localPosition;
                    imageRectTransform[4].localPosition = middlePoint.localPosition;
                    imageRectTransform[5].localPosition = rightPoint.localPosition;
                    imageRectTransform[6].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 6:
                    imageRectTransform[4].localPosition = leftPoint.localPosition;
                    imageRectTransform[5].localPosition = middlePoint.localPosition;
                    imageRectTransform[6].localPosition = rightPoint.localPosition;
                    imageRectTransform[7].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 7:
                    imageRectTransform[5].localPosition = leftPoint.localPosition;
                    imageRectTransform[6].localPosition = middlePoint.localPosition;
                    imageRectTransform[7].localPosition = rightPoint.localPosition;
                    imageRectTransform[8].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 8:
                    imageRectTransform[6].localPosition = leftPoint.localPosition;
                    imageRectTransform[7].localPosition = middlePoint.localPosition;
                    imageRectTransform[8].localPosition = rightPoint.localPosition;
                    imageRectTransform[9].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 9:
                    imageRectTransform[7].localPosition = leftPoint.localPosition;
                    imageRectTransform[8].localPosition = middlePoint.localPosition;
                    imageRectTransform[9].localPosition = rightPoint.localPosition;
                    imageRectTransform[10].localPosition = UnderLeftPoint.position + new Vector3(5000, 0, 0);
                    break;
                case 10:
                    imageRectTransform[8].localPosition = leftPoint.localPosition;
                    imageRectTransform[9].localPosition = middlePoint.localPosition;
                    imageRectTransform[10].localPosition = rightPoint.localPosition;
                    break;
            }
            StartCoroutine(MovePage());
        }
    }

    IEnumerator MovePage()
    {
        Vector3 startPos = levelPagesRect.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < tweenTime)
        {
            levelPagesRect.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / tweenTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        levelPagesRect.localPosition = targetPos;
        CheckIsBuyRecord();
    }

    public void CheckIsBuyRecord()
    {
        int count = currentPage;
        count--;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            StartCoroutine(MovePage());
        }
    }
}
