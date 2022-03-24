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

    private Button m_button;
    private BoardController m_board;

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
}
