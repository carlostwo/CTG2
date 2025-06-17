using CTG2;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CTG2.Content;
using ClassesNamespace;
using CTG2.Content.ClientSide;


public class ClassCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "class";
    public override string Description => "Select a player class";

    public override void Action(CommandCaller caller, string input, string[] args)
    {   
        var thisPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
        
        if (GameInfo.matchStage != 1) //!CTG2.Content.Game.preparationPhase
        {
            caller.Reply("You can only select a class during class selection!", Color.Red);
            return;
        }

        if (args.Length < 1 || !int.TryParse(args[0], out int classType))
        {
            caller.Reply("Usage: /class [number]", Color.Red);
            return;
        }

        Player player = caller.Player;
        var modPlayer = player.GetModPlayer<ClassSystem>();
        
        switch (classType)
        {
            case 1:
                modPlayer.playerClass = 1;
                caller.Reply("You selected Archer.", Color.Green);
                break;

            case 2:
                modPlayer.playerClass = 2;
                caller.Reply("You selected Ninja.", Color.Green);
                break;

            case 3:
                modPlayer.playerClass = 3;
                caller.Reply("You selected Beast.", Color.Green);
                break;

            case 4:
                modPlayer.playerClass = 4;
                caller.Reply("You selected Gladiator.", Color.Green);
                break;

            case 5:
                modPlayer.playerClass = 5;
                caller.Reply("You selected Paladin.", Color.Green);
                break;

            case 6:
                modPlayer.playerClass = 6;
                caller.Reply("You selected Jungle Man.", Color.Green);
                break;

            case 7:
                modPlayer.playerClass = 7;
                caller.Reply("You selected Black Mage.", Color.Green);
                break;

            case 8:
                modPlayer.playerClass = 8;
                caller.Reply("You selected Psychic.", Color.Green);
                break;

            case 9:
                modPlayer.playerClass = 9;
                caller.Reply("You selected White Mage.", Color.Green);
                break;

            case 10:
                modPlayer.playerClass = 10;
                caller.Reply("You selected Miner.", Color.Green);
                break;

            case 11:
                modPlayer.playerClass = 11;
                caller.Reply("You selected Fish.", Color.Green);
                break;

            case 12:
                modPlayer.playerClass = 12;
                caller.Reply("You selected Clown.", Color.Green);
                break;

            case 13:
                modPlayer.playerClass = 13;
                caller.Reply("You selected Flame Bunny.", Color.Green);
                break;

            case 14:
                modPlayer.playerClass = 14;
                caller.Reply("You selected Tiki Priest.", Color.Green);
                break;

            case 15:
                modPlayer.playerClass = 15;
                caller.Reply("You selected Tree.", Color.Green);
                break;

            case 16:
                modPlayer.playerClass = 16;
                caller.Reply("You selected Mutant.", Color.Green);
                break;

            case 17:
                modPlayer.playerClass = 17;
                caller.Reply("You selected Leech.", Color.Green);
                break;


            default:
                caller.Reply("Invalid class.", Color.Red);
                break;
        }
    }
}
