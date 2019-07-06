using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WavesCS;

public class Players
{
    public string Address;
    public int[] Skills;
    public Players(string address, int[] skills)
    {
        Address = address;
        Skills = skills;
    }
}

public class Txs
{
    public string Tx;
    public bool Sender = false;
    public string AddressSender;
    public int Height;
    public Txs(string tx, bool sender, string address, int height)
    {
        Tx = tx;
        Sender = sender;
        AddressSender = address;
        Height = height;
    }
}

public class SkillsPlayer
{
    public int[] Skills;
    public int Height = -1;
    public bool PrevSkills = false;
    public int HeightPrev = -1;
    public SkillsPlayer()
    {

    }
    public SkillsPlayer(int[] skills, int height)
    {
        Skills = skills;
        Height = height;
    }
    public void SetPrevSkills(bool prevSkills, int height)
    {
        PrevSkills = prevSkills;
        HeightPrev = height;
    }
}

public class LoadGameHistory : MonoBehaviour
{
    public GameObject GameHistory;
    public List<GameObject> panels;

    public void LoadHistory(string address)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            Destroy(panels[i]);
        }

        int height = new Node(Node.MainNetHost).GetHeight();

        SkillsPlayer skillsplayer = AddressSkills(address, height);
        List<Players> players = GetPlayers(address, height);
        List<Txs> txs = LoadTx(address, skillsplayer.Height, height);

        Load(skillsplayer,players,txs,height,address);
        LoadPrevHistory(skillsplayer, players, txs, height, address);

    }

    public void LoadPrevHistory(SkillsPlayer skillsplayer, List<Players> players, List<Txs> txs, int height, string address)
    {
        while (skillsplayer.PrevSkills)
        {
            skillsplayer = AddressSkills(address, height);
            int height_new = skillsplayer.Height;
            skillsplayer = AddressSkills(address, skillsplayer.HeightPrev);
            players = GetPlayers(address, height_new);
            txs = LoadTx(address, skillsplayer.Height, height_new);
            Load(skillsplayer, players, txs, height, address);
        }
    }


    public void Load(SkillsPlayer skillsplayer, List<Players> players, List<Txs> txs, int height,string address)
    {
        for (int i = 0; i < txs.Count; i++)
        {
            players = GetPlayers(address, txs[i].Height);
            if (!txs[i].Sender)
            {
                List<Players> _players = GetPlayers(txs[i].AddressSender, txs[i].Height);
                int seed = int.Parse(AttachmentBase58.HexToDecimal(AttachmentBase58.Base58NumericDecode(txs[i].Tx.Substring(0, 4))));
                LinearConRng rnd = new LinearConRng(seed);
                int enemy = rnd.NextInt(0, _players.Count);
                SkillsPlayer skillsEnemy = AddressSkills(txs[i].AddressSender, txs[i].Height);
                if (_players[enemy].Address == address && skillsEnemy.Height <= txs[i].Height)
                {
                    var panelGO = Instantiate(GameHistory, transform);
                    panelGO.GetComponent<GameHistoryPanel>().SetStats(txs[i].Tx, txs[i].AddressSender, skillsplayer.Skills, skillsEnemy.Skills, Convert.ToInt32(txs[i].Sender));
                    panels.Add(panelGO);
                }
            }
            else
            {
                int seed = int.Parse(AttachmentBase58.HexToDecimal(AttachmentBase58.Base58NumericDecode(txs[i].Tx.Substring(0, 4))));
                LinearConRng rnd = new LinearConRng(seed);
                int enemy = rnd.NextInt(0, players.Count);
                SkillsPlayer skillsEnemy = AddressSkills(players[enemy].Address, txs[i].Height);
                if (skillsEnemy.Height <= txs[i].Height)
                {
                    var panelGO = Instantiate(GameHistory, transform);
                    panelGO.GetComponent<GameHistoryPanel>().SetStats(txs[i].Tx, players[enemy].Address, skillsplayer.Skills, players[enemy].Skills, Convert.ToInt32(txs[i].Sender));
                    panels.Add(panelGO);
                }
            }
        }
    }

    public SkillsPlayer AddressSkills(string address, int height)
    {
        SkillsPlayer skills = new SkillsPlayer();
        var node = new Node(Node.MainNetHost);
        Dictionary<string, object>[] player = node.GetTransactionsByAddress(AddressInfo.recipient[0], 1000);
        for (int j = 0; j < player.Length; j++)
        {
            bool correctTx = int.Parse(player[j].GetValue("type").ToString()) == 4 &&
            player[j].GetValue("assetId") != null &&
            player[j].GetValue("assetId").ToString() == AddressInfo.Asset &&
            int.Parse(player[j].GetValue("amount").ToString()) >= 100000000 &&
            !string.IsNullOrEmpty(player[j].GetValue("attachment").ToString()) &&
            int.Parse(player[j].GetValue("height").ToString()) <= height;

            if (correctTx && player[j].GetValue("sender").ToString() == address)
            {
                string DecodedAttachment = AttachmentBase58.Base58NumericDecode(Encoding.UTF8.GetString(Base58.Decode(player[j].GetValue("attachment").ToString())));

                string[] info = new string[3];
                info = DecodedAttachment.Split('A');
                if (string.IsNullOrEmpty(info[0]))
                {
                    info[0] = "0";
                }
                int[] skillsArr = new int[3];
                try
                {
                    if (int.TryParse(info[0], out skillsArr[0]) &&
                    int.TryParse(info[1], out skillsArr[1]) &&
                    int.TryParse(info[2], out skillsArr[2]) &&
                    skillsArr[0] >= 0 && skillsArr[0] < FindObjectOfType<Skills>().skills.Count &&
                    skillsArr[1] >= 0 && skillsArr[1] < FindObjectOfType<Skills>().skills.Count &&
                    skillsArr[2] >= 0 && skillsArr[2] < FindObjectOfType<Skills>().skills.Count)
                    {
                        if (skills.Height == -1)
                        {
                            skills = new SkillsPlayer(skillsArr, int.Parse(player[j].GetValue("height").ToString()));
                        }
                        else
                        {
                            skills.SetPrevSkills(true, int.Parse(player[j].GetValue("height").ToString()));
                            return skills;
                        }
                    }
                }
                catch
                {
                    continue;
                }
                
            }

        }

        return skills;
    }

    public List<Players> GetPlayers(string address, int height)
    {
        List<Players> playersList = new List<Players>();
        var node = new Node(Node.MainNetHost);
        Dictionary<string, object>[] enemies = node.GetTransactionsByAddress(AddressInfo.recipient[0], 1000);
        for (int j = 0; j < enemies.Length; j++)
        {
            bool correctTx = int.Parse(enemies[j].GetValue("type").ToString()) == 4 &&
            enemies[j].GetValue("assetId") != null &&
            enemies[j].GetValue("assetId").ToString() == AddressInfo.Asset &&
            int.Parse(enemies[j].GetValue("amount").ToString()) >= 100000000 &&
            !string.IsNullOrEmpty(enemies[j].GetValue("attachment").ToString()) &&
            int.Parse(enemies[j].GetValue("height").ToString()) <= height;

            if (correctTx && enemies[j].GetValue("sender").ToString() != address)
            {
                string enemy = enemies[j].GetValue("sender").ToString();
                bool equalStr = false;
                for (int k = 0; k < playersList.Count; k++)
                {
                    if (enemy == playersList[k].Address)
                    {
                        equalStr = true;
                        //has prev skills
                    }
                }
                if (!equalStr)
                {
                    string DecodedAttachment = AttachmentBase58.Base58NumericDecode(Encoding.UTF8.GetString(Base58.Decode(enemies[j].GetValue("attachment").ToString())));

                    string[] info = new string[3];
                    info = DecodedAttachment.Split('A');
                    if (string.IsNullOrEmpty(info[0]))
                    {
                        info[0] = "0";
                    }
                    int[] skillsEnemies = new int[3];
                    try
                    {
                        if (int.TryParse(info[0], out skillsEnemies[0]) &&
                        int.TryParse(info[1], out skillsEnemies[1]) &&
                        int.TryParse(info[2], out skillsEnemies[2]) &&
                        skillsEnemies[0] >= 0 && skillsEnemies[0] < FindObjectOfType<Skills>().skills.Count &&
                        skillsEnemies[1] >= 0 && skillsEnemies[1] < FindObjectOfType<Skills>().skills.Count &&
                        skillsEnemies[2] >= 0 && skillsEnemies[2] < FindObjectOfType<Skills>().skills.Count)
                        {
                            playersList.Add(new Players(enemies[j].GetValue("sender").ToString(), skillsEnemies));
                        }
                    }
                    catch
                    {

                        continue;
                    }
                }
            }
        }
        return playersList;
    }

    public List<Txs> LoadTx(string address, int heightMin, int heightMax)
    {
        List<Txs> txs = new List<Txs>();
        var node = new Node(Node.MainNetHost);
        Dictionary<string, object>[] transactions = node.GetTransactionsByAddress(AddressInfo.recipient[1], 1000);
        for (int i = 0; i < transactions.Length; i++)
        {
            if (int.Parse(transactions[i].GetValue("type").ToString()) == 4 &&
                transactions[i].GetValue("assetId") != null &&
                transactions[i].GetValue("assetId").ToString() == AddressInfo.Asset &&
                int.Parse(transactions[i].GetValue("amount").ToString()) >= 100000000 &&
                string.IsNullOrEmpty(transactions[i].GetValue("attachment").ToString()) &&
                transactions[i].GetValue("recipient").ToString() == AddressInfo.recipient[1] &&
                int.Parse(transactions[i].GetValue("height").ToString()) >= heightMin &&
                int.Parse(transactions[i].GetValue("height").ToString()) <= heightMax)
            {
                if (transactions[i].GetValue("sender").ToString() == address)
                {
                    txs.Add(new Txs(transactions[i].GetValue("id").ToString(), true, transactions[i].GetValue("sender").ToString(), int.Parse(transactions[i].GetValue("height").ToString())));
                }
                else
                {
                    txs.Add(new Txs(transactions[i].GetValue("id").ToString(), false, transactions[i].GetValue("sender").ToString(), int.Parse(transactions[i].GetValue("height").ToString())));
                }
            }
        }
        return txs;
    }
}
