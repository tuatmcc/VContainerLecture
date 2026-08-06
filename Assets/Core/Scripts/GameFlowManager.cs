using Core.Scripts;
using UnityEngine;

namespace Core.Scripts
{
    public class GameFlowManager : IGameFlowManager
    {
        public GameState CurrentState { get; private set; }

        public GameFlowManager()
        {
            CurrentState = new GameState();
            CurrentState = GameState.Title;
        }
        public GameState NextState(TransitionType transitionType)
        {
            if(CurrentState == GameState.Title && transitionType == TransitionType.Enter)
            {
                return GameState.InGame;
            }
            else if(CurrentState == GameState.InGame && transitionType == TransitionType.Exit)
            {
                return GameState.EndGame;
            }
            else if(CurrentState == GameState.EndGame && transitionType == TransitionType.Exit)
            {
                return GameState.Title;
            }
            else
            {
                return CurrentState;
            }
        } 
    }
}
