using System;
using System.Collections.Generic;
using System.Linq;
using CTG2.Content.ServerSide;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace CTG2.Content.ClientSide;

public class UIManager : ModSystem
{
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (index != -1)
        {
            layers.Insert(index + 1, new LegacyGameInterfaceLayer(
                "CTG2: Match Timer",
                delegate
                {
                    if (true)
                    {
                        DrawMatchTimer();
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }


    private void DrawMatchTimer()
    {
        string timeText = "";
        Vector2 timeRow = new Vector2(Main.screenWidth - 320, 400);
        Vector2 blueGemRow = new Vector2(Main.screenWidth - 320, 500);
        Vector2 blueGemRow2 = new Vector2(Main.screenWidth - 320, 550);
        Vector2 redGemRow = new Vector2(Main.screenWidth - 320, 600);
        Vector2 redGemRow2 = new Vector2(Main.screenWidth - 320, 650);
        
        Color textColor = Color.White;
        int matchStage = GameInfo.matchStage;
        if (matchStage == 0) return;
        
        int matchTime = GameInfo.matchTime;
        if (matchTime < 1800)
        {
            timeText = $"Class selection ends in: {30-matchTime/60}s";
        }
        else
        {
            int secondsElapsed = matchTime / 60 - 30;
            int secondsLeft = 900 - secondsElapsed;
            int minutesLeft = secondsLeft / 60;
            int remainder = secondsLeft % 60;
            timeText = $"Time left in match: {minutesLeft}:{remainder}";
        }
        var blueGemStatus = GameInfo.blueGemCarrier;
        var redGemStatus = GameInfo.redGemCarrier;
        var blueGemPosition = GameInfo.blueGemX;
        var redGemPosition = GameInfo.redGemX;

        var totalBars = 20;
        var blueBars = (int)Math.Clamp(Math.Round(blueGemPosition / 100f * totalBars), 0, totalBars);
        var redBars = (int)Math.Clamp(Math.Round(redGemPosition / 100f * totalBars), 0, totalBars);
        // Draw Gem Position like in CTG
        string blueGemIndicator  = "[c/0000FF:⬢"+ new string('▮', blueBars) + "]" + "[i:1524]" + "[c/FFFFFF:"+ new string('▮', totalBars-blueBars) + "⬢]";

        string redGemIndicator = "[c/FFFFFF:⬢"+ new string('▮', totalBars-redBars) + "]" + "[i:1526]" + "[c/FF0000:"+ new string('▮', redBars) + "⬢]";
        
        if (!string.IsNullOrEmpty(timeText))
        {
            Utils.DrawBorderString(Main.spriteBatch, timeText, timeRow, textColor);
        }
        if (!string.IsNullOrEmpty(blueGemStatus))
        {
            Utils.DrawBorderString(Main.spriteBatch, blueGemStatus, blueGemRow, Color.Blue);
        }
        if (!string.IsNullOrEmpty(redGemStatus))
        {
            Utils.DrawBorderString(Main.spriteBatch, redGemStatus, redGemRow, Color.Red);
        }
        ChatManager.DrawColorCodedStringWithShadow(
            Main.spriteBatch,
            FontAssets.MouseText.Value,
            blueGemIndicator,
            blueGemRow2,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One
        );
        ChatManager.DrawColorCodedStringWithShadow(
            Main.spriteBatch,
            FontAssets.MouseText.Value,
            redGemIndicator,
            redGemRow2,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One
        );
    }
}