using UnityEngine;

public static class Settings
{   
    // Obscuring Item Fading - ObscuringItemFader(this is to make items transparent when we walk behind them)
    public const float fadeInSeconds = 0.25f;
    public const float fadeOutSeconds = 0.35f;
    public const float targetAlpha = 0.45f;   // how transparent the item is going to be once it's faded

    // Player movement

    public const float runningspeed = 5.333f;
    public const float walkingspeed = 3.666f;

    // Inventory
    public static int playerInitialInventoryCapacity = 24;
    public static int playerMaximumInventoryCapacity = 48;
    
    // Player Animation Parameters
    public static int xInput;
    public static int yInput;
    public static int isWalking;
    public static int isRunning;
    public static int isCarrying;
    public static int toolEffect;
    public static int isUsingToolRight;
    public static int isUsingToolLeft;
    public static int isUsingToolUp;
    public static int isUsingToolDown;
    public static int isLiftingToolRight;
    public static int isLiftingToolLeft;
    public static int isLiftingToolUp;
    public static int isLiftingToolDown;
    public static int isPickingRight;
    public static int isPickingLeft;
    public static int isPickingUp;
    public static int isPickingDown;
    public static int isSwingingToolRight;
    public static int isSwingingToolLeft;
    public static int isSwingingToolUp;
    public static int isSwingingToolDown;

    // Shared Animation Parameters
    //isIdle
    public static int idleUp;
    public static int idleDown;
    public static int idleLeft;
    public static int idleRight;

    // Modlitwy & Intencje
    // Intencje
    public const string IntencjaZlota = "Intencja Zlota";
    public const string IntencjaPoswiecenia = "Intencja Poswiecenia";
    public const string IntencjaPlodnosci = "Intencja Plodnosci";
    public const string IntencjaWiary = "Intencja Wiary";
    public const string IntencjaWiedzy = "Intencja Wiedzy";
    public const string IntencjaPomyslnosci = "Intencja Pomyslnosci";
    public const string IntencjaBlogoslawienstwa = "Intencja Blogoslawienstwa";
    public const string IntencjaPrzyjaciela = "Intencja Przyjaciela";
    public const string IntencjaRelikwiarzu = "Intencja Relikwiarzu";
    // Modlitwy
    public const string ZarliwejModlitwy = "Zarliwa Modlitwa";
    public const string MajestatycznejModlitwy = "Majestatyczna Modlitwa";
    public const string CholerycznejModlitwy = "Choleryczna Modlitwa";
    public const string BiernejModlitwy = "Bierna Modlitwa";


    //static conctructor
    static Settings()
    {
        

        // Player Animation Parameters
        xInput = Animator.StringToHash("xInput");
        yInput = Animator.StringToHash("yInput");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        //isIdle,
        isCarrying = Animator.StringToHash("isCarrying");
        toolEffect = Animator.StringToHash("toolEffect");
        isUsingToolRight = Animator.StringToHash("isUsingToolRight");
        isUsingToolLeft = Animator.StringToHash("isUsingToolLeft");
        isUsingToolUp = Animator.StringToHash("isUsingToolUp");
        isUsingToolDown = Animator.StringToHash("isUsingToolDown");
        isLiftingToolRight = Animator.StringToHash("isLiftingToolRight");
        isLiftingToolLeft = Animator.StringToHash("isLiftingToolLeft");
        isLiftingToolUp = Animator.StringToHash("isLiftingToolUp");
        isLiftingToolDown = Animator.StringToHash("isLiftingToolDown");
        isPickingRight = Animator.StringToHash("isPickingRight");
        isPickingLeft = Animator.StringToHash("isPickingLeft");
        isPickingUp = Animator.StringToHash("isPickingUp");
        isPickingDown = Animator.StringToHash("isPickingDown");
        isSwingingToolRight = Animator.StringToHash("isSwingingToolRight");
        isSwingingToolLeft = Animator.StringToHash("isSwingingToolLeft");
        isSwingingToolUp = Animator.StringToHash("isSwingingToolUp");
        isSwingingToolDown = Animator.StringToHash("isSwingingToolDown");

        // Shared Animation parameters

        idleUp = Animator.StringToHash("idleUp");
        idleDown = Animator.StringToHash("idleDown");
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");

    }
}
