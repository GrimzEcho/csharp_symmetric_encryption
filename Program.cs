
/*
Brian von Behren
HW 1

L:i = R:i-1
R:i = L:i-1) ++ F( R:i-1, K:0, K:1, D:i )

L:i+1 = R:i
R:i+1 = L:i ++ F( R:i, K:2, K:3, D:i+1 )

F(X, K:m, K:n, D:y) = ((X << 4) ++ K:m) XOR ((X>>5) ++ K:n) XOR (X ++ D:y)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace hw1
{
    internal class Program
    {
        static uint delta1 = 0x11111111;
        static uint delta2 = 0x22222222;

        //static uint key0 = 0xCAB005E, key1 = 0xCA11AB1E, key2 = 0xDEADBEA7, key3 = 0x1ABE11ED;
        static uint key0 = 0x90001C55, key1 = 0x1234ABCD, key2 = 0xFEDCBA98, key3 = 0xE2468AC0;
        
        private static void Main(string[] args)
        {
            // uint x = 0xA0000009;
            // uint y = 0x8000006B;
            
            Msg("HW 1 -- Start\n");
            
            Msg("Please select B, E, or D:\n\t(B)oth encrypt then decrypt.\n\t(E)ncrypt only\n\t(D)ecrypt only");
            string choice = Console.ReadLine();
            
            Msg("\nPlease enter an 8-bit hexedicimal value for the left word (no error checking, do not prefix with 0x):");
            var left = Console.ReadLine();
            uint leftHex = 0x0;
            uint.TryParse(left, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out leftHex);
            
            Msg("\nPlease enter an 8-bit hexedicimal value for the right word (no error checking, do not prefix with 0x):");
            var right = Console.ReadLine();
            uint rightHex = 0x0;
            uint.TryParse(right, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out rightHex);

            Msg(string.Format("\nParsed input in hex (left - right): {0:X} - {1:X}\n", leftHex, rightHex));

            if (choice.ToLower().Equals("e"))
            {
                Encrypt(leftHex, rightHex);
            }
            else if (choice.ToLower().Equals("d"))
            {
                Decrypt(leftHex, rightHex);

            }
            else
            {
                var encryptRslts = Encrypt(leftHex, rightHex);
                var decryptResults = Decrypt(encryptRslts[0], encryptRslts[1]);    
            }
            
        }
        
        private static uint CoreFunction(uint x, uint keyM, uint keyN, uint delta)
        {
            // F(X, K:m, K:n, D:y) = ((X << 4) ++ K:m) XOR ((X>>5) ++ K:n) XOR (X ++ D:y)
            
//            var tmp1 = x << 4;
//            var tmp2 = tmp1 + keyM;
//            
//            var tmp3 = x >> 5;
//            var tmp4 = tmp3 + keyN;
//            
//            var tmp5 = x + delta;
//
//            var tmp6 = tmp2 ^ tmp4;
//            var result = tmp6 ^ tmp5;

            var result = ((x << 4) + keyM) ^ ((x >> 5) + keyN) ^ (x + delta);
            return result;
        }

        private static List<uint> Encrypt(uint left0, uint right0)
        {
            uint left2 = 0, right2 = 0;

            var left1 = right0;
            var right1 = left0 + CoreFunction(right0, key0, key1, delta1);

            left2 = right1;
            right2 = left1 + CoreFunction(right1, key2, key3, delta2);
            
            var rtnList = new List<uint>();
            rtnList.Add(left2);
            rtnList.Add(right2);

            Msg("...encrypting (2 rounds)");
            Msg(string.Format("Cypher text in hex (left - right): {0:X} - {1:X}\n", left2, right2));
            
            return rtnList;

        }
        
        private static List<uint> Decrypt(uint left2, uint right2)
        {
            uint left0;
            uint right0;

            right0 = right2 - CoreFunction(left2, key2, key3, delta2);
            left0 = left2 - CoreFunction(right0, key0, key1, delta1);
            
            var rtnList = new List<uint>();
            rtnList.Add(left0);
            rtnList.Add(right0);
            
            Msg("...decrypting (2 rounds)");
            Msg(string.Format("Cypher text in hex (left - right): {0:X} - {1:X}", left0, right0));

            return rtnList;
        }


        
        private static void Msg(string s)
        {
            System.Console.WriteLine(s);
        }
        
        private static void Msg(uint i)
        {
            Msg("" + i);
        }
    }
}