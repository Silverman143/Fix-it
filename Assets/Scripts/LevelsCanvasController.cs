using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FixItGame
{
    public class LevelsCanvasController : MonoBehaviour
    {
       public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
