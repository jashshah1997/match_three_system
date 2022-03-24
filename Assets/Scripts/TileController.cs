using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public int x;
    public int y;
    public IconController Icon;

    private Button m_button;
    private ItemInterface m_item;
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
        if (m_item == newItem) return;

        m_item = newItem;
        Icon.IconImage.sprite = m_item.itemSprite;
    }
}
