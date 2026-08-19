using System;
using VContainer;
using VContainer.Unity;
using VContainerLecture.Core.Scripts;

namespace VContainerLecture.Result.Scripts
{
    public class ResultLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MenuInput>(Lifetime.Singleton)
                .As<IMenuInput>()
                .As<IDisposable>();
            builder.RegisterEntryPoint<ResultManager>();
        }
    }
}
