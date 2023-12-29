using System;

public enum GameState
{
    LOBBY,
    IN_GAME,
    END_GAME,
}

/// <remarks>
/// Nếu cần thiết thì có thể thêm các bộ level khác vào đây. Nhớ sửa lại trong <see cref="LevelConstraint"/> tương ứng.
/// </remarks>
public enum LevelType
{
    Normal = 0,
}

public enum EnemyType
{
    Normal = 0,
    Boss = 1
}

public enum EnemyBossType
{
    MiniBoss = 0,
    Boss = 1
}

public enum IdPack
{
    NONE = -1,
    ONE = 0,
    HEAP = 1,
    PACK = 2,
    CLOTH_SACK = 3,
    IRON_CASE = 4,
    GOLDEN = 5,
    NO_ADS_PREMIUM = 6,
    NO_ADS_BASIC = 7
}

public enum LevelResult
{
    NotDecided,
    Win,
    Lose
}

public enum TypeHat
{
    EGG = 0,
    KUNG_FU = 1,
    DESTAP = 2,
    CORONA = 3,
    FLOWER = 4,
    BOX_HAT = 5,
    PULPO = 6,
    HORN = 7,
    AUDIFON = 8,
    Amoung_Us_Baby_2 = 9,
    Baloon_Hat = 10,
    Basic_Hat = 11,
    Gorrito_Hat = 12,
    Horn = 13,
    Kung_Fu_Hat = 14,
    Leaves_Hat = 15,
    Party_Hat = 16,
    Toilete_Paper_Hat = 17

}

public enum TypePet
{
    PET_1 = 0,
    PET_2 = 1,
    PET_3 = 2,
    PET_4 = 3,
    PET_5 = 4,
    PET_6 = 5,
}


[Flags]
public enum TypeUnlockSkin
{
    NONE = 0,
    SPIN = 1 << 1,
    COIN = 1 << 2,
    VIDEO = 1 << 3
}

public enum TypeDialogReward
{
    LUCKY_WHEEL,
    END_GAME
}

public enum Skin
{
    SKIN_ROB = 0,
    SKIN_NOOB = 1,
    SKIN_NINJA = 2,
    SKIN_SPIDA = 3,
    SKIN_BADMON = 4,
    SKIN_CAPTAIN = 5,
    SKIN_IRON = 6,
    SKIN_DEAD_PULL = 7,
    SKIN_GOKU = 8,
    SKIN_JASON = 9,
    SKIN_SLENDERMAN = 10,
    SKIN_VENOM = 11,
    SKIN_MARIO = 12,
    SKIN_MABU = 13,
    SKIN_BIG6 = 14,
}

public enum TypeGift
{
    GOLD,
    MINI_CEW,
    X5_GOLD,
    REMOVE_ADS,
    HAT,
    SKIN,
    PET
}

public enum TypeItem
{
    Coin = 0,
    Key = 1

}

public enum TypeSoundIngame
{
    NONE = 0,
    PICK_COIN = 1,
    COLLECT_SPEED = 2
}

public enum Role
{
    CatMan,
    Sword,
    Assassin,
    Gunner,
    Hunter,
    Captain,
    Trapper
}

public enum Skill
{
    JUMP_AND_EAT = 0,
    KATANA = 1,
    KNIFE = 2,
    GUN = 3,
    BAZOOKA = 4,
    HAMMER = 5,
    TRAP = 6
}

public enum TypeColorMarble
{
    Blue = 0,
    Green = 1,
    Pink = 2,
    Purple = 3,
    Red = 4,
    White = 5,
    Yellow = 6
}