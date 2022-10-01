using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util.Inventory;
using UnityEngine;

public static class ItemRegister
{
    [RuntimeInitializeOnLoadMethodAttribute]
    private static void RegistItems()
    {
        RegisterClass(ItemType.Wood, null, "나무", "생각보다 무거운 나무 조각이다.");
        RegisterClass(ItemType.Stone, null, "돌", "가볍고 단단한 돌 조각이다.");
        RegisterClass(ItemType.Iron, null, "철", "매우 단단한 철 조각이다.");
        RegisterClass(ItemType.Gold, null, "금", "희귀하고 비싸보이는 금 조각이다.");
        RegisterClass(ItemType.Diamond, null, "다이아몬드", "매우 희귀하고 비싸보이는 다이아몬드 조각이다.");

        RegisterFoodClass(ItemType.Apple, null, "사과", "맛있는 사과다.", 4);
        RegisterFoodClass(ItemType.Bread, null, "빵", "맛있는 빵이다.", 5);
        RegisterFoodClass(ItemType.Steak, null, "스테이크", "맛있는 스테이크다.", 8);
        RegisterFoodClass(ItemType.Fish, null, "생선", "맛있는 생선이다.", 6);
        RegisterFoodClass(ItemType.Chicken, null, "닭", "맛있는 닭이다.", 7);
        RegisterFoodClass(ItemType.Pork, null, "돼지고기", "맛있는 돼지고기다.", 9);
    }

    private static void RegisterClass(ItemType type, Sprite icon, string name, string description, int maxStack, ItemBase<ItemType> itemClass)
    {
        ItemDatabase<ItemType>.RegisterItemClass(type, icon, name, description, maxStack, itemClass);
    }

    private static void RegisterClass(ItemType type, Sprite icon, string name, string description)
    {
        ItemDatabase<ItemType>.RegisterItemClass(type, icon, name, description);
    }

    private static void RegisterFoodClass(ItemType type, Sprite icon, string name, string description, float saturation)
    {
        ItemDatabase<ItemType>.RegisterItemClass(type, icon, name, description, 99, new FoodItem(type, saturation));
    }
}
