using UnityEngine.SceneManagement;

namespace Core.Scripts
{
    public class SceneLoader : ISceneLoader
    {
        public void LoadScene(GameState gameState)
        {
            var sceneName = gameState switch
            {
                GameState.Title => "TitleScene",
                GameState.Play => "PlayScene",
                GameState.Result => "ResultScene",
                _ => "TitleScene"
            };
            
            SceneManager.LoadScene(sceneName);
        }
    }
}