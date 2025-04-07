using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppScheduleOne.PlayerScripts;

public static class ScheduleBoostHelper
{
    public static void GiveItemToPlayer(ItemData item, int amount)
    {
        var instance = new ItemInstance((ItemDefinition)item.ID, amount);
        PlayerInventory.Instance.AddItemToInventory(instance);
    }
}
