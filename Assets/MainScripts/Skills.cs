using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using WavesCS;
using System.Text;

public class Skills : MonoBehaviour {
    public List<Skill> skills;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Dictionary<string,object> a = new Node(Node.TestNetHost).GetAddressData("3N9L8LAAPnzqfw6Cm3fDJ4jcFX5QM2ht6Ct");
        //string path = Application.streamingAssetsPath + "/Skills.json";
        //string JsonSkills = File.ReadAllText(path);
        //skills = JsonConvert.DeserializeObject<List<Skill>>(JsonSkills);
        skills = JsonConvert.DeserializeObject<List<Skill>>(Encoding.UTF8.GetString(Base58.Decode(a.GetValue("skills").ToString())));
    }

}
