using System;
using VContainer;
using VContainer.Unity;

namespace Play.Scripts
{
    public class PlayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .As<IPlayerInput>()
                .As<ITickable>()
                .As<IDisposable>();
            builder.RegisterComponentInHierarchy<PlayerCameraController>()
                .As<IPlayerCamera>();
            builder.RegisterComponentInHierarchy<PlayerController>();
        }
    }
}
