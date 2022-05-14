using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        transform.Find("retryBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            // GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
            TransitionCanvas.Instance.MakeTransition(GameSceneManager.Scene.MainMenuScene);
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        transform.Find("wavesSurvivedText").GetComponent<TextMeshProUGUI>()
            .SetText("You Survived " + EnemyWaveManager.Instance.GetWaveNumber() + " Waves!");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}