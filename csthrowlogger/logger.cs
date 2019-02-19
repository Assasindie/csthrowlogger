using CSGSI;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace csthrowlogger
{
    class Logger
    {
        static GameStateListener gsl;
        static bool meTrying = false;
        static int killsGotten = -1;
        static bool meDying = false;
        static string map = "";
        public static DiscordRpcClient client;
        public static DateTime roundStart = DateTime.UtcNow;
        public static bool stopped = false;
        public static Timer csCheck;

        public static void Start()
        {
            csCheck = new Timer(10000)
            {
                AutoReset = true
            };
            csCheck.Elapsed += CsCheck_Elapsed;
            csCheck.Start();
            Logger log = new Logger();
            log.Initialize();
            if (!GetCsgo())
            {
                Console.WriteLine("CSGO not open, please try again with CSGO open! Press any key to exit");
                Console.ReadKey();
                return;
            }
            gsl = new GameStateListener(3000);
            gsl.NewGameState += new NewGameStateHandler(OnNewGameState);
            gsl.RoundEnd += RoundEnd;
            gsl.RoundBegin += RoundBegin;
            gsl.EnableRaisingIntricateEvents = true;
            if (!gsl.Start())
            {
                SendMessage("Failed to initialize");
            }
            SendMessage("Started Tracking Throwers!!!");
        }

        private static void CsCheck_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!GetCsgo())
            {
                SendMessage("CSGO is no longer open!");
                client.ClearPresence();
                stopped = true;
            }else if (stopped)
            {
                roundStart = DateTime.UtcNow;
                stopped = false;
            }
        }

        public static bool GetCsgo()
        {
            Process[] csgo = Process.GetProcessesByName("csgo");
            return (csgo.Length <= 0) ? false : true;
        }

        static void OnNewGameState(GameState gs)
        {
            if (gsl.CurrentGameState.Map.Name == "" && gsl.CurrentGameState.Player.MatchStats.Kills == -1)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = "In Main Menu",
                    State = "Main Menu",
                    Assets = new Assets()
                    {
                        LargeImageKey = "mainmenu",
                        LargeImageText = "Main Menu"
                    },
                    Timestamps = new Timestamps(roundStart),
                });
            }
            else
            {
                if (gsl.CurrentGameState.Provider.SteamID == gsl.CurrentGameState.Player.SteamID)
                {
                    client.SetPresence(new RichPresence()
                    {
                        Details = gsl.CurrentGameState.Player.MatchStats.Kills + "-" + gsl.CurrentGameState.Player.MatchStats.Assists + "-" + gsl.CurrentGameState.Player.MatchStats.Deaths
                        + " " + gsl.CurrentGameState.Player.Weapons.ActiveWeapon.Name,

                        State = "CT : " + gsl.CurrentGameState.Map.TeamCT.Score + " T : " + gsl.CurrentGameState.Map.TeamT.Score + " " + gsl.CurrentGameState.Map.Phase,
                        Assets = new Assets()
                        {
                            LargeImageKey = gsl.CurrentGameState.Map.Name,
                            LargeImageText = gsl.CurrentGameState.Map.Name + " - " + gsl.CurrentGameState.Map.Mode.ToString(),
                            SmallImageKey = "team" + gsl.CurrentGameState.Player.Team.ToString().ToLower(),
                            SmallImageText = "Team " + gsl.CurrentGameState.Player.Team.ToString() + " - " + gsl.CurrentGameState.Player.State.Health + " Health",
                        },
                        Timestamps = new Timestamps(roundStart),
                    });
                }
                client.Invoke();
                if (gs.Player.State.Health == 0 && !meDying)
                {
                    SendMessage("IMAGINE DIEING  @" + gs.Player.Name);
                    meDying = true;
                }

                if (gs.Player.State.RoundKills > 0 && gs.Player.State.RoundKills != killsGotten)
                {
                    killsGotten = gs.Player.State.RoundKills;
                    SendMessage("IMAGINE TRYING AND GETTING " + killsGotten + "KILL(S) @" + gs.Player.Name);
                }

                if (map != gs.Map.Name && gs.Map.Name != String.Empty)
                {
                    SendMessage("Currently on Map : " + gs.Map.Mode + " " + gs.Map.Name);
                    map = gs.Map.Name;
                }

                List<String> listofweapons = new List<String>(3)
            {
                "weapon_c4",
                "weapon_usp_silencer",
                "weapon_glock"
            };

                if (!meTrying && (!listofweapons.Any(gs.Player.Weapons.ActiveWeapon.Name.Contains)))
                {
                    meTrying = true;
                    SendMessage("IMAGINE TRYING @" + gs.Player.Name);
                }
            }
        }

        static void RoundEnd(CSGSI.Events.RoundEndEventArgs e)
        {
            SendMessage(e.Winner.ToString() + " has won round " + gsl.CurrentGameState.Map.Round);
            if (e.Winner.ToString() == gsl.CurrentGameState.Player.Team.ToString())
            {
                SendMessage("we unfortunately won this round");
            }
            else
            {
                SendMessage("We lost this round :)!");
            }
            SendMessage("Current score is T : " + gsl.CurrentGameState.Map.TeamT.Score + " CT : " + gsl.CurrentGameState.Map.TeamCT.Score);

            //idk how to handle surrenders lol!!!!!!! im also hungry at this point so this part will be really well done!!
            if(gsl.CurrentGameState.Map.TeamT.Score == 16)
            {
                SendMessage(gsl.CurrentGameState.Player.Team.ToString() == "T" ? "We must have screwed up and won :(!" : "We lost FeelsGoodMan");
                SendMessage("End of game statistics for " +gsl.CurrentGameState.Player.Name + " : \n " + gsl.CurrentGameState.Player.MatchStats.Kills.ToString() + " kills, "
                    + gsl.CurrentGameState.Player.MatchStats.Deaths.ToString() + " deaths, " + gsl.CurrentGameState.Player.MatchStats.MVPs.ToString()
                    + " mvps!" );
            }
            if(gsl.CurrentGameState.Map.TeamCT.Score == 16)
            {
                SendMessage(gsl.CurrentGameState.Player.Team.ToString() == "CT" ? "We must have screwed up and won :(!" : "We lost FeelsGoodMan");
                SendMessage("End of game statistics for " + gsl.CurrentGameState.Player.Name + " : \n " + gsl.CurrentGameState.Player.MatchStats.Kills.ToString() + " kills, "
                    + gsl.CurrentGameState.Player.MatchStats.Deaths.ToString() + " deaths, " + gsl.CurrentGameState.Player.MatchStats.MVPs.ToString()
                    + " mvps!");
            }
               
        }

        static void RoundBegin(CSGSI.Events.RoundBeginEventArgs e)
        {
            SendMessage("Beginning round " + e.TotalRound);
            meTrying = false;
            meDying = false;
            killsGotten = -1;
            roundStart = DateTime.UtcNow;
            client.Invoke();
        }

        //sends to console and webhook
        public static void SendMessage(string message)
        {
            Console.WriteLine(System.DateTime.Now + " " + message);
            Webhook.SendMessage(System.DateTime.Now + " " + message);
        }

        void Initialize()
        {
            /*
            Create a discord client
            NOTE: 	If you are using Unity3D, you must use the full constructor and define
                     the pipe connection as DiscordRPC.IO.NativeNamedPipeClient
            */
            client = new DiscordRpcClient("545493713779163147");

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {

            };

            client.Initialize();
        }
    }
}
