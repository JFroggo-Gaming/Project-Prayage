public enum AnimationName
{
    IdleDown,
    IdleUp,
    IdleRight,
    IdleLeft,
    walkUp,
    walkDown,
    walkRight,
    walkLeft,
    runUp,
    runDown,
    runLeft,
    useToolUp,
    useToolDown,
    useToolRight,
    useToolLeft,
    swingToolUp,
    swingToolDown,
    swingToolRight,
    swingToolLeft,
    liftToolUp,
    liftToolDown,
    liftToolRight,
    liftToolLeft,
    holdToolUp,
    holdToolDown,
    holdToolRight,
    holdToolLeft,
    pickDown,
    pickUp,
    pickRight,
    pickLeft,
    count
}

public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}
public enum PartVariantColour
{
    none,
    count
}

public enum PartVariantType
{
    none,
    carry,
    hoe,
    pickaxe,
    axe,
    scythe,
    wateringCan,
    count
}
public enum InventoryLocation
{
    player,
    chest,
    count
}


public enum ToolEffect // defines the number of specific values
{
    none,
    watering
}

public enum Direction // player movement direction
{
    up,
    down,
    left,
    right,
    none
}

public enum ItemType
{   
    // Intencje
    IntencjaZlota,
    IntencjaPoswiecenia,
    IntencjaPlodnosci,
    IntencjaWiary,
    IntencjaWiedzy,
    IntencjaPomyslnosci,
    IntencjaBlogoslawienstwa,
    IntencjaPrzyjaciela,
    IntencjaRelikwiarzu,
    // Modlitwy
    ZarliwejModlitwy,
    MajestatycznejModlitwy,
    CholerycznejModlitwy,
    BiernejModlitwy,
    none,
    count
}