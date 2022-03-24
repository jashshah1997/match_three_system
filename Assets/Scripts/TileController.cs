using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    public int x;
    public int y;

    private IconController m_icon_controller;
    private Button m_button;
    private ItemInterface m_item;

    // Start is called before the first frame update
    void Start()
    {
        m_icon_controller = GetComponentInChildren<IconController>();
        m_button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(ItemInterface newItem)
    {
        if (m_item == newItem) return;

        m_item = newItem;
        m_icon_controller.IconImage.sprite = m_item.itemSprite;
    }
}
