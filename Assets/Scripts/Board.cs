
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public GameObject tilePrefab;
    public int height;

    private BackgroundTile[,] allTiles;
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        // SetUp();
    }

    // Update is called once per frame
    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //for instantiate  Object , Vector3 position, Quaternion rotation
                Vector2 tempPosition = new Vector2(i, j);
                Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                //quaternion.identity regular roatation
            }
        }

    }
}
