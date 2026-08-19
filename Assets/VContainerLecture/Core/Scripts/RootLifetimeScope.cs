using VContainer;
using VContainer.Unity;

namespace VContainerLecture.Core.Scripts
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.Register<GameFlowManager>(Lifetime.Singleton).As<IGameFlowManager>();
        }
    }
}
