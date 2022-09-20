using UnityEngine;
using UnityEngine.SceneManagement;

namespace World
{
    public class MainMenu: MonoBehaviour {  
    
        public void PlayGame() {
            SceneManager.LoadScene("GameScene");  
        }  
    }
} 