using UnityEngine;
namespace VContainerLecture.Play.Scripts
{
    public interface IMazeGenerator
    {
        public bool[,] GenerateMaze(int width, int height);
    }
}
