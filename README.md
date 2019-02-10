# csthrowlogger
Sends realtime information about your csgo game that you are throwing to a discord server

## Installation
Via NuGet:

```
Install-Package CSGSI
Install-Package Discord.Net
```
You will also need to create a gamestate_integration_name.cfg in your csgo config file this is a working example:   
```
"DotMa"
{
	"uri" "http://localhost:3000"
	"timeout" "5.0"
	"data"
	{
		"provider"              	"1"
		"map"                   	"1"
		"round"                 	"1"
		"player_id"					"1"
		"player_weapons"			"1"
		"player_match_stats"		"1"
		"player_state"				"1"
		"allplayers_id"				"1"
		"allplayers_state"			"1"
		"allplayers_match_stats"	"1"
	}
}
