using CSGSI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csthrowlogger
{
    class Logger
    {
        static GameStateListener gsl;
        static bool meTrying = false;
        static int killsGotten = 0;
        static string map = "";
        public static void Start()
        {
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

        public static bool GetCsgo()
        {
            Process[] csgo = Process.GetProcessesByName("csgo");
            return (csgo.Length <= 0) ? false : true;
        }

        static void OnNewGameState(GameState gs)
        {
            if(gs.Player.State.Health == 0)
            {
                SendMessage("IMAGINE DIEING");
            }

            if(gs.Player.State.RoundKills > 0 && gs.Player.State.RoundKills != killsGotten)
            {
                killsGotten = gs.Player.State.RoundKills;
                SendMessage("IMAGINE TRYING AND GETTING " + killsGotten + "KILL(S)");
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
                SendMessage("IMAGINE TRYING");
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
                SendMessage("End of game statistics: \n kills : " + gsl.CurrentGameState.Player.MatchStats.Kills.ToString() + " deaths : "
                    + gsl.CurrentGameState.Player.MatchStats.Deaths.ToString() + " mvps : " + gsl.CurrentGameState.Player.MatchStats.MVPs.ToString());
            }
            if(gsl.CurrentGameState.Map.TeamCT.Score == 16)
            {
                SendMessage(gsl.CurrentGameState.Player.Team.ToString() == "CT" ? "We must have screwed up and won :(!" : "We lost FeelsGoodMan");
            }
               
        }

        static void RoundBegin(CSGSI.Events.RoundBeginEventArgs e)
        {
            SendMessage("Beginning round " + e.TotalRound);
            meTrying = false;
        }

        //sends to console and webhook
        public static void SendMessage(string message)
        {
            Console.WriteLine(message);
            Webhook.SendMessage(message);
        }
    }
}
