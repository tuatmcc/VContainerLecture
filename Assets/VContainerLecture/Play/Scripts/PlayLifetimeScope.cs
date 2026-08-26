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
            // TODO: PlayerInputをVContainerのEntryPointとして登録する
            // builder.Register<PlayerInput>(Lifetime.Singleton)
            //     .As<IPlayerInput>()
            //     .As<ITickable>()
            //     .As<IDisposable>();
            builder.Register<MazeGenerator>(Lifetime.Singleton)
                .As<IMazeGenerator>();

            if (isTest)
            {
                builder.RegisterEntryPoint<TestPlayManager>();
            }
            else
            {
                builder.RegisterEntryPoint<PlayManager>();
            }

            builder.RegisterComponentInHierarchy<PlayerCameraController>()
                .As<IPlayerCamera>();
            // TODO: シーン上のPlayerControllerをコンテナへ登録する
            // builder.RegisterComponentInHierarchy<PlayerController>();
            builder.RegisterComponentInHierarchy<GenerateStage>();
            builder.RegisterComponentInHierarchy<GoalTrigger>();
        }
    }
}
