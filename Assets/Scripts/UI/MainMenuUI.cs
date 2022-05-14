using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("quitBtn").GetComponent<Button>().onClick.AddListener(Application.Quit);

        transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(
            // () => GameSceneManager.Load(GameSceneManager.Scene.GameScene)
            () => TransitionCanvas.Instance.MakeTransition(GameSceneManager.Scene.GameScene)
        );
    }
}