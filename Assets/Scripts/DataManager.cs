using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager InstanceData { get; private set; }

    private void Awake()
    {
        if (InstanceData != null && InstanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceData = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Header("Окончание уровня")]
    public bool isGameOver = false;
    [Space(10)]

    public Image_Car[] imageCar;
    public string[] idImageCar;
    public int[] priceForCar;

    public TMP_Text[] textGold;
    public int countGold;
    public string idCountGold = "CountGold";

    public int countLevel = 1;
    public string idCountLevel = "idCountLevel";
    public TMP_Text textCountLevel;

    public Shop shop;

    public GameObject[] carGame;

    public Vector3[] startPosition;

    public Sprite[] spriteCar;

    private void Start()
    {
        ApplyGoldToText();
        ApplyId();
        LoadImageCar();
        LoadGold();

        LoadLevelProgress();
    }

    public void SaveLevelProgress()
    {
        PlayerPrefs.SetInt(idCountLevel, countLevel);
        PlayerPrefs.Save();
    }
    public void LoadLevelProgress()
    {
        if (PlayerPrefs.HasKey(idCountLevel))
        {
            countLevel = PlayerPrefs.GetInt(idCountLevel);
            ApplyCountLevelProgress();
        }
    }
    public void ApplyCountLevelProgress()
    {
        textCountLevel.text = countLevel.ToString();
    }

    public void LoadGold()
    {
        if (PlayerPrefs.HasKey(idCountGold))
        {
            countGold = PlayerPrefs.GetInt(idCountGold);
            ApplyGoldToText();
        }
    }
    public void SaveGold()
    {
        PlayerPrefs.SetInt(idCountGold, countGold);
        PlayerPrefs.Save();
    }
    public void ApplyGoldToText()
    {
        for (int i = 0; i < textGold.Length; i++)
        {
            textGold[i].text = countGold.ToString();
        }
    }

    private void ApplyId()
    {
        for (int i = 0; i < imageCar.Length; i++)
        {
            idImageCar[i] = imageCar[i].gameObject.name;
        }
    }

    public void SaveImageCar()
    {
        for (int i =0; i < imageCar.Length; i++)
        {
            PlayerPrefs.SetInt(idImageCar[i],imageCar[i].isChangeCar);
            PlayerPrefs.Save();
        }
    }
    public void LoadImageCar()
    {
        for (int i = 0; i < imageCar.Length; i++)
        {
            if (PlayerPrefs.HasKey(idImageCar[i]))
            {
                imageCar[i].isChangeCar = PlayerPrefs.GetInt(idImageCar[i]);
            }
        }
    }
}
