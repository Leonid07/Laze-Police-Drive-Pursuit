using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Load : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText; // ��������� ��������� ��� ����������� �������� ��������
    [SerializeField] private Image progressBar; // ��������-��� ��� ��������� FillAmount

    private void Start()
    {
        StartCoroutine(SimulateLoading());
    }

    private IEnumerator SimulateLoading()
    {
        float loadingProgress = 0f;

        while (loadingProgress < 1f)
        {
            loadingProgress += Time.deltaTime * 0.2f; // ����������� �������� ��������, ������� ���������
            loadingText.text = $"{(loadingProgress * 100):0}%"; // ������� �������
            progressBar.fillAmount = loadingProgress; // ��������� fillAmount

            yield return null;
        }

        loadingText.text = "100%";
        progressBar.fillAmount = 1f;
        gameObject.SetActive(false);
    }
}
