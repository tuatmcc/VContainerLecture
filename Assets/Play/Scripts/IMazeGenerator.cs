using UnityEngine;
namespace Play.Scripts
{
    public interface IMazeGenerator
    {
        public bool[,] GenerateMaze(int width, int height);
    }
}