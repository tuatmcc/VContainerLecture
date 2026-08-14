using System;
using VContainerLecture.Core.Scripts;
using UnityEngine;

namespace VContainerLecture.Core.Scripts
{
    public class GameFlowManager : IGameFlowManager
    {
        public event Action<GameState> OnGameStateChange;
        public GameState CurrentState { get; private set; }

        public GameFlowManager()
        {
            CurrentState = new GameState();
            CurrentState = GameState.Title;
        }
        public GameState NextState(TransitionType transitionType)
        {
            if (CurrentState == GameState.Title)
            {
                if(transitionType == TransitionType.Enter)
                {
                    return CurrentState = GameState.Play;
                }
                else if (transitionType == TransitionType.Exit)
                {
                    return CurrentState;
                }
            }
            else if (CurrentState == GameState.Play)
            {
                if (transitionType == TransitionType.Enter)
                {
                    return CurrentState = GameState.Play;
                }
                else if (transitionType == TransitionType.Exit)
                {
                    return CurrentState =  GameState.Title;
                }
            }
            else if (CurrentState == GameState.Result)
            {
                return CurrentState = GameState.Title;
            }
            return CurrentState;
        } 
    }
}
