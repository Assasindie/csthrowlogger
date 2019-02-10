using CSGSI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csthrowlogger
{
    class logger
    {
        static GameStateListener gsl;
        static bool metrying = false;
        static string map = "";
        public static void Start()
        {
            if (!getCsgo())
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
                sendMessage("Failed to initialize");
            }
            sendMessage("Started Tracking Throwers!!!");
        }

        public static bool getCsgo()
        {
            Process[] csgo = Process.GetProcessesByName("csgo");
            return (csgo.Length <= 0) ? false : true;
        }

        static void OnNewGameState(GameState gs)
        {
            if (map != gs.Map.Name && gs.Map.Name != String.Empty)
            {
                sendMessage("Currently on Map : " + gs.Map.Name);
                map = gs.Map.Name;
            }
            //what a bad way of doing this haha!!
            List<String> listofweapons = new List<String>(3);
            listofweapons.Add("weapon_c4");
            listofweapons.Add("weapon_usp_silencer");
            listofweapons.Add("weapon_glock");
            if (!metrying && (!listofweapons.Any(gs.Player.Weapons.ActiveWeapon.Name.Contains)))
            {
                metrying = true;
                sendMessage("Ye im trying u mad lmao!!!!!!!!");
            }
        }

        static void RoundEnd(CSGSI.Events.RoundEndEventArgs e)
        {
            sendMessage(e.Winner.ToString() + " has won round " + gsl.CurrentGameState.Map.Round);
            if (e.Winner.ToString() == gsl.CurrentGameState.Player.Team.ToString())
            {
                sendMessage("we unfortunately won this round");
            }
            else
            {
                sendMessage("We lost this round :)!");
            }
            sendMessage("Current score is T : " + gsl.CurrentGameState.Map.TeamT.Score + " CT : " + gsl.CurrentGameState.Map.TeamCT.Score);

            //idk how to handle surrenders lol!!!!!!! im also hungry at this point so this part will be really well done!!
            if(gsl.CurrentGameState.Map.TeamT.Score == 16)
            {
                sendMessage(gsl.CurrentGameState.Player.Team.ToString() == "T" ? "We must have screwed up and won :(!" : "We lost FeelsGoodMan");
            }
            if(gsl.CurrentGameState.Map.TeamCT.Score == 16)
            {
                sendMessage(gsl.CurrentGameState.Player.Team.ToString() == "CT" ? "We must have screwed up and won :(!" : "We lost FeelsGoodMan");
            }
               
        }

        static void RoundBegin(CSGSI.Events.RoundBeginEventArgs e)
        {
            metrying = false;
        }

        //sends to console and webhook
        public static void sendMessage(string message)
        {
            Console.WriteLine(message);
            webhook.SendMessage(message);
        }
    }
}
