using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ReadPrivateFiles
{
    private static string _privateDirectory; // path to private directory
    private static bool _isSetUp; // has path been generated


    /// <summary>
    /// Function to retrieve weather data api key
    /// </summary>
    /// <returns>Api key</returns>
    public static string GetAPIKey()
    {
        if(!_isSetUp) // if class variable not set
        {
            GetPrivateDirectory(); // get project path
            _isSetUp = true;
        }

        string keyPath = _privateDirectory + "key.txt"; // ket file path of api key

        if(File.Exists(keyPath)) // if the filepath exists
        {
            using (StreamReader reader = new StreamReader(keyPath))
            {
                string keyContent = reader.ReadToEnd(); // read content of key file
                string[] keySplit = keyContent.Split('\"'); // split content up into words
                reader.Close();
                return keySplit[1]; // return key value
            }
        }

        return "";
    }

    /// <summary>
    /// Function to get the directory of the private folder
    /// </summary>
    private static void GetPrivateDirectory()
    {
        string cD = Directory.GetCurrentDirectory(); // get current directory of file
        string[] subD = cD.Split('\\'); // split sub directories
        string privateD = "";

        for(int i = 0; i < subD.Length; i++) // for each directory
        {
            privateD += subD[i] + "\\"; // add to path 
            if (subD[i+1] == "Unity Project") // if root project directory
            {
                privateD += "private\\"; // add private file
                break;
            }
        }

        _privateDirectory = privateD; // set to class variable

        return;
    }
}