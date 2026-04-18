using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText;

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        if (messageText != null)
            messageText.text = "";
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        ShowMessage("¡Acierto! +10");
    }

    public void ShowMissMessage()
    {
        ShowMessage("¡Fallaste!");
    }

    public void ShowOvershootMessage()
    {
        ShowMessage("¡Demasiada fuerza!");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + score;
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            CancelInvoke(nameof(ClearMessage));
            Invoke(nameof(ClearMessage), 1.5f);
        }
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}
