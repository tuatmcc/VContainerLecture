using UnityEngine;

namespace VContainerLecture.Play.Scripts
{
    public interface IPlayerCamera
    {
        Vector3 Forward { get; }
        Vector3 Right { get; }
    }
}
