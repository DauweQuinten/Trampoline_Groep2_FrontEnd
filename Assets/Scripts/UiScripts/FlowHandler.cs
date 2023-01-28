using Models;
using UnityEngine.SceneManagement;

namespace UiScripts
{
    public static class FlowHandler
    {
        public static void LoadNextSceneInstantly(string sceneName)
        {
            ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
            ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
            SceneManager.LoadScene(sceneName);
        }
    }
}