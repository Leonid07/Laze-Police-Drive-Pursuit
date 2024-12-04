using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [Header("Цвета бонусов")]
    public Color32 DisActiveColor = new Color32(104, 104, 104, 255);
    public Color32 ActiveColor = new Color32(255, 255, 255, 255);
    public Image imageDaily;

    public TMP_Text dailyBonusText;

    public TMP_Text dailyText;

    [Space(10)]
    public Button dailyBonusButton;

    private const string DailyBonusTimeKey = "daily_bonus_time";

    public int DailyBonusCooldownInSeconds = 86400; // 24 

    public Image BG_car;
    public Image car;
    public float duration = 2f;

    private void Start()
    {
        dailyBonusButton.onClick.AddListener(() => HandleButtonClick(ClaimDailyBonus));

        StartCoroutine(UpdateBonusTextsRoutine());
    }

    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");

        long dailyBonusTime = long.Parse(dailyBonusTimeStr);

        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;

        dailyText.text = FormatTimeDaily(dailyCooldown);

        dailyBonusButton.interactable = dailyCooldown <= 0;
    }

    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            dailyText.gameObject.SetActive(false);
            dailyBonusText.gameObject.SetActive(true);
            imageDaily.color = ActiveColor;

            BG_car.color = new Color32(255,255,255,255);
            car.color = new Color32(255,255,255,0);

            return "Open";
        }
        imageDaily.color = DisActiveColor;
        dailyBonusText.gameObject.SetActive(false);
        dailyText.gameObject.SetActive(true);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        car.sprite = DataManager.InstanceData.spriteCar[FindAvailableCar()];

        StartFadeIn();
        StartFadeOut();
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    public int FindAvailableCar()
    {
        List<int> availableCars = new List<int>();

        for (int i = 0; i < DataManager.InstanceData.imageCar.Length; i++)
        {
            if (DataManager.InstanceData.imageCar[i].isChangeCar == 0)
            {
                availableCars.Add(i);
            }
        }

        if (availableCars.Count > 0)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(availableCars.Count);

            int chosenCarIndex = availableCars[randomIndex];
            DataManager.InstanceData.imageCar[chosenCarIndex].isChangeCar = 1;
            DataManager.InstanceData.shop.Change(chosenCarIndex);
            DataManager.InstanceData.SaveImageCar();
            return chosenCarIndex;
        }
        return 0;
    }

    private void HandleButtonClick(Action onAnimationComplete)
    {
        onAnimationComplete.Invoke();
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    public IEnumerator FadeIn()
    {
        Color color = car.color;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            car.color = color;
            yield return null;
        }
        color.a = endAlpha;
        car.color = color;
    }
    public IEnumerator FadeOut()
    {
        Color color = BG_car.color;
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            BG_car.color = color;
            yield return null;
        }
        color.a = endAlpha;
        BG_car.color = color;
    }
}