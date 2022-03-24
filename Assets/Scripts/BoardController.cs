using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private RowController[] m_rows;
    private TileController[,] m_tiles;

    // Start is called before the first frame update
    void Start()
    {
        m_rows = GameObject.FindObjectsOfType<RowController>();
        m_tiles = new TileController[m_rows[0].Tiles.Count, m_rows.Length];

        // Initialize tiles
        for (int j = 0; j < getBoardHeight(); j++)
        {
            for (int i = 0; i < getBoardLength(); i++)
            {   
                var tile = m_rows[j].Tiles[i];
                m_tiles[i, j] = tile;
                tile.SetItem(ItemGenerator.GoodItems[Random.Range(0, ItemGenerator.GoodItems.Length)]);
                tile.x = i;
                tile.y = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getBoardHeight()
    {
        return m_tiles.GetLength(1);
    }

    public int getBoardLength()
    {
        return m_tiles.GetLength(0);
    }
}
