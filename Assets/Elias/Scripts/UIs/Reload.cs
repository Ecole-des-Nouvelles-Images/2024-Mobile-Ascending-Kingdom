using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elias.Scripts.UIs
{
    public class Reload : MonoBehaviour
    {
        public void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
