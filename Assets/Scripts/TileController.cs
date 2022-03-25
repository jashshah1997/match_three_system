using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public int x;
    public int y;
    public IconController Icon;
    public ItemInterface Item;
    public bool IsScaling = false;

    private Button m_button;
    private BoardController m_board;
    private readonly float SCALE_DURATION = 0.5f;
    private float scale_time;

    // Start is called before the first frame update
    void Start()
    {
        Icon = GetComponentInChildren<IconController>();
        m_button = GetComponent<Button>();
        m_board = GameObject.Find("Board").GetComponent<BoardController>();
        m_button.onClick.AddListener(onTileButtonClicked);
    }

    void onTileButtonClicked()
    {
        m_board.setSelected(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(ItemInterface newItem)
    {
        if (Item == newItem) return;

        Item = newItem;
        Icon.IconImage.sprite = Item.itemSprite;
    }

    public TileController LeftTile()
    {
        return x > 0 ? m_board.Tiles[x - 1, y] : null;
    }

    public TileController TopTile()
    {
        return y > 0 ? m_board.Tiles[x, y - 1] : null;
    }


    public TileController RightTile()
    {
        return x < m_board.getBoardLength() - 1 ? m_board.Tiles[x + 1, y] : null;
    }

    public TileController BottomTile()
    {
        return y < m_board.getBoardHeight() - 1 ? m_board.Tiles[x, y + 1] : null;
    }

    public TileController[] getNeighbours()
    {
        return new []{ LeftTile(), TopTile(), RightTile(), BottomTile()};
    }

    public List<TileController> GetConnected(List<TileController> except = null)
    {
        var connected = new List<TileController>();
        connected.Add(this);

        if (except == null)
        {
            except = new List<TileController>(); 
        }
        except.Add(this);

        var neighbours = getNeighbours();
        foreach (var node in neighbours)
        {
            if (node == null || except.Contains(node) || node.Item != Item) continue;
            connected.AddRange(node.GetConnected(except));
        }
        return connected;
    }

    public void AnimateInflate()
    {
        IsScaling = true;
        scale_time = 0;
        InvokeRepeating("inflate", 0, 0.01f);
    }

    public void AnimateDeflate()
    {
        IsScaling = true;
        scale_time = 0;
        InvokeRepeating("deflate", 0, 0.01f);
    }

    private void deflate()
    {
        IsScaling = true;
        scale_time += Time.deltaTime / SCALE_DURATION;
        Icon.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, scale_time);

        if (scale_time >= 1)
        {
            CancelInvoke("deflate");
            IsScaling = false;
        }
    }
    private void inflate()
    {
        IsScaling = true;
        scale_time += Time.deltaTime / SCALE_DURATION;
        Icon.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scale_time);

        if (scale_time >= 1)
        {
            CancelInvoke("inflate");
            IsScaling = false;
        }
    }
}
