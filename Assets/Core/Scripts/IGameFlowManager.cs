using System;

namespace Core.Scripts
{
    public enum GameState
    {
        Title,
        Play,
        Result,
    }

    public enum TransitionType
    {
        Enter,
        Exit
    }
    public interface IGameFlowManager
    {
        public event Action<GameState> OnGameStateChange;
        public GameState CurrentState { get; }
        GameState NextState(TransitionType  transitionType);
    } 
}