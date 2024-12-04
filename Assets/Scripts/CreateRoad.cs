using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoad : MonoBehaviour
{
    public GameObject[] prefabs;    // ������ ��������
    public Transform parent;        // ������������ ������
    public int objectCount = 10;    // ���������� ����������� ��������
    public float zStep = 20f;       // ��� �� ��� Z
    public float minSpawnInterval = 0.5f;  // ����������� �������� ������
    public float maxSpawnInterval = 0.7f;    // ������������ �������� ������
    public string destroyTag = "Despawn";  // ��� ��� ������������ ��������
    public Camera mainCamera;              // ������ ��� ������������ ��������� ��������

    private Queue<GameObject> spawnedObjects = new Queue<GameObject>(); // ������� ��������
    private float lastZPosition;   // ������� Z ���������� �������
    private float nextSpawnTime;   // ����� �� ���������� ������

    void Start()
    {
        GameObject cameraObject = GameObject.Find("Main Camera");
        if (cameraObject != null && cameraObject.activeInHierarchy)
        {
            mainCamera = cameraObject.GetComponent<Camera>();
        }
        SpawnInitialObjects(); // ������� ��������� �������
        SetNextSpawnTime();    // ������������� ����� ��� ���������� ������
    }

    void Update()
    {
        CheckAndDestroyObjects();  // ��������� � ������� �������

        if (DataManager.InstanceData.isGameOver == true)
        {
            spawnedObjects.Clear();
            return;
        }

        // ������������ ������� ����� �������
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            SetNextSpawnTime();  // ��������� ����� ��� ���������� ������
        }
    }

    // ����� ��� ���������� �������� ��������
    void SpawnInitialObjects()
    {
        lastZPosition = 0f; // ��������� ������� �� Z

        for (int i = 0; i < objectCount; i++)
        {
            SpawnObject();  // ������� ������
        }
    }

    // ����� ���������� �������
    void SpawnObject()
    {
        Vector3 position = new Vector3(0, 0, lastZPosition);
        GameObject prefab = GetRandomPrefab();  // �������� ��������� ������
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity, parent);
        newObject.tag = destroyTag; // ������������� ���
        spawnedObjects.Enqueue(newObject); // ��������� � �������

        lastZPosition += zStep; // ����������� ������� Z ��� ���������� �������
    }

    // ��������� ������� ��� ���������� ������
    void SetNextSpawnTime()
    {
        float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnTime = Time.time + interval;
    }

    // ����� ���������� ������� �� �������
    GameObject GetRandomPrefab()
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    // �������� ��������� �������� � �� �������� ��� ������ �� ������� ������
    void CheckAndDestroyObjects()
    {
        if (DataManager.InstanceData.isGameOver == false)
        {
            if (spawnedObjects.Count > 0)
            {
                GameObject firstObject = spawnedObjects.Peek(); // ����� ������ ������ � �������

                if (!IsObjectVisible(firstObject)) // ���� ������ �� ����� �������
                {
                    if (DataManager.InstanceData.isGameOver == false) // ��������� ����� ������������
                    {
                        Destroy(firstObject, 3f); // ���������� ������ � ���������
                    }
                    spawnedObjects.Dequeue(); // ������� �� �������

                    // ������� ����� ������
                    SpawnObject();
                }
            }
        }
    }


    // �������� ��������� ������� � ������
    bool IsObjectVisible(GameObject obj)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(obj.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}