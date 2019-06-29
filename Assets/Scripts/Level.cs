using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] int delayInSecondsGameOver = 2;

    private void ResetGameSession()
    {
        var gs = FindObjectOfType<GameSession>();
        if (gs)
        {
            gs.ResetGame();
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
        ResetGameSession();
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene(0);
        ResetGameSession();
    }

    public void LoadGameOverScreen()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(delayInSecondsGameOver);
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
