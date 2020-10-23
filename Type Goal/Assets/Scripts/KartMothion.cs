using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMothion : MonoBehaviour
{
    public TextBulletManager textBulletManager;

    [Header("Точки передвижение(последовательно)")]
    [SerializeField]
    private Transform[] keyRoadPoints;

    [Header("Скорость движение игрока")]
    [SerializeField]
    private float speed;

    private Transform targetPoint; // точка к которой движемся
    private int currentPoint; // индекс точки
    private bool moveForward; // необходимо ли двигаться дальше

    private void Start()
    {
        // Задаем движение к первой точке
        currentPoint = 0;
        moveForward = true;
        targetPoint = keyRoadPoints[currentPoint];

        Time.timeScale = 1f;
    }
    private void Update()
    {
        // двигаем объект к точке(цели)
        if(moveForward)
        {
            // по достижению точки(цели)
            if (transform.position == new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z))
            {
                // переключаемся на след точку(цель)
                currentPoint++;
                // если след цели не существует
                if (currentPoint == keyRoadPoints.Length)
                {
                    // останавливаем движение
                    moveForward = false;
                }
                // иначе задаем след точку(цель)
                else
                    targetPoint = keyRoadPoints[currentPoint];
            }
            // плавно поворачиваем к точке(цели)
            transform.rotation = Quaternion.Lerp(transform.rotation, targetPoint.rotation, 2 * Time.deltaTime);
            // двигаем к точке(цели)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z), speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // в случаи проигрыша 
        if (other.CompareTag("Bullet"))
        {
            // останавливаем движение всех снарядов
            textBulletManager.StopAllCoroutines();
            // запускам окно поражения
            UIManager.instance.WinLosePanel(UIManager.instance.losePanel);
        }

        if(other.CompareTag("Finish"))
        {
            // если дошли до финиша то запускаем окно победы
            UIManager.instance.WinLosePanel(UIManager.instance.winPanel);
        }
    }
}
