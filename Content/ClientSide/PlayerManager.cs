using System;
using ClassesNamespace;
using Terraria;
using Terraria.ModLoader;

namespace CTG2.Content.ClientSide;

public class PlayerManager : ModPlayer
{
    
    public override void OnRespawn()
    {   
        int blueBaseX = 12346 / 16;
        int blueBaseY = 10940 / 16;
        int redBaseX = 20385 / 16;
        int redBaseY = 10940 / 16;
        switch (GameInfo.matchStage)
        { 
            case 0:
                break;
            case 1:
                if (Player.team == 3)
                {
                    Player.SpawnX = blueBaseX;
                    Player.SpawnY = blueBaseY;
                }
                else
                {
                    Player.SpawnX = redBaseX;
                    Player.SpawnY = redBaseY;
                }
                break;
            case 2:
                if (Player.team == 3)
                {
                    Player.SpawnX = blueBaseX;
                    Player.SpawnY = blueBaseY;
                }
                else
                {
                    Player.SpawnX = redBaseX;
                    Player.SpawnY = redBaseY;
                }
                break;
        }
    }

    public override void PreUpdate()
    {
        if (GameInfo.matchStage == 1)
        {
            if (GameInfo.matchTime == 1795)
            {   
                var player = Main.LocalPlayer.GetModPlayer<ClassSystem>();
                
                if (player.playerClass == 0)
                {
                    Main.NewText($"You did not select a class! Assigning a random class.");
                    player.playerClass = CTG2.randomGenerator.Next(1, 18);
                    player.ResetEffects();
                }
            }
        }

    }

    // When a player disconnects, this hook can clean up their data.
    public override void PlayerDisconnect()
    {

    }
}