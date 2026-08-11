using UnityEngine;

namespace Play.Scripts
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        bool JumpPressed { get; }
    }
}