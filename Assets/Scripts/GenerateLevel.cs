using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateLevel : MonoBehaviour
{

    public GameObject cube;
    public GameObject ground;
    public GameObject start;
    public GameObject finish;

    public GameObject player;

    public GameObject enemy;

    public NavMeshSurface surface;
    public int height = 20;
    public int width = 20;

    int minRoomHeight = 2;
    int minRoomWidth = 2;
    int maxRoomHeight;
    int maxRoomWidth;

    int numEnemies;


    byte[,] map;

    // Use this for initialization
    void Start()
    {
        width = 20 + GManager.Instance.Level * 2;
        height = 20 + GManager.Instance.Level * 2;

        numEnemies = (height / 5) * (width / 5);

        map = new byte[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = 0;
            }
        }

        maxRoomHeight = height / 3;
        maxRoomWidth = width / 3;

        int filled = 0;

        int rh = Random.Range(minRoomHeight, maxRoomHeight + 1);
        int rw = Random.Range(minRoomWidth, maxRoomWidth + 1);

        MakeRoom(width / 2, height / 2, rw, rh);

        filled += rh * rw;

        while (filled < (height * width) * 0.6)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            direction d;

            if (CheckAdjWall(x, y, out d))
            {
                int dist = Random.Range(Mathf.Max(minRoomHeight, minRoomWidth), Mathf.Min(maxRoomHeight, maxRoomWidth));

                rh = Random.Range(minRoomHeight, maxRoomHeight + 1);
                rw = Random.Range(minRoomWidth, maxRoomWidth + 1);

                switch (d)
                {
                    case direction.north:
                        if (RoomIsPossible(x, y + dist, rw + 1, rh + 1))
                        {
                            MakeRoom(x, y + dist, rw, rh);
                            filled += rh * rw;
                            for (int i = 0; i < dist; i++)
                            {
                                map[x, y + i] = 1;
                            }
                        }
                        break;
                    case direction.east:
                        if (RoomIsPossible(x + dist, y, rw + 1, rh + 1))
                        {
                            MakeRoom(x + dist, y, rw, rh);
                            filled += rh * rw;
                            for (int i = 0; i < dist; i++)
                            {
                                map[x + i, y] = 1;
                            }
                        }
                        break;
                    case direction.south:
                        if (RoomIsPossible(x, y - dist, rw + 1, rh + 1))
                        {
                            MakeRoom(x, y - dist, rw, rh);
                            filled += rh * rw;
                            for (int i = 0; i < dist; i++)
                            {
                                map[x, y - i] = 1;
                            }
                        }
                        break;
                    case direction.west:
                        if (RoomIsPossible(x - dist, y, rw + 1, rh + 1))
                        {
                            MakeRoom(x - dist, y, rw, rh);
                            filled += rh * rw;
                            for (int i = 0; i < dist; i++)
                            {
                                map[x - i, y] = 1;
                            }
                        }
                        break;
                }

            }

        }

        int sx = Random.Range(0, width);
        int sy = Random.Range(0, height);

        while (map[sx, sy] == 0)
        {
            sx = Random.Range(0, width);
            sy = Random.Range(0, height);
        }

        map[sx, sy] = 2;

        int fx = Random.Range(0, width);
        int fy = Random.Range(0, height);

        while (map[fx, fy] != 1 || Mathf.Sqrt((fx - sx) * (fx - sx) + (fy - sy) * (fy - sy)) < 5)
        {
            fx = Random.Range(0, width);
            fy = Random.Range(0, height);
        }

        map[fx, fy] = 3;

        for (int l = 0; l < numEnemies; l++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while (map[x, y] != 1 || Mathf.Sqrt((x - sx) * (x - sx) + (y - sy) * (y - sy)) < 3)
            {
                Debug.Log("Retry");
                x = Random.Range(0, width);
                y = Random.Range(0, height);
            }

            map[x, y] = 4;

            Debug.Log(l);
        }

        FillScene();

        Vector3 startPoint = GameObject.FindGameObjectWithTag("Start").GetComponent<Transform>().position;

        GManager.Instance.Level = GManager.Instance.Level;

        Instantiate(player, startPoint, Quaternion.identity);

    }

    bool RoomIsPossible(int x, int y, int rwidth, int rheight)
    {
        if (x - rwidth / 2 < 0 || y - rheight / 2 < 0 || x + rwidth / 2 >= width || y + rheight / 2 >= height)
            return false;

        for (int i = x - rwidth / 2; i < x + rwidth / 2; i++)
        {
            for (int j = y - rheight / 2; j < y + rheight / 2; j++)
            {
                if (map[i, j] == 1)
                    return false;
            }
        }
        return true;
    }

    void MakeRoom(int x, int y, int rwidth, int rheight)
    {
        for (int i = x - rwidth / 2; i < x + rwidth / 2; i++)
        {
            for (int j = y - rheight / 2; j < y + rheight / 2; j++)
            {
                map[i, j] = 1;
            }
        }
    }

    void FillScene()
    {
        int cubeScale = (int)cube.GetComponent<Transform>().localScale.x;
        int groundScale = (int)ground.GetComponent<Transform>().localScale.x;

        for (int i = -1; i < height + 1; i++)
        {
            Instantiate(cube, new Vector3(-1 * cubeScale, cubeScale / 2, i * cubeScale), Quaternion.identity);
            Instantiate(cube, new Vector3(width * cubeScale, cubeScale / 2, i * cubeScale), Quaternion.identity);
        }

        for (int i = 0; i < width; i++)
        {
            Instantiate(cube, new Vector3(i * cubeScale, cubeScale / 2, -1 * cubeScale), Quaternion.identity);
            Instantiate(cube, new Vector3(i * cubeScale, cubeScale / 2, height * cubeScale), Quaternion.identity);
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                switch (map[i, j])
                {
                    case 0:
                        Instantiate(cube, new Vector3(i * cubeScale, cubeScale / 2, j * cubeScale), Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(start, new Vector3(i * cubeScale, cubeScale / 2, j * cubeScale), Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(finish, new Vector3(i * cubeScale, 0, j * cubeScale), Quaternion.identity);
                        break;
                    default:
                        break;
                }

            }
        }

        for (int i = -5; i <= (width / groundScale) + 5; i++)
        {
            for (int j = -5; j <= (height / groundScale) + 5; j++)
            {
                Instantiate(ground, new Vector3(i * groundScale * 10 + 5, 0, j * groundScale * 10 + 5), Quaternion.identity);
            }
        }

        surface.BuildNavMesh();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] == 4)
                    Instantiate(enemy, new Vector3(i * cubeScale, 1, j * cubeScale), Quaternion.identity);
            }
        }
    }

    bool CheckAdjWall(int x, int y, out direction dir)
    {
        dir = direction.north;

        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
            return false;

        if (map[x, y] == 0)
        {
            if (map[x - 1, y] > 0)
            {
                dir = direction.east;
                return true;
            }
            if (map[x + 1, y] > 0)
            {
                dir = direction.west;
                return true;
            }
            if (map[x, y - 1] > 0)
            {
                dir = direction.north;
                return true;
            }
            if (map[x, y + 1] > 0)
            {
                dir = direction.south;
                return true;
            }

            return false;
        }
        else
            return false;
    }

    public enum direction
    {
        north,
        east,
        south,
        west
    }

}
