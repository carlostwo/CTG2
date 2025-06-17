using System;
using Terraria.ModLoader;
using System.IO;
using Terraria;
using CTG2.Content;
using CTG2.Content.ClientSide;
using CTG2.Content.ServerSide;
using Microsoft.Xna.Framework;


namespace CTG2
{   
    public enum MessageType : byte
    {
        RequestStartGame = 0,  // client → server
        RequestEndGame   = 1,  // client → server
        ServerGameStart  = 2,  // server → client
        ServerGameEnd    = 3,  // server → client
        RequestPause  = 4,  // client -> server
        ServerGameUpdate  = 5,  // server → client
        ServerTeleport = 6, // server -> client
        ServerSetSpawn = 7 // server -> client
    }
    
    public class CTG2 : Mod
    {   
        public static Random randomGenerator = new Random();
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {   
            // GameManager
            var manager = ModContent.GetInstance<GameManager>();
            
            byte msgType = reader.ReadByte();
            switch (msgType) {
                // Client -> Server Packets (these cases will run on the Server)
                case (byte)MessageType.RequestStartGame:
                    manager.StartGame();
                    Console.WriteLine("Server Received Game Start Request!");
                    break;
                
                case (byte)MessageType.RequestEndGame:
                    manager.EndGame();
                    Console.WriteLine("Server Received Game End Request!");
                    break;
                case (byte)MessageType.RequestPause:
                    manager.PauseGame();
                    break;
                // Server->Client Packets (these cases will run on the Client)
                case (byte)MessageType.ServerGameStart:
                    GameInfo.matchStage = 1;
                    Console.WriteLine("Client Received Game Start!");
                    break;
                case (byte)MessageType.ServerGameEnd:
                    GameInfo.matchStage = 0;
                    Console.WriteLine("Client Received Game End!");
                    break;
                
                case (byte)MessageType.ServerTeleport:
                    var local = Main.player[Main.myPlayer];
                    int tpX = reader.ReadInt32();
                    int tpY = reader.ReadInt32();
                    local.Teleport(new Vector2(tpX, tpY), 1);
                    Console.WriteLine("Client Received Teleport!");
                    break;
                case (byte)MessageType.ServerSetSpawn:
                    var localPlayer = Main.player[Main.myPlayer];
                    int spawnX = reader.ReadInt32();
                    int spawnY = reader.ReadInt32();
                    localPlayer.ChangeSpawn(spawnX, spawnY);
                    Console.WriteLine("Client Received Spawn Update!");
                    break;
                
                // Gems Status Updates
                case (byte)MessageType.ServerGameUpdate:
                    // Populate GameInfo fields
                    GameInfo.matchStage = reader.ReadInt32();
                    GameInfo.matchTime = reader.ReadInt32();
                    GameInfo.blueGemX = reader.ReadInt32();
                    GameInfo.redGemX = reader.ReadInt32();
                    GameInfo.blueGemCarrier = reader.ReadString();
                    GameInfo.redGemCarrier = reader.ReadString();
                    break;
                
                default:
                    Logger.WarnFormat("CTG2: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}