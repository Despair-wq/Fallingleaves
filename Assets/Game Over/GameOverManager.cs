using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // این متد بازی را از نو شروع می‌کند
    public void RestartGame()
    {
        // نام دقیق صحنه بازی‌تان ("Game")
        SceneManager.LoadScene("Game");
    }

    // این متد بازیکن را به منوی اصلی برمی‌گرداند
    public void GoToMainMenu()
    {
        // نام دقیق صحنه منوی اصلی‌تان ("Main Menu")
        SceneManager.LoadScene("Main Menu");
    }
}