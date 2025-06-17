using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.Items;
using CTG2.Content.Items.ModifiedWeps;
using Microsoft.Xna.Framework; 
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.CompilerServices;


namespace ClassesNamespace
{
    public class ItemData
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Stack { get; set; }
        public int Prefix { get; set; }
        public int Slot { get; set; }
    }


    public class ClassSystem : ModPlayer
    {
        public int playerClass = 0;
        private int lastPlayerClass = 0;
        private int currentHP = 100;
        string path = "";
        string inventoryData = "";
        List<ItemData> classData;

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


        private string GetPathRelativeToSource(string fileName, [CallerFilePath] string sourceFilePath = "")
        {
            string folder = Path.GetDirectoryName(sourceFilePath);
            return Path.Combine(folder, fileName);
        }


        private void SpawnCustomItem(int itemID, int? prefix = null, int? damage = null, int? useTime = null, int? useAnimation = null, float? scale = null, float? knockBack = null, int? shoot = null, float? shootSpeed = null, Color? colorOverride = null)
        {
            Item item = new Item();
            item.SetDefaults(itemID);

            if (prefix.HasValue) item.Prefix(prefix.Value);
            if (damage.HasValue) item.damage = damage.Value;
            if (useTime.HasValue) item.useTime = useTime.Value;
            if (useAnimation.HasValue) item.useAnimation = useAnimation.Value;
            if (scale.HasValue) item.scale = scale.Value;
            if (knockBack.HasValue) item.knockBack = knockBack.Value;
            if (shoot.HasValue) item.shoot = shoot.Value;
            if (shootSpeed.HasValue) item.shootSpeed = shootSpeed.Value;
            if (colorOverride.HasValue) item.color = colorOverride.Value;

            Player.QuickSpawnItem(null, item, 1);
        }


        public override void ResetEffects()
        {

            Player.AddBuff(BuffID.Shine, 54000);
            Player.AddBuff(BuffID.NightOwl, 54000);
            Player.AddBuff(BuffID.Builder, 54000);


            if (playerClass != lastPlayerClass)
            {   
                
                // TODO: combine classes into one json (dictionary with key=className, value=[ClassHp, InventoryData])
                // remove switch, use classData = deserialisedJson["className"][1], currentHP = deserialisedJson["className"][0]
                switch (playerClass)
                {
                    case 1: // Archer
                        currentHP = 160;

                        path = GetPathRelativeToSource("archer.json");

                        break;

                    case 2: // Ninja
                        currentHP = 160;

                        path = GetPathRelativeToSource("ninja.json");

                        break;

                    case 3: // Beast
                        currentHP = 160;

                        path = GetPathRelativeToSource("beast.json");

                        break;

                    case 4: // Gladiator
                        currentHP = 140;

                        path = GetPathRelativeToSource("gladiator.json");

                        break;

                    case 5: // Paladin
                        currentHP = 160;

                        path = GetPathRelativeToSource("paladin.json");

                        break;

                    case 6: // Jungle Man
                        currentHP = 140;

                        path = GetPathRelativeToSource("jungleman.json");

                        break;

                    case 7: // Black Mage
                        currentHP = 160;

                        path = GetPathRelativeToSource("blackmage.json");

                        break;

                    case 8: // Psychic
                        currentHP = 160;

                        path = GetPathRelativeToSource("psychic.json");

                        break;

                    case 9: // White Mage
                        currentHP = 160;

                        path = GetPathRelativeToSource("whitemage.json");

                        break;

                    case 10: // Miner
                        currentHP = 180;

                        path = GetPathRelativeToSource("miner.json");

                        break;

                    case 11: // Fish
                        currentHP = 150;

                        path = GetPathRelativeToSource("fish.json");

                        break;

                    case 12: // Clown
                        currentHP = 120;

                        path = GetPathRelativeToSource("clown.json");

                        break;

                    case 13: // Flame Bunny
                        currentHP = 160;

                        path = GetPathRelativeToSource("flamebunny.json");

                        break;

                    case 14: // Tiki Priest
                        currentHP = 160;

                        path = GetPathRelativeToSource("tikipriest.json");

                        break;

                    case 15: // Tree
                        currentHP = 160;

                        path = GetPathRelativeToSource("tree.json");

                        break;

                    case 16: // Mutant
                        currentHP = 100;

                        path = GetPathRelativeToSource("rushmutant.json");

                        break;

                    case 17: // Leech
                        currentHP = 140;

                        path = GetPathRelativeToSource("leech.json");

                        break;

                    default:
                        path = GetPathRelativeToSource("archer.json");

                        break;
                }

                inventoryData = File.ReadAllText(path);

                try
                {
                    classData = JsonSerializer.Deserialize<List<ItemData>>(inventoryData);
                }
                catch
                {
                    Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                    return;
                }

                SetInventory(classData);
            }

            Player.statLifeMax2 = currentHP;
            if (playerClass != lastPlayerClass) Player.statLife = currentHP;

            lastPlayerClass = playerClass;
        }
    }
}
