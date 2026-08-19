using System;
using UnityEngine;
using VContainer.Unity;
using VContainerLecture.Core.Scripts;

namespace VContainerLecture.Play.Scripts
{
    /// <summary>
    /// PlayManager の状態遷移を確認するための、ログ出力用実装です。
    /// </summary>
    public class TestPlayManager : IPlayManager, IStartable
    {
        public event Action<PlayState> OnPlayStateChange;

        public PlayState CurrentPlayState { get; private set; }

        private readonly IGameFlowManager _gameFlowManager;

        public TestPlayManager(IGameFlowManager gameFlowManager)
        {
            _gameFlowManager = gameFlowManager;
            CurrentPlayState = PlayState.GenerateStage;
        }

        public void Start()
        {
            Debug.Log($"{nameof(TestPlayManager)}: Start ({CurrentPlayState})");
            OnPlayStateChange?.Invoke(CurrentPlayState);
        }

        public void CompletePlay()
        {
            Debug.Log($"{nameof(TestPlayManager)}: CompletePlay ({CurrentPlayState})");

            if (CurrentPlayState != PlayState.Playing)
            {
                return;
            }

            NextState(TransitionType.Enter);
            _gameFlowManager.NextState(TransitionType.Exit);
        }

        public PlayState NextState(TransitionType transitionType)
        {
            Debug.Log($"{nameof(TestPlayManager)}: NextState ({CurrentPlayState}, {transitionType})");

            if (CurrentPlayState == PlayState.GenerateStage)
            {
                if (transitionType == TransitionType.Enter)
                {
                    CurrentPlayState = PlayState.Playing;
                    OnPlayStateChange?.Invoke(CurrentPlayState);
                }
                else if (transitionType == TransitionType.Exit)
                {
                    CurrentPlayState = PlayState.GenerateStage;
                    OnPlayStateChange?.Invoke(CurrentPlayState);
                }
            }
            else if (CurrentPlayState == PlayState.Playing)
            {
                if (transitionType == TransitionType.Enter)
                {
                    CurrentPlayState = PlayState.Finished;
                    OnPlayStateChange?.Invoke(CurrentPlayState);
                }
                else if (transitionType == TransitionType.Exit)
                {
                    CurrentPlayState = PlayState.GenerateStage;
                    OnPlayStateChange?.Invoke(CurrentPlayState);
                }
            }
            else if (CurrentPlayState == PlayState.Finished)
            {
                CurrentPlayState = PlayState.GenerateStage;
                OnPlayStateChange?.Invoke(CurrentPlayState);
            }

            Debug.Log($"{nameof(TestPlayManager)}: Current state ({CurrentPlayState})");
            return CurrentPlayState;
        }
    }
}
