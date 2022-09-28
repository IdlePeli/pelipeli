using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}