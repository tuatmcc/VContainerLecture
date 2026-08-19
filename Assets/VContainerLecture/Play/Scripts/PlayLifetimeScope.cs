using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VContainerLecture.Play.Scripts
{
    public class PlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private PlaySettings playSettings;
        [SerializeField] private bool isTest;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(playSettings);
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .As<IPlayerInput>()
                .As<ITickable>()
                .As<IDisposable>();
            builder.Register<MazeGenerator>(Lifetime.Singleton)
                .As<IMazeGenerator>();
            if (isTest)
            {
                builder.Register<TestPlayManager>(Lifetime.Singleton)
                    .As<IPlayManager>()
                    .As<IStartable>();
            }
            else
            {
                builder.Register<PlayManager>(Lifetime.Singleton)
                    .As<IPlayManager>()
                    .As<IStartable>();
            }
            builder.RegisterComponentInHierarchy<PlayerCameraController>()
                .As<IPlayerCamera>();
            builder.RegisterComponentInHierarchy<PlayerController>();
            builder.RegisterComponentInHierarchy<GenerateStage>();
            builder.RegisterComponentInHierarchy<GoalTrigger>();
        }
    }
}
