using UnityEngine;

namespace VContainerLecture.Play.Scripts
{
    [CreateAssetMenu(fileName = "PlaySettings", menuName = "Scriptable Objects/PlaySettings")]
    public class PlaySettings : ScriptableObject
    {
        [field: SerializeField] public float LookSensitivity { get; private set; } = 0.1f;
        [field: SerializeField] public float MinPitch { get; private set; } = -35f;
        [field: SerializeField] public float MaxPitch { get; private set; } = 65f;

        [field: SerializeField] public float TargetHeight { get; private set; } = 1.5f;
        [field: SerializeField] public float DefaultDistance { get; private set; } = 5f;
        [field: SerializeField] public float MinDistance { get; private set; } = 0.8f;
        [field: SerializeField] public float CameraRadius { get; private set; } = 0.25f;

        [field: SerializeField] public float CollisionInSmooth { get; private set; } = 0.03f;
        [field: SerializeField] public float CollisionOutSmooth { get; private set; } = 0.15f;
        [field: SerializeField] public LayerMask CollisionLayers { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; } = 1.0f;
    }
}
