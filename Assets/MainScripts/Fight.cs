using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WavesCS;
using System.IO;
using Newtonsoft.Json;

public class Fight : MonoBehaviour {

    public static int CalculateFight(int[] player1, int[] player2, int seed, int steps = 3)
    {
        int player = new LinearConRng(seed).NextInt(0, 2);
        Player player_1 = new Player();
        Player player_2 = new Player();
        Debug.Log("Player RND : "+player);
        for (int i = 0; i < steps; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if(player == 0)
                {
                    UseSkill(player_1, player_2, player, player1[i],steps - i, FindObjectOfType<Skills>().skills);
                    Debug.Log("Player2 HP : " + player_2.HP);
                    if (player_2.HP <= 0)
                    {
                        return player;
                    }
                    player = NextPlayer(player);
                }
                else
                {
                    UseSkill(player_1, player_2, player, player2[i], steps - i, FindObjectOfType<Skills>().skills);
                    Debug.Log("Player1 HP : " + player_1.HP);
                    if (player_1.HP <= 0)
                    {
                        return player;
                    }
                    player = NextPlayer(player);
                }
            }
        }
        return 2;
    }

    static int NextPlayer(int player)
    {
        int min = 0, max = 1;
        player++;
        if(player > max)
        {
            player = min;
        }
        return player;
    }

    static void UseSkill(Player player1, Player player2, int player, int spellID, int step, List<Skill> skills)
    {
        if(player == 0)
        {
            if (player1.immune_to_dmg)
            {
                player1.immune_to_dmg = false;
            }
            if(skills[spellID].Buff == true)
            {
                player1.immune_to_dmg = true;
            }
            else
            {
                if (!player2.immune_to_dmg && skills[spellID].Range >= step)
                {
                    player2.HP -= skills[spellID].Damage;
                }
            }
            
        }
        else
        {
            if (player2.immune_to_dmg)
            {
                player2.immune_to_dmg = false;
            }
            if (skills[spellID].Buff == true)
            {
                player2.immune_to_dmg = true;
            }
            else
            {
                if (!player1.immune_to_dmg && skills[spellID].Range >= step)
                {
                    player1.HP -= skills[spellID].Damage;
                }
            }
        }
    }
}
