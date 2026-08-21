using System;
using UnityEngine.SceneManagement;
namespace VContainerLecture.Core.Scripts
{
    public class GameFlowManager : IGameFlowManager
    {
        public event Action<GameState> OnGameStateChange;
        public GameState CurrentState { get; private set; }

        private ISceneLoader _sceneLoader;

        public GameFlowManager(ISceneLoader sceneLoader)
        {
            // TODO: 注入されたISceneLoaderをフィールドへ代入する
            // _sceneLoader = sceneLoader;
            CurrentState = SceneManager.GetActiveScene().name switch
            {
                "PlayScene" => GameState.Play,
                "ResultScene" => GameState.Result,
                _ => GameState.Title,
            };
        }

        public GameState NextState(TransitionType transitionType)
        {
            var nextState = CurrentState;

            if (CurrentState == GameState.Title && transitionType == TransitionType.Enter)
            {
                nextState = GameState.Play;
            }
            else if (CurrentState == GameState.Play && transitionType == TransitionType.Exit)
            {
                nextState = GameState.Result;
            }
            else if (CurrentState == GameState.Result && transitionType == TransitionType.Enter)
            {
                nextState = GameState.Title;
            }

            if (nextState == CurrentState)
                return CurrentState;

            CurrentState = nextState;
            OnGameStateChange?.Invoke(CurrentState);
            _sceneLoader.LoadScene(CurrentState);

            return CurrentState;
        }
    }
}
