using NUnit.Framework;
using VContainerLecture.Core.Scripts;
using VContainerLecture.Play.Scripts;

namespace VContainerLecture.Tests.Editor
{
    public class GameFlowAndPlayManagerTests
    {
        private sealed class SceneLoaderStub : ISceneLoader
        {
            public GameState LastLoadedState { get; private set; }
            public int LoadCount { get; private set; }

            public void LoadScene(GameState gameState)
            {
                LastLoadedState = gameState;
                LoadCount++;
            }
        }

        [Test]
        public void GameFlow_TransitionsTitlePlayResultTitle_AndLoadsEachScene()
        {
            var sceneLoader = new SceneLoaderStub();
            var gameFlowManager = new GameFlowManager(sceneLoader);

            Assert.That(gameFlowManager.NextState(TransitionType.Enter), Is.EqualTo(GameState.Play));
            Assert.That(sceneLoader.LastLoadedState, Is.EqualTo(GameState.Play));

            Assert.That(gameFlowManager.NextState(TransitionType.Exit), Is.EqualTo(GameState.Result));
            Assert.That(sceneLoader.LastLoadedState, Is.EqualTo(GameState.Result));

            Assert.That(gameFlowManager.NextState(TransitionType.Enter), Is.EqualTo(GameState.Title));
            Assert.That(sceneLoader.LastLoadedState, Is.EqualTo(GameState.Title));
            Assert.That(sceneLoader.LoadCount, Is.EqualTo(3));
        }

        [Test]
        public void PlayManager_CompletionFinishesPlay_AndMovesGameFlowToResult()
        {
            var sceneLoader = new SceneLoaderStub();
            var gameFlowManager = new GameFlowManager(sceneLoader);
            gameFlowManager.NextState(TransitionType.Enter);
            var playManager = new PlayManager(gameFlowManager);

            playManager.NextState(TransitionType.Enter);
            playManager.CompletePlay();

            Assert.That(playManager.CurrentPlayState, Is.EqualTo(PlayState.Finished));
            Assert.That(gameFlowManager.CurrentState, Is.EqualTo(GameState.Result));
            Assert.That(sceneLoader.LastLoadedState, Is.EqualTo(GameState.Result));
        }
    }
}
