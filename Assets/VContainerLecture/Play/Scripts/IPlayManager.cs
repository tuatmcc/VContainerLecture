using System;
using VContainerLecture.Core.Scripts;

namespace VContainerLecture.Play.Scripts
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
