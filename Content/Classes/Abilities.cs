using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures; 
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.CompilerServices;
using ClassesNamespace;


namespace CTG2.Content
{
    public class Abilities : ModPlayer
    {
        private int class4BuffTimer = 0;
        private bool class4PendingBuffs = false;

        private int class6ReleaseTimer = -1;

        private int class8HP = 0;
        private PlayerDeathReason reason = PlayerDeathReason.ByCustomReason("");
        private string path = "";
        private string inventoryData = "";
        private List<ItemData> class16RushData;
        private List<ItemData> class16RegenData;
        private bool initializedMutant;
        private int mutantState = 1;


        private string GetPathRelativeToSource(string fileName, [CallerFilePath] string sourceFilePath = "")
        {
            string folder = Path.GetDirectoryName(sourceFilePath);
            return Path.Combine(folder, fileName);
        }


        private void SetInventory(List<ItemData> classData)
        {
            for (int b = 0; b < Player.inventory.Length; b++)
            {
                var itemData = classData[b];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.inventory[b] = newItem;
            }

            for (int d = 0; d < Player.armor.Length; d++)
            {
                var itemData = classData[Player.inventory.Length + d];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.armor[d] = newItem;
            }
        }


        private void SetCooldown(int seconds)
        {
            Player.AddBuff(BuffID.ChaosState, seconds * 60);
        }


        private void ArcherOnUse()
        {
            Player.AddBuff(137, 5 * 60);
        }


        private void ArcherPostStatus() // not finished
        {

        }


        private void NinjaOnUse()
        {
            Player.AddBuff(BuffID.Invisibility, 60 * 60);
        }


        private void BeastOnUse()
        {
            Player.AddBuff(BuffID.MagicPower, 600);

            int npcIndex = NPC.NewNPC(Player.GetSource_Misc("Class3Ability"), (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<StationaryBeast>());
            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
        }


        private void GladiatorOnUse()
        {
            Player.AddBuff(206, 300);
            Player.AddBuff(195, 300);
            Player.AddBuff(75, 300);
            Player.AddBuff(320, 300);

            class4BuffTimer = 300;
            class4PendingBuffs = true;
        }


        private void GladiatorPostStatus()
        {
            if (class4PendingBuffs) // runs 5 second interval
            {
                class4BuffTimer--;

                if (class4BuffTimer <= 0)
                {

                    Player.AddBuff(137, 180);
                    Player.AddBuff(32, 180);
                    Player.AddBuff(195, 180);
                    Player.AddBuff(5, 180);
                    Player.AddBuff(215, 180);

                    class4PendingBuffs = false;
                }
            }
        }


        private void PaladinOnUse()
        {
            foreach (Player other in Main.player)
            {
                if (!other.active || other.dead || other.whoAmI == Player.whoAmI)
                    continue;

                if (Vector2.Distance(Player.Center, other.Center) <= 20 * 16 && Player.team == other.team) // 20 block radius
                {
                    other.AddBuff(58, 200);
                    other.AddBuff(119, 200);
                    other.AddBuff(2, 200);
                    
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 58, 200);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 119, 200);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 2, 200);
                }
            }

            Player.AddBuff(58, 100);
            Player.AddBuff(119, 100);
            Player.AddBuff(2, 100);
        }


        private void JungleManOnUse()
        {
            Player.AddBuff(149, 60);
            Player.AddBuff(114, 60);

            class6ReleaseTimer = 60;
        }


        private void JungleManPostStatus()
        {
            class6ReleaseTimer = (class6ReleaseTimer > -1) ? class6ReleaseTimer - 1 : -1;
            if (class6ReleaseTimer == 0)
            {

                if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 spawnPos = Player.Center + new Vector2(0, Player.height / 2);

                    for (int i = 0; i < 25; i++)
                    {
                        // Horizontal speed, Vertical
                        float speed = Main.rand.NextFloat(0f, 5f);

                        // might be too fast currently
                        float direction = Main.rand.NextBool() ? 0f : 180f;
                        Vector2 velocity = direction.ToRotationVector2() * speed;

                        //horizontal offset
                        float xOffset = Main.rand.NextFloat(-32f, 32f); // 1 tile = 16px 
                        float yOffset = Main.rand.NextFloat(-32f, 10f);

                        Vector2 spawnPoss = Player.Center + new Vector2(xOffset, Player.height / 2f + yOffset);

                        if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(
                                Player.GetSource_Misc("Class6GroundFlames"),
                                spawnPoss,
                                velocity,
                                480, // cursed flame
                                26,
                                1f,
                                Player.whoAmI
                            );
                        }
                    }
                }
            }
        }


        private void BlackMageOnUse()
        {
            Player.AddBuff(176, 15);
            Player.AddBuff(26, 420);
            Player.AddBuff(137, 420);
            Player.AddBuff(320, 420);
        }


        private void PsychicOnUse()
        {
            Player.AddBuff(196, 54000);
            Player.AddBuff(178, 54000);
            Player.AddBuff(181, 54000);
            
            class8HP = Player.statLife;
        }


        private void PsychicPostStatus()
        {
            if (class8HP != 0)
            {
                if (Player.HeldItem.type == ItemID.NebulaArcanum && Player.controlUseItem && Player.itemAnimation == 30) class8HP = (class8HP <= 20) ? 0 : class8HP - 20;

                if (Player.statLife < class8HP) class8HP = Player.statLife;
                Player.statLife = class8HP;
            }

            if (Player.statLife <= 0) Player.KillMe(reason, 1, 0);
        }


        private void WhiteMageOnUse()
        {
            foreach (Player other in Main.player)
            {
                if (!other.active || other.dead || other.whoAmI == Player.whoAmI)
                    continue;

                if (Vector2.Distance(Player.Center, other.Center) <= 25 * 16 && Player.team == other.team) // 25 block radius
                {
                    other.AddBuff(103, 480);
                    other.AddBuff(26, 480);
                    other.AddBuff(2, 480);
                    
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 2, 480);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 26, 480);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 103, 480);
                }
            }
        }


        private void MinerOnUse() //not finished
        {

        }


        private void FishOnUse()
        {
            Player.AddBuff(1, 180);
            Player.AddBuff(104, 180);
            Player.AddBuff(109, 180);
        }


        private void ClownOnUse() //not finished
        {

        }


        private void FlameBunnyOnUse() //not finished
        {

        }


        private void TikiPriestOnUse() //not finished
        {
            int npcIndex = NPC.NewNPC(Player.GetSource_Misc("Class14Ability"), (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<TikiTotem>());
            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
        }


        private void TreeOnUse() //not finished
        {

        }

        
        private void MutantInitialize()
        {
            path = GetPathRelativeToSource("rushmutant.json");
            inventoryData = File.ReadAllText(path);
            try
            {
                class16RushData = JsonSerializer.Deserialize<List<ItemData>>(inventoryData);
            }
            catch
            {
                Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                return;
            }

            path = GetPathRelativeToSource("regenmutant.json");
            inventoryData = File.ReadAllText(path);
            try
            {
                class16RegenData = JsonSerializer.Deserialize<List<ItemData>>(inventoryData);
            }
            catch
            {
                Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                return;
            }
        }


        private void MutantOnUse()
        {
            Player.AddBuff(149, 90);

            switch (mutantState)
            {
                case 1:
                    SetInventory(class16RegenData);
                    mutantState = 2;

                    break;

                case 2:
                    SetInventory(class16RushData);
                    mutantState = 1;

                    break;
            }
        }


        private void LeechOnUse() //not finished
        {

        }


        public override void PostItemCheck() // Upon activation
        {
            if (!initializedMutant)
            {
                MutantInitialize();
                initializedMutant = true;
            }

            if (Player.HeldItem.type == ItemID.WhoopieCushion && Player.controlUseItem && Player.itemTime == 0 && !Player.HasBuff(BuffID.ChaosState)) // Only activate if not on cooldown
            {
                int selectedClass = Player.GetModPlayer<ClassSystem>().playerClass;

                switch (selectedClass)
                {
                    case 1:
                        //SetCooldown(36);
                        ArcherOnUse();

                        break;

                    case 2:
                        SetCooldown(10);
                        NinjaOnUse();
                        
                        break;

                    case 3:
                        SetCooldown(35);
                        BeastOnUse();

                        break;

                    case 4:
                        SetCooldown(35);
                        GladiatorOnUse();

                        break;

                    case 5:
                        SetCooldown(10);
                        PaladinOnUse();

                        break;

                    case 6:
                        SetCooldown(31);
                        JungleManOnUse();

                        break;

                    case 7:
                        SetCooldown(42);
                        BlackMageOnUse();

                        break;

                    case 8:
                        SetCooldown(40);
                        PsychicOnUse();

                        break;

                    case 9:
                        SetCooldown(30);
                        WhiteMageOnUse();

                        break;

                    case 10: //not finished
                        //SetCooldown(15);
                        MinerOnUse();

                        break;

                    case 11:
                        SetCooldown(35);
                        FishOnUse();

                        break;

                    case 12: //not finished
                        //SetCooldown(11);
                        ClownOnUse();

                        break;

                    case 13: //not finished
                        //SetCooldown(41);
                        FlameBunnyOnUse();

                        break;

                    case 14: //not finished
                        //SetCooldown(20);
                        TikiPriestOnUse();

                        break;

                    case 15: //not finished 
                        //SetCooldown(27);
                        TreeOnUse();

                        break;

                    case 16: //not finished
                        SetCooldown(1);
                        MutantOnUse();

                        break;

                    case 17: //not finished
                        //SetCooldown(40);
                        LeechOnUse();

                        break;
                }
            }
        }

        //All timer logic below
        public override void PostUpdate()
        {
            ArcherPostStatus(); 
            GladiatorPostStatus();
            JungleManPostStatus();
            PsychicPostStatus();
        }
    }
}
