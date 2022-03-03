using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagement : MonoBehaviour
{
    [SerializeField] private Transform itemParent = null;

    private List<ItemController> itemControllers = new List<ItemController>();
    private Player player = null;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void SetupItemControllers(ItemProperties[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            ItemController item = Instantiate(items[i].itemPrefab, itemParent).GetComponent<ItemController>();
            item.transform.localPosition = item.startingPosition;
            item.transform.localRotation = Quaternion.Euler(item.startingRotation);
            item.baseItem = items[i];
            item.SetupController(player);
            itemControllers.Add(item);
        }
    }

    public void SetupNewItem(ItemProperties item)
    {

    }

    public void RemoveItem(ItemProperties item)
    {

    }

    private void Start()
    {
        player.playerInput.Player.Flashlight.performed += ctx => ToggleItem(player.inventory.FindItem(ItemProperties.WeaponTypes.Flashlight));
    }


    public void ToggleItem(Item item)
    {
        if (item == null)
            return;

        ItemController controller = FindItemController(item);

        if (controller)
            controller.UseItem();
    }


    public ItemController FindItemController(Item item)
    {
        for(int i = 0; i < itemControllers.Count; i++)
        {
            if (item.item == itemControllers[i].baseItem)
                return itemControllers[i];
        }

        return null;
    }

    public ItemController FindItemController(ItemProperties item)
    {
        for (int i = 0; i < itemControllers.Count; i++)
        {
            if (item == itemControllers[i].baseItem)
                return itemControllers[i];
        }

        return null;
    }
}
