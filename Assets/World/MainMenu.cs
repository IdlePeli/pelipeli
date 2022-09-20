using UnityEngine;
using UnityEngine.SceneManagement;

namespace World
{
    public class MainMenu: MonoBehaviour {  
    
        public void PlayGame() { 
            Debug.Log("kkkkkkk");
            SceneManager.LoadScene("GameScene");  
        }  
    }
} 