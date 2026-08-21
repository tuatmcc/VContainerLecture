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
            // builder.RegisterInstance(playSettings);
            builder.RegisterEntryPoint<PlayerInput>();
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

            builder.RegisterComponentInHierarchy<PlayerCameraController>();
            builder.RegisterComponentInHierarchy<PlayerController>();
            builder.RegisterComponentInHierarchy<GenerateStage>();
            builder.RegisterComponentInHierarchy<GoalTrigger>();
        }
    }
}
