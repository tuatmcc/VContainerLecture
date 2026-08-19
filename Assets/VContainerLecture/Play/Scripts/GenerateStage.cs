using UnityEngine;
using R3;
using VContainer;
using VContainerLecture.Core.Scripts;

namespace VContainerLecture.Play.Scripts
{
    public class GenerateStage : MonoBehaviour
    {
        [SerializeField] private Transform mazeStartPos;
        [SerializeField] private Transform mazeGoalPos;
        
        [SerializeField] private GameObject Cube;
        private IMazeGenerator _mazeGenerator;
        private IPlayManager _playManager;
        
        [Inject]
        public void Construct(IMazeGenerator mazeGenerator,  IPlayManager playManager)
        {
            _mazeGenerator = mazeGenerator;
            _playManager = playManager;

            Observable.FromEvent<PlayState>(
                h => _playManager.OnPlayStateChange += h,
                h => _playManager.OnPlayStateChange -= h)
                .Where(state => state == PlayState.GenerateStage)
                .Subscribe(_ => GenerateMaze())
                .AddTo(this);
        }

        private void GenerateMaze()
        {
            var start = mazeStartPos.position;
            var goal = mazeGoalPos.position;

            var directionX = goal.x >= start.x ? 1 : -1;
            var directionZ = goal.z >= start.z ? 1 : -1;
            var mazeWidth = Mathf.Abs(Mathf.RoundToInt(goal.x - start.x));
            var mazeHeight = Mathf.Abs(Mathf.RoundToInt(goal.z - start.z));
            if (mazeWidth % 2 == 0)
            {
                mazeWidth++;
            }

            if (mazeHeight % 2 == 0)
            {
                mazeHeight++;
            }
            var maze = _mazeGenerator.GenerateMaze(mazeWidth, mazeHeight);
            maze[1, 1] = false;
            maze[mazeWidth - 2, mazeHeight - 2] = false;
            for (int x = 0; x <  mazeWidth; x++)
            {
                for (int z = 0; z <  mazeHeight; z++)
                {
                    if (maze[x, z])
                    {
                        var position = ToWorldPosition(start, directionX, directionZ, x, z);
                        Instantiate(Cube, position, Quaternion.identity); 
                    }
                }
            }

            _playManager.NextState(TransitionType.Enter);
        }

        private Vector3 ToWorldPosition(Vector3 start, int directionX, int directionZ, int diffX, int diffZ)
        {
            return new Vector3(start.x + directionX * (diffX-1), start.y, start.z + directionZ * (diffZ-1));
        } 
    }
}
