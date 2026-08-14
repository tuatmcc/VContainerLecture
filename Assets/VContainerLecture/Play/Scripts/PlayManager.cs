using System;
using VContainerLecture.Core.Scripts;
using VContainer.Unity;

namespace VContainerLecture.Play.Scripts
{
    public class PlayManager : IPlayManager, IStartable
    {
        public event Action<PlayState> OnPlayStateChange; 
        
        public PlayState CurrentPlayState { get; private set; }

        public PlayManager()
        {
            CurrentPlayState = PlayState.GenerateStage; 
        }

        public void Start()
        {
            OnPlayStateChange?.Invoke(CurrentPlayState);
        }

        public PlayState NextState(TransitionType transitionType)
        {
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
                if(transitionType == TransitionType.Enter)
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
                if(transitionType == TransitionType.Enter)
                {
                    CurrentPlayState = PlayState.GenerateStage; 
                    OnPlayStateChange?.Invoke(CurrentPlayState);
                }
                else if (transitionType == TransitionType.Exit)
                {
                    CurrentPlayState = PlayState.GenerateStage;
                    OnPlayStateChange?.Invoke(CurrentPlayState);
                }
            }
            return CurrentPlayState;
        }
    }
}
