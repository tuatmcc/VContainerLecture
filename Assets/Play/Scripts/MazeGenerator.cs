using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using System;
using Random = System.Random;

namespace Play.Scripts
{
    public class MazeGenerator : IMazeGenerator
    {
        public bool[,] Maze { get;  private set; }

        private Vector2Int[] Directions =
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
        };

        private void Shuffle(IList<Vector2Int> directions)
        {
            for (int i = directions.Count - 1; i >= 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i+1);
                (directions[i], directions[j]) = (directions[j], directions[i]);
            }
        }

        private void Dig(Vector2Int current, int width, int height)
        {
            Maze[current.x, current.y] = false;
            var directions = new List<Vector2Int>(Directions);     
            Shuffle(directions);
            foreach (var dir in directions)
            {
                Vector2Int v2 = current + dir * 2;
                if (v2.x <= 0 || v2.x >= width - 1 || v2.y <= 0 || v2.y >= height - 1)
                {
                    continue;
                }

                if (!Maze[v2.x, v2.y])
                {
                    continue; 
                }
                var v1 = current + dir;
                Maze[v1.x, v1.y] = false;
                Dig(v2, width, height);
            }
        }
        public bool[,] GenerateMaze(int width, int height)
        {
            if (width % 2 == 0)
            {
                width--;
            }

            if (height % 2 == 0)
            {
                height--;
            }
            Maze = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Maze[x, y] = true;
                }
            }
            Dig(new Vector2Int(1, 1), width, height);
            Maze[width - 2, height - 2] = false;
            
            return Maze;
        }
    }
}