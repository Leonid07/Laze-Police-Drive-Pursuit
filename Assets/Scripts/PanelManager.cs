using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Button startButton;

    public GameObject mainCanvas;
    public GameObject mainCamera;
    public GameObject gameCanvas;
    public TMP_Text textCountMiles;

    private void Start()
    {
        startButton.onClick.AddListener(StartLevel);

        buttonNext.onClick.AddListener(NextLevel);
        buttonHome.onClick.AddListener(BackToHome);

        buttonHomeLose.onClick.AddListener(BackToHome);
        buttonAgain.onClick.AddListener(StartLevel);
    }
    public void StartLevel()
    {
        mainCamera.SetActive(false);
        mainCanvas.SetActive(false);
        gameCanvas.SetActive(true);

        panelWin.SetActive(false);
        panelLose.SetActive(false);
        DataManager.InstanceData.isGameOver = false;
        StartMovingImages();
        FindCarInChoose();

        Destroy(GameObject.Find("Road"));
        Destroy(road);
        road = Instantiate(startLocation);

        SoundManager.InstanceSound.gameSound.Play();
        SoundManager.InstanceSound.mainMenuSound.Stop();

        SetStartPosition();
    }

    public void FindCarInChoose()
    {
        for (int i = 0; i < DataManager.InstanceData.imageCar.Length; i++)
        {
            if (DataManager.InstanceData.imageCar[i].isChangeCar == 2)
            {
                DataManager.InstanceData.carGame[i].SetActive(true);
                DataManager.InstanceData.carGame[i].GetComponent<Player>().maxMiles = 1000 * DataManager.InstanceData.countLevel;
                textCountMiles.text = $"0 mi/{DataManager.InstanceData.carGame[i].GetComponent<Player>().maxMiles} MI";
                DataManager.InstanceData.carGame[i].GetComponent<Player>().StartCounter();
            }
        }
    }

    [Header("Панели проигрыша и выйгрыша")]
    public GameObject panelWin;
    public Button buttonNext;
    public Button buttonHome;
    public GameObject panelLose;
    public Button buttonAgain;
    public Button buttonHomeLose;

    #region
    public void StartCoroutineFadeOff(GameObject panel)
    {
        StartCoroutine(FadeOff(panel));
    }
    public void RemoveFadeOff(GameObject panel)
    {
        panel.GetComponent<Image>().color = new Color32 (0,0,0,200);
    }
    public IEnumerator FadeOff(GameObject panel)
    {
        panel.SetActive(true);
        Image panelImage = panel.GetComponent<Image>();
        Color32 startColor = panelImage.color;
        Color32 endColor = new Color32(0, 0, 0, 255);
        float duration = 1f; // длительность затемнения
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panelImage.color = Color32.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        // Убедитесь, что конечный цвет установлен
        panelImage.color = endColor;
    }
    #endregion

    [Header("рестар уровень")]
    public GameObject startLocation;
    GameObject road;

    public void NextLevel()
    {
        DataManager.InstanceData.countLevel++;
        DataManager.InstanceData.SaveLevelProgress();
        DataManager.InstanceData.ApplyCountLevelProgress();
        StartLevel();
    }
    public void BackToHome()
    {
        SoundManager.InstanceSound.gameSound.Stop();
        SoundManager.InstanceSound.mainMenuSound.Play();

        mainCamera.SetActive(true);
        mainCanvas.SetActive(true);
        gameCanvas.SetActive(false);

        panelWin.SetActive(false);
        panelLose.SetActive(false);

        for (int i = 0; i < DataManager.InstanceData.carGame.Length; i++)
        {
            if (DataManager.InstanceData.carGame[i].gameObject.activeInHierarchy == true)
            {
                DataManager.InstanceData.carGame[i].SetActive(false);
            }
        }
    }

    public void SetStartPosition()
    {
        for (int i = 0; i < DataManager.InstanceData.carGame.Length; i++)
        {
            if (DataManager.InstanceData.carGame[i].gameObject.activeInHierarchy == true)
            {
                DataManager.InstanceData.carGame[i].transform.position = DataManager.InstanceData.startPosition[i];
                DataManager.InstanceData.carGame[i].GetComponent<Player>().currentTargetIndex = 0;
            }
        }
    }

    [Header("анимация в начале игры")]
    public Image topImage;
    public Image bottomImage;
    public float moveSpeed = 200f;

    private Coroutine moveTopCoroutine;
    private Coroutine moveBottomCoroutine;

    private IEnumerator MoveImageUpwards(Image image)
    {
        while (image.rectTransform.anchoredPosition.y < Screen.height)
        {
            image.rectTransform.anchoredPosition += new Vector2(0, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveImageDownwards(Image image)
    {
        while (image.rectTransform.anchoredPosition.y > -Screen.height)
        {
            image.rectTransform.anchoredPosition -= new Vector2(0, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StartMovingImages()
    {
        if (moveTopCoroutine != null)
        {
            StopCoroutine(moveTopCoroutine);
        }
        if (moveBottomCoroutine != null)
        {
            StopCoroutine(moveBottomCoroutine);
        }

        moveTopCoroutine = StartCoroutine(MoveImageUpwards(topImage));
        moveBottomCoroutine = StartCoroutine(MoveImageDownwards(bottomImage));
    }
}