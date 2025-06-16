using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity { Normal, Magic, Rare, Unique, Set }
public enum ItemType
{
    Sword, Axe, Bow, Crossbow, Staff, Wand,
    Helmet, Armor, Belt, Boots, Gloves, Cloak,
    Necklace, Ring1, Ring2
}
public enum CharacterClass { Warrior, Archer, Wizard, All }

public enum ItemOptionType
{
    공격력, 방어력, 이동속도, 공격속도, 치명타확률, 치명타피해량, 
    행운, 최대HP량증가, 최대MP량증가, 초당HP회복량증가, 초당MP회복량증가, 초당HP회복속도증가, 초당MP회복속도증가,
    힘, 민첩, 지능, 투사체수증가, 투사체속도증가, 특수공격력증가, 공격범위증가
}