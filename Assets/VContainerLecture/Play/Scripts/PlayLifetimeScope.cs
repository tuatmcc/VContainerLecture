using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VContainerLecture.Play.Scripts
{
    public class PlayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private PlaySettings playSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(playSettings);
            builder.RegisterEntryPoint<PlayerInput>();
            builder.Register<MazeGenerator>(Lifetime.Singleton)
                .As<IMazeGenerator>();

            builder.RegisterEntryPoint<PlayManager>();
            // TODO: isTestの値に応じてPlayManagerとTestPlayManagerを切り替えて登録する

            builder.RegisterComponentInHierarchy<PlayerCameraController>()
                .As<IPlayerCamera>();
            builder.RegisterComponentInHierarchy<PlayerController>();
            builder.RegisterComponentInHierarchy<GenerateStage>();
            builder.RegisterComponentInHierarchy<GoalTrigger>();
        }
    }
}
