using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ReadFile
{
    /// <summary>
    /// Function to retrieve weather data api key
    /// </summary>
    /// <returns>Api key</returns>
    public static string GetAPIKey()
    {
        string cd = Directory.GetCurrentDirectory(); // get full directory of project
        string[] files = cd.Split('\\'); // split each directory
        string filePath = "";

        foreach(string word in files) // for each directory
        {
            filePath += word + "\\"; // add to path
            if (word == "Unity Project")
            {
                filePath += "User\\key.txt";
                break;
            } 
        }

        if(File.Exists(filePath)) // if the filepath exists
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string key = reader.ReadToEnd(); // read key
                reader.Close();
                return key;
            }
        }

        return "";
    }
}