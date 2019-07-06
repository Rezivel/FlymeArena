using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressInfo : MonoBehaviour {
    public string address;
    public static string Asset = "D39gyBeFXkff5xre1zRpLAf8o3JSCowGEp9sJe8XK7gW";
    public static string[] recipient =
    {
        "3PL9MfngUCdYKhrjtSJq9kcM3GRxaTtJnRQ",
        "3P89HHaokT8QgTVg65m9kc5MeeeuciTSToe"
    };
    void Start () {
        DontDestroyOnLoad(gameObject);
	}
    public void SetAddress(string _address)
    { 
        address = _address;
    }
    
}
