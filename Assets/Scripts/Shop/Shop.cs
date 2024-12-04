using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button buttonShop;
    public SwipePanel swipePanel;

    public GameObject textSelected;
    public GameObject textChoose;
    public GameObject BuyPrice;

    public TMP_Text price;

    private void Start()
    {
        buttonShop.onClick.AddListener(Buy);
    }

    public void Buy()
    {
        int count = swipePanel.currentPage;
        count--;

        if (DataManager.InstanceData.imageCar[count].isChangeCar == 2)
        {
            return;
        }

        if (DataManager.InstanceData.imageCar[count].isChangeCar == 1)
        {
            IgnoreIndex(count);
            DataManager.InstanceData.imageCar[count].isChangeCar = 2;
            Change(count);
            return;
        }

        if (DataManager.InstanceData.imageCar[count].isChangeCar == 0)
        {
            if (DataManager.InstanceData.countGold >= DataManager.InstanceData.priceForCar[count])
            {
                DataManager.InstanceData.countGold -= DataManager.InstanceData.priceForCar[count];

                DataManager.InstanceData.SaveGold();
                DataManager.InstanceData.ApplyGoldToText();

                DataManager.InstanceData.imageCar[count].isChangeCar = 1;

                Change(count);
                return;
            }
        }
        DataManager.InstanceData.SaveImageCar();
    }

    public void Change(int count)
    {
        switch (DataManager.InstanceData.imageCar[count].isChangeCar)
        {
            case 0:
                textSelected.SetActive(false);
                textChoose.SetActive(false);
                BuyPrice.SetActive(true);
                price.text = DataManager.InstanceData.priceForCar[count].ToString();
                break;
            case 1:
                textSelected.SetActive(false);
                textChoose.SetActive(true);
                BuyPrice.SetActive(false);
                break;
            case 2:
                textSelected.SetActive(true);
                textChoose.SetActive(false);
                BuyPrice.SetActive(false);
                break;
        }
    }

    private void IgnoreIndex(int count)
    {
        for (int i = 0; i < DataManager.InstanceData.imageCar.Length; i++)
        {
            if (DataManager.InstanceData.imageCar[i].isChangeCar == 2)
            {
                DataManager.InstanceData.imageCar[i].isChangeCar = 1;
            }
        }
    }
}