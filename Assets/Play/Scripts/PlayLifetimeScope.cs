using System;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Play.Scripts
{
    public class PlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject boxPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .As<IPlayerInput>()
                .As<ITickable>()
                .As<IDisposable>();
            builder.Register<MazeGenerator>(Lifetime.Singleton)
                .As<IMazeGenerator>();
            builder.Register<PlayManager>(Lifetime.Singleton)
                .As<IPlayManager>()
                .As<IStartable>();
            builder.RegisterComponentInHierarchy<PlayerCameraController>()
                .As<IPlayerCamera>();
            builder.RegisterComponentInHierarchy<PlayerController>();
            builder.RegisterComponentInHierarchy<GenerateStage>();
        }
    }
}
