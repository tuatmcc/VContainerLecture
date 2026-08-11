using System;

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
        public PlayState PlayState { get; }
    }
}