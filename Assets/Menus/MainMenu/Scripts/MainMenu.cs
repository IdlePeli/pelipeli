using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.MainMenu.Scripts
{
    public class MainMenu: MonoBehaviour {  
        public void PlayGame() {
            SceneManager.LoadScene("GameScene");  
        }  
    }
} 