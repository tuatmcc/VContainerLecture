using System;
using Core.Scripts;

namespace Play.Scripts
{
    public enum PlayState
    {
        GenerateStage,
        Playing,
        Finished
    }
    public interface IPlayManager
    {
        public event Action<PlayState>  OnPlayStateChange;
        public PlayState CurrentPlayState { get; }

        public PlayState NextState(TransitionType transitionType);
    }
}