using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public List<RowController> Rows;
    public TileController[,] Tiles;

    private AudioSource m_match_sound;
    private GameManager m_game_manager;
    private List<TileController> m_selected_tiles = new List<TileController>();
    private List<Vector3> m_selected_initial_transforms = new List<Vector3>();

    private readonly float MAX_SWAP_TIME = 0.5f;
    private float swapTime;
    private int swapCounter;
    private bool selectionDisabled = true;
    private bool game_paused = true;
    private int match_count = 3;

    private void Awake()
    {
        Tiles = new TileController[Rows[0].Tiles.Count, Rows.Count];
        m_game_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_match_sound = GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        // Initialize tiles
        for (int j = 0; j < getBoardHeight(); j++)
        {
            for (int i = 0; i < getBoardLength(); i++)
            {
                var tile = Rows[j].Tiles[i];
                Tiles[i, j] = tile;
                tile.SetItem(ItemGenerator.GenerateForMatchCount(match_count));
                tile.AnimateInflate();
                tile.x = i;
                tile.y = j;
            }
        }

        // Initial clear of the board
        StartCoroutine(MatchAndReplace());
        game_paused = false;
    }

    public int getBoardHeight()
    {
        return Tiles.GetLength(1);
    }

    public int getBoardLength()
    {
        return Tiles.GetLength(0);
    }

    public void SetMatchCount(int newMatchCount)
    {
        match_count = newMatchCount;
    }

    public void setSelected(TileController selectedTile)
    {
        if (game_paused) return;

        if (selectionDisabled) return;

        if (m_selected_tiles.Contains(selectedTile)) return;

        if (selectedTile.Item.isObstacle) return;

        // Have atleast one already
        if (m_selected_tiles.Count > 0)
        {
            // Check if new selected tile is a neighbour
            var neighbours = m_selected_tiles[0].getNeighbours();
            foreach(var node in neighbours)
            {
                if (node == selectedTile)
                {
                    m_selected_tiles.Add(selectedTile);
                    break;
                }
            }
        } else
        {
            m_selected_tiles.Add(selectedTile);
        }

        if (m_selected_tiles.Count < 2) return;

        Debug.Log($"({ m_selected_tiles[0].x }, {m_selected_tiles[0].y} ) and ({ m_selected_tiles[1].x }, {m_selected_tiles[1].y} )");
        m_selected_initial_transforms.Add(m_selected_tiles[0].Icon.transform.position);
        m_selected_initial_transforms.Add(m_selected_tiles[1].Icon.transform.position);

        swapCounter = 0;
        swapSelectedTiles();
    }

    public void swapSelectedTiles()
    {
        swapTime = 0;
        swapCounter++;
        InvokeRepeating("animateSwap", 0, 0.01f);
    }

    private void animateSwap()
    {
        selectionDisabled = true;
        swapTime += Time.deltaTime / MAX_SWAP_TIME;
        m_selected_tiles[0].Icon.transform.position = 
            Vector3.Lerp(m_selected_initial_transforms[0],
            m_selected_initial_transforms[1], swapTime);
        m_selected_tiles[1].Icon.transform.position = 
            Vector3.Lerp(m_selected_initial_transforms[1], 
            m_selected_initial_transforms[0], swapTime);

        if (swapTime >= 1)
        {
            CancelInvoke("animateSwap");
            Debug.Log("Done with swap!");

            // Swap parents
            m_selected_tiles[0].Icon.transform.SetParent(m_selected_tiles[1].transform);
            m_selected_tiles[1].Icon.transform.SetParent(m_selected_tiles[0].transform);

            // Swap icons
            var tempIcon = m_selected_tiles[0].Icon;
            m_selected_tiles[0].Icon = m_selected_tiles[1].Icon;
            m_selected_tiles[1].Icon = tempIcon;

            // Swap items
            var tempItem = m_selected_tiles[0].Item;
            m_selected_tiles[0].Item = m_selected_tiles[1].Item;
            m_selected_tiles[1].Item = tempItem;

            // Swap again if no matches
            if (swapCounter < 2 && !AnyMatches())
            {
                swapSelectedTiles();
                return;
            }

            StartCoroutine(MatchAndReplace());
            m_selected_tiles.Clear();
            m_selected_initial_transforms.Clear();
        }
    }

    private bool AnyMatches()
    {
        for (int j = 0; j < getBoardHeight(); j++)
        {
            for (int i = 0; i < getBoardLength(); i++)
            {
                if (Tiles[i, j].GetConnected().Count >= match_count) return true;
            }
        }
        return false;
    }

    private IEnumerator MatchAndReplace()
    {
        for (int j = 0; j < getBoardHeight(); j++)
        {
            for (int i = 0; i < getBoardLength(); i++)
            {
                var connected = Tiles[i, j].GetConnected();

                if (connected.Count < match_count) continue;

                int newScore = 0;
                foreach(var node in connected)
                {
                    node.AnimateDeflate();
                    newScore += node.Item.scoreValue;
                }
                m_game_manager.Score += newScore;

                // Play the sound
                m_match_sound.Play();

                // Wait for deflation
                yield return new WaitForSeconds(0.5f);

                // After deflation randomize and inflate icons
                foreach (var node in connected)
                {
                    node.SetItem(ItemGenerator.GenerateForMatchCount(match_count));
                    node.AnimateInflate();
                }

                // Wait for inflation
                yield return new WaitForSeconds(0.5f);

                // Reset the loop
                i = 0;
                j = 0;
            }
        }

        // Re-enable tile selection
        selectionDisabled = false;
    }

    public void Paused()
    {
        game_paused = true;
    }
}
