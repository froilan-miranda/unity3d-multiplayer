using System;
using System.Collections;

public static class PlayerManagerController : System.Object {

	private static  PlayerModel[] arr_players;
	private static uint _rankBase;

	// Use this for initialization
	static PlayerManagerController () {
		CreatePlayers();
	}
	private static void CreatePlayers()
	{
		arr_players = new PlayerModel[] { new PlayerModel(), new PlayerModel(), new PlayerModel() };
	}
	internal static void InitPlayer(uint player, string name)
	{
		arr_players[player-1].Init(name);
	}
	internal static void CancelPlayer(uint player)
	{
		arr_players[player-1].Deactivate();
	}
	internal static void UpdateScore(uint player)
	{
		arr_players[player - 1].IncreaseScore();
	}
	internal static uint GetScore(uint player)
	{
		return arr_players[player -1].Score;
	}
	internal static string GetName(uint player)
	{
		return arr_players[player - 1].Name;
	}
	internal static bool IsPlayerActive(uint player)
	{
		return arr_players[player-1].Active;
	}
	internal static uint TotalScore
	{
		get{
			uint total = 0;
			foreach(PlayerModel p in arr_players) total += p.Score;
			return total;
		}
	}
	internal static uint TopScore
	{
		get{
			uint p1 = GetScore(1);
			uint p2 = GetScore(2);
			uint p3 = GetScore(3);
			if(p1 > p2){
				if(p1>p3){
					//p1 is greatest
					return 1;
				}else{
					//p3 is greatest
					return 3;
				}
			}else{
				if(p2>p3){
					//p2 is greatest
					return 2;
				}else{
					//p3 is greatest
					return 3;
				}
			}
		}
	}
	internal static uint Rank
	{
		get{
			if(TotalScore <= RankBase){
				float ratio = (float)TotalScore/(float)RankBase;
				float percentage = ratio * 100;
				percentage = 100 - percentage;
				return (uint)Math.Round(percentage);
			}else{
				return 1;
			}
		}
	}
	internal static uint RankBase
	{
		get{return _rankBase;}
		set{_rankBase = value;}
	}
	internal static  void ClearPlayers()
	{
		CreatePlayers();
	}
}//class
