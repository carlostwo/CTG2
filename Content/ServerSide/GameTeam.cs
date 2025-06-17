using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CTG2.Content;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CTG2.Content.ServerSide;

public class GameTeam
{   
    public List<Player> Players { get; set; }
    private Vector2 ClassLocation { get; set; }
    private Vector2 BaseLocation { get; set; }
    
    private int TeamColor { get; set; }
    
    public GameTeam(Vector2 classLocation, Vector2 baseLocation, int teamColor)
    {
        ClassLocation = classLocation;
        BaseLocation = baseLocation;
        TeamColor = teamColor;
        Players = new List<Player>();
    }

    public void UpdateTeam()
    {   
        Players.Clear();
        foreach (Player ply in Main.player)
        {   
            if (ply.team == TeamColor) Players.Add(ply);
        }
    }
    
    public void StartMatch()
    {   
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerGameStart);
        packet.Send();
        
        foreach (Player ply in Players)
        {   
            ply.Teleport(ClassLocation);
            ply.position = ClassLocation;
            
            int tpX = (int)ClassLocation.X;
            int tpY = (int)ClassLocation.Y;
            
            ModPacket packet2 = mod.GetPacket();
            packet2.Write((byte)MessageType.ServerTeleport);
            packet2.Write(tpX);
            packet2.Write(tpY);
            packet2.Send(toClient: ply.whoAmI);
            Console.WriteLine($"teleported {ply.name} to {ply.position}!");
        }
    }

    public void SendToBase()
    {   
        var mod = ModContent.GetInstance<CTG2>();
        foreach (Player ply in Players)
        {

            ply.Teleport(BaseLocation);
            int tpX = (int)BaseLocation.X;
            int tpY = (int)BaseLocation.Y;
            ModPacket packet2 = mod.GetPacket();
            packet2.Write((byte)MessageType.ServerTeleport);
            packet2.Write(tpX);
            packet2.Write(tpY);
            packet2.Send(toClient: ply.whoAmI);

            int spawnPosX = tpX / 16;
            int spawnPosY = tpY / 16;

            ply.SpawnX = spawnPosX;
            ply.SpawnY = spawnPosY;
            
            ModPacket packet3 = mod.GetPacket();
            packet3.Write((byte)MessageType.ServerSetSpawn);
            packet3.Write(spawnPosX);
            packet3.Write(spawnPosY);
            packet3.Send(toClient: ply.whoAmI);
        }
        
    }

    public void PauseTeam()
    {
        foreach (Player ply in Players)
        {
            int buffTicks = 15 * 60;
            ply.AddBuff(BuffID.Webbed, buffTicks);
            NetMessage.SendData(MessageID.AddPlayerBuff, -1, -1, null, ply.whoAmI, BuffID.Webbed, buffTicks);
        }
    }
    
}