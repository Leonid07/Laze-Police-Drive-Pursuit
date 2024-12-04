using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoad : MonoBehaviour
{
    public GameObject[] prefabs;    // Массив префабов
    public Transform parent;        // Родительский объект
    public int objectCount = 10;    // Количество создаваемых объектов
    public float zStep = 20f;       // Шаг по оси Z
    public float minSpawnInterval = 0.5f;  // Минимальный интервал спавна
    public float maxSpawnInterval = 0.7f;    // Максимальный интервал спавна
    public string destroyTag = "Despawn";  // Тег для уничтожаемых объектов
    public Camera mainCamera;              // Камера для отслеживания видимости объектов

    private Queue<GameObject> spawnedObjects = new Queue<GameObject>(); // Очередь объектов
    private float lastZPosition;   // Позиция Z последнего объекта
    private float nextSpawnTime;   // Время до следующего спавна

    void Start()
    {
        GameObject cameraObject = GameObject.Find("Main Camera");
        if (cameraObject != null && cameraObject.activeInHierarchy)
        {
            mainCamera = cameraObject.GetComponent<Camera>();
        }
        SpawnInitialObjects(); // Создаем начальные объекты
        SetNextSpawnTime();    // Устанавливаем время для следующего спавна
    }

    void Update()
    {
        CheckAndDestroyObjects();  // Проверяем и удаляем объекты

        if (DataManager.InstanceData.isGameOver == true)
        {
            spawnedObjects.Clear();
            return;
        }

        // Периодически создаем новые объекты
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            SetNextSpawnTime();  // Обновляем время для следующего спавна
        }
    }

    // Метод для начального создания объектов
    void SpawnInitialObjects()
    {
        lastZPosition = 0f; // Начальная позиция по Z

        for (int i = 0; i < objectCount; i++)
        {
            SpawnObject();  // Создаем объект
        }
    }

    // Спавн случайного префаба
    void SpawnObject()
    {
        Vector3 position = new Vector3(0, 0, lastZPosition);
        GameObject prefab = GetRandomPrefab();  // Выбираем случайный префаб
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity, parent);
        newObject.tag = destroyTag; // Устанавливаем тег
        spawnedObjects.Enqueue(newObject); // Добавляем в очередь

        lastZPosition += zStep; // Увеличиваем позицию Z для следующего объекта
    }

    // Установка времени для следующего спавна
    void SetNextSpawnTime()
    {
        float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnTime = Time.time + interval;
    }

    // Выбор случайного префаба из массива
    GameObject GetRandomPrefab()
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    // Проверка видимости объектов и их удаление при выходе за пределы экрана
    void CheckAndDestroyObjects()
    {
        if (DataManager.InstanceData.isGameOver == false)
        {
            if (spawnedObjects.Count > 0)
            {
                GameObject firstObject = spawnedObjects.Peek(); // Берем первый объект в очереди

                if (!IsObjectVisible(firstObject)) // Если объект не виден камерой
                {
                    if (DataManager.InstanceData.isGameOver == false) // Проверяем перед уничтожением
                    {
                        Destroy(firstObject, 3f); // Уничтожаем объект с задержкой
                    }
                    spawnedObjects.Dequeue(); // Удаляем из очереди

                    // Создаем новый объект
                    SpawnObject();
                }
            }
        }
    }


    // Проверка видимости объекта в камере
    bool IsObjectVisible(GameObject obj)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(obj.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}