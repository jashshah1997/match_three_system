using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private RowController[] m_rows;
    private TileController[,] m_tiles;

    private List<TileController> m_selected_tiles = new List<TileController>();
    private List<Vector3> m_selected_initial_transforms = new List<Vector3>();

    private readonly float MAX_SWAP_TIME = 0.5f;
    private float swapTime;
    private bool selectionDisabled = false;

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

    public void setSelected(TileController selectedTile)
    {
        if (selectionDisabled) return;

        if (!m_selected_tiles.Contains(selectedTile)) m_selected_tiles.Add(selectedTile);

        if (m_selected_tiles.Count < 2) return;

        Debug.Log($"({ m_selected_tiles[0].x }, {m_selected_tiles[0].y} ) and ({ m_selected_tiles[1].x }, {m_selected_tiles[1].y} )");
        m_selected_initial_transforms.Add(m_selected_tiles[0].transform.position);
        m_selected_initial_transforms.Add(m_selected_tiles[1].transform.position);
        swapSelectedTiles();
    }

    public void swapSelectedTiles()
    {
        swapTime = 0;
        InvokeRepeating("animateSwap", 0, 0.01f);
    }

    private void animateSwap()
    {
        selectionDisabled = true;
        swapTime += Time.deltaTime / MAX_SWAP_TIME;
        m_selected_tiles[0].transform.position = 
            Vector3.Lerp(m_selected_initial_transforms[0],
            m_selected_initial_transforms[1], swapTime);
        m_selected_tiles[1].transform.position = 
            Vector3.Lerp(m_selected_initial_transforms[1], 
            m_selected_initial_transforms[0], swapTime);

        if (swapTime >= 1)
        {
            CancelInvoke("animateSwap");
            Debug.Log("Done!");
            selectionDisabled = false;
            m_selected_tiles.Clear();
            m_selected_initial_transforms.Clear();
        }
    }
}
