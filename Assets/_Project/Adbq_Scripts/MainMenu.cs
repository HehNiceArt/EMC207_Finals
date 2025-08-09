using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] string sceneToLoad = "GameScene";

        public void LoadGameScene()
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
        public void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}
