using AchromaticDev.Util.Inventory;

public class FoodItem : ItemBase<ItemType>
{
    public float RestoreHealth { get; private set; }

    public FoodItem(ItemType type, float restoreHealth) : base(type)
    {
        RestoreHealth = restoreHealth;
    }
}
