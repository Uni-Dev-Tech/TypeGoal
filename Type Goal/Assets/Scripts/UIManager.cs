using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Иконки победа/проигрыш")]
    public GameObject winPanel, losePanel;

    // текст написанный игроком
    [HideInInspector]
    public string inputText;

    // поле ввода
    public InputField inputField;

    static public UIManager instance;

    private void Awake()
    {
        if (UIManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        UIManager.instance = this;
    }
    private void Update()
    {
        // обновляем введенный текст
        inputText = inputField.text;
    }
    /// <summary>
    /// Останавливает игру и включает необходимую окно
    /// </summary>
    /// <param name="panelName">Окно победы/поражения</param>
    public void WinLosePanel(GameObject panelName)
    {
        Time.timeScale = 0f;
        inputField.gameObject.SetActive(false);
        panelName.SetActive(true);
    }

    /// <summary>
    /// Перезапуск уровня
    /// </summary>
    public void AgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
