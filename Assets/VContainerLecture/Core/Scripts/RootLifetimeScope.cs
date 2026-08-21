using VContainer;
using VContainer.Unity;

namespace VContainerLecture.Core.Scripts
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // TODO: SceneLoaderをISceneLoaderとして登録する
            // builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            // TODO: GameFlowManagerをIGameFlowManagerとして登録する
            // builder.Register<GameFlowManager>(Lifetime.Singleton).As<IGameFlowManager>();
        }
    }
}
