using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBulletManager : MonoBehaviour
{
    [Header("Префаб снаряда")]
    [SerializeField]
    private GameObject bulletPrefab;
    
    // массив всех снарядов
    private GameObject[] bullets;
    private int bulletIndex;

    [Header("Слова на снарядах")]
    [SerializeField]
    private string[] demandWords;

    public GameObject player;
    [Header("Точка спавна снаряда")]
    [SerializeField]
    private Transform spawnPoint;

    [Header("Ключевые точки(выстрела)")]
    // при достижении которых вылетает снаряд
    [SerializeField]
    private Transform[] keyShootPoints;
    private int keyPointsIndex;

    [Header("Скорость снаряда")]
    [SerializeField]
    private float bulletSpeed;

    [Header("Партикл вызрыва")]
    [SerializeField]
    private ParticleSystem partSys;

    // хранение запущенных корутин и доступ к ним
    private Coroutine[] coroutines;
    private int indexCoroutines;

    // индекс нужен для проверка текста поочередно 
    private int indexNextText;

    // хранит расположение след точки вылета снаряда
    private Vector3 keyPoint;

    private void Start()
    {
        // инициализируем переменные
        keyPointsIndex = 0;
        bulletIndex = 0;
        indexCoroutines = 0;
        indexNextText = 0;

        // инициализация массивов
        bullets = new GameObject[demandWords.Length];
        coroutines = new Coroutine[bullets.Length];

        // создание, заполнение, подготовка массива снарядов
        for (int i = 0; i < demandWords.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, player.transform);
            bullets[i].GetComponentInChildren<TextMeshPro>().text = demandWords[i];
            bullets[i].SetActive(false);
        }

        // создание и присвоение взрыва
        partSys = Instantiate(partSys, player.transform);

        // получаем координаты первой точки вылета снаряда
        keyPoint = new Vector3(keyShootPoints[keyPointsIndex].position.x, player.transform.position.y, keyShootPoints[keyPointsIndex].position.z);
    }
    private void Update()
    {
        // если точка вылета снаряда достигнута
        if (player.transform.position == keyPoint)
        {
            // запускам движение снаряда
            coroutines[indexCoroutines] = StartCoroutine(ShootTheText(bulletIndex));
            indexCoroutines++;

            // переходим к след снаряду
            if (bulletIndex < keyShootPoints.Length)
                bulletIndex++;
            if (keyPointsIndex < keyShootPoints.Length - 1)
                keyPointsIndex++;

            // получаем координаты след точки вылета снаряда
            keyPoint = new Vector3(keyShootPoints[keyPointsIndex].position.x, player.transform.position.y, keyShootPoints[keyPointsIndex].position.z);
        }
        CheckInputWord();
    }

    /// <summary>
    /// Выпускает заряд в игрока
    /// </summary>
    /// <param name="bulletIndex">Номер заряда</param>
    /// <returns></returns>
    private IEnumerator ShootTheText(int bulletIndex)
    {
        // размещаем снаряд в место спавна
        bullets[bulletIndex].transform.position = spawnPoint.position;

        // поворачиваем снаряд к игроку и включаем
        bullets[bulletIndex].transform.LookAt(player.transform);
        bullets[bulletIndex].SetActive(true);

        //двигаем снаряд к игроку
        do
        {
            yield return new WaitForEndOfFrame();
            bullets[bulletIndex].transform.position = Vector3.MoveTowards(bullets[bulletIndex].transform.position,
                                                                      new Vector3(player.transform.position.x,
                                                                      bullets[bulletIndex].transform.position.y,
                                                                      player.transform.position.z), bulletSpeed * Time.deltaTime);
        } while (true);
    }

    /// <summary>
    /// Проверка введенного слова
    /// </summary>
    public void CheckInputWord()
    {
        // если снаряд активен
        if(bullets[indexNextText].activeSelf)
            // если совпадают написанное с требуемым
            if (String.Equals(UIManager.instance.inputText.ToUpper(), demandWords[indexNextText]))
            {
                // остнавливаем движение снаряда
                StopCoroutine(coroutines[indexNextText]);

                // взрыв помещаем на место снаряда и запускаем
                partSys.transform.position = bullets[indexNextText].transform.position;
                partSys.Play();

                // Отключаем снаряд
                bullets[indexNextText].SetActive(false);

                // опусташаем поле инпута
                UIManager.instance.inputField.text = null;

                // переходим к след слову
                if(indexNextText < bullets.Length - 1)
                    indexNextText++;
            }
    }
}
