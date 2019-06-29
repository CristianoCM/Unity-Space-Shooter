using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score;

    private void Awake()
    {
        ScoreSingleton();
    }

    private void ScoreSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

}
