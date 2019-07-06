using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.Linq;
using System;


public class AttachmentBase58{
    static string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    public static string DecimalToHex(BigInteger dec)
    {
        var s = dec.ToString();
        var result = new List<byte>();
        result.Add(0);
        foreach (char c in s)
        {
            int val = (int)(c - '0');
            for (int i = 0; i < result.Count; i++)
            {
                int digit = result[i] * 10 + val;
                result[i] = (byte)(digit & 0x0F);
                val = digit >> 4;
            }
            if (val != 0)
                result.Add((byte)val);
        }

        var hex = "";
        foreach (byte b in result)
            hex = "0123456789ABCDEF"[b] + hex;
        return hex;
    }
    public static string HexToDecimal(string hex)
    {
        List<int> dec = new List<int> { 0 };

        foreach (char c in hex)
        {
            int carry = Convert.ToInt32(c.ToString(), 16);

            for (int i = 0; i < dec.Count; ++i)
            {
                int val = dec[i] * 16 + carry;
                dec[i] = val % 10;
                carry = val / 10;
            }

            while (carry > 0)
            {
                dec.Add(carry % 10);
                carry /= 10;
            }
        }

        var chars = dec.Select(d => (char)('0' + d));
        var cArr = chars.Reverse().ToArray();
        return new string(cArr);
    }

    public static string Base58NumericEncode(BigInteger num)
    {
        string encoded = string.Empty;
        if (num == 0)
        {
            encoded = alphabet[0] + encoded;
        }
        while (num > 0)
        {
            BigInteger numRem = num % 58;
            num = num / 58;
            encoded = alphabet[(int)numRem] + encoded;
        }
        return encoded;
    }
    public static string Base58NumericDecode(string encoded)
    {
        BigInteger conv = 0;
        BigInteger numTemp = 1;
        if (!string.IsNullOrEmpty(encoded))
        {
            while (encoded.Length > 0)
            {
                char currentChar = encoded[encoded.Length - 1];
                conv += (numTemp * alphabet.IndexOf(currentChar));
                numTemp *= 58;
                encoded = encoded.Remove(encoded.Length - 1);
            }
        }
        return DecimalToHex(conv);
    }
}
