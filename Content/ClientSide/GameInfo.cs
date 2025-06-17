using Microsoft.Xna.Framework;
namespace CTG2.Content.ClientSide;

public static class GameInfo
{   
    // 0 = Inactive, 1 = Class Selection, 2 = Game Active
    public static int matchStage = 0;
    public static int matchTime = 0;
    public static int blueGemX = 0;
    public static int redGemX = 0;
    public static string blueGemCarrier = "At Base";
    public static string redGemCarrier = "At Base";
    
}