using UnityEngine;

public class PuzzleMap : MonoBehaviour
{
    public GameObject[] Blocks;
    public int Columns;
    public int Rows;

    bool[,] Map;

    void Awake()
    {
        Map = new bool[Columns, Rows];

        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                var firstOrLastOfColumn = y == 0 || y == Rows - 1;
                var firstOrLastOfRow = x == 0 || x == Columns - 1;

                int index;
                if (firstOrLastOfColumn && firstOrLastOfRow)
                {
                    index = 2;
                }
                else if (firstOrLastOfColumn != firstOrLastOfRow)
                {
                    index = 1;
                }
                else
                {
                    continue;
                }

                Instantiate(Blocks[index], new Vector3(x, 0, y), Quaternion.identity);
                Map[x, y] = true;
            }
        }

        for (var i = 0; i < 10; i++)
        {
            TryInsertBlock(
                Random.Range(0, Blocks.Length),
                Random.Range(1, Columns - 1),
                Random.Range(1, Rows - 1)
            );
        }
    }

    void TryInsertBlock(int id, int posX, int posY)
    {
        var gameObject = Instantiate(Blocks[id], new Vector3(0, 0, 0), Quaternion.identity);
        var block = gameObject.GetComponent<PuzzleBlock>();
        var allowed = true;

        for (var x = posX; x < posX + block.Columns; x++)
        {
            for (var y = posY; y < posY + block.Rows; y++)
            {
                if (Map[x, y])
                {
                    allowed = false;
                    break;
                }
            }

            if (!allowed)
            {
                break;
            }
        }

        if (allowed)
        {
            gameObject.transform.position = new Vector3(posX, 0, posY);
            for (var x = posX; x < posX + block.Columns; x++)
            {
                for (var y = posY; y < posY + block.Rows; y++)
                {
                    Map[x, y] = true;
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
