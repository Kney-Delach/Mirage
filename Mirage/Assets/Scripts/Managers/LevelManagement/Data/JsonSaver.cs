using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

namespace LevelManagement.Data
{
    public class JsonSaver
    {
        // default filename
        private static readonly string _filename = "saveData1.sav";

        // returns filename with fullpath
        public static string GetSaveFilename()
        {
            return Application.persistentDataPath + "/" + _filename;
        }

        // convert the SaveData to JSON format and write to disk
        public void Save(SaveData data)
        {
            data.hashValue = String.Empty;

            // convert the data to a JSON-formatted string
            string json = JsonUtility.ToJson(data);

            // generate a hash value as a hexidecimal string and store in SaveData 
            data.hashValue = GetSHA256(json);

            // convert the data to JSON (to add the hash string)
            json = JsonUtility.ToJson(data);

            string saveFilename = GetSaveFilename();

            FileStream filestream = new FileStream(saveFilename, FileMode.Create);

            using (StreamWriter writer = new StreamWriter(filestream))
            {
                writer.Write(json);
            }
        }

        // load the data from disk and overwrite the contents of SaveData object
        public bool Load(SaveData data)
        {
            // reference to filename
            string loadFilename = GetSaveFilename();

            if (File.Exists(loadFilename))
            {
                using (StreamReader reader = new StreamReader(loadFilename))
                {
                    // read the file as a string
                    string json = reader.ReadToEnd();

                    // verify the data using the hash value
                    if (CheckData(json))
                        JsonUtility.FromJsonOverwrite(json, data);
                    else
                        Debug.LogWarning("JSONSAVER Load: invalid hash`");
                }
                return true;
            }
            return false;
        }

        // verifies if a save file has a valid hash
        private bool CheckData(string json)
        {
            SaveData tempSaveData = new SaveData();

            JsonUtility.FromJsonOverwrite(json, tempSaveData);

            // grab the saved hash value and reset
            string oldHash = tempSaveData.hashValue;
            tempSaveData.hashValue = String.Empty;

            // generate a temporary JSON file with the hash reset to empty
            string tempJson = JsonUtility.ToJson(tempSaveData);

            // calculate the hash 
            string newHash = GetSHA256(tempJson);

            // return whether the old and new hash values match
            return (oldHash == newHash);
        }

        // deletes the save file from disk
        public void Delete()
        {
            File.Delete(GetSaveFilename());
        }

        // converts an array of bytes into a hexidecimal string 
        public string GetHexStringFromHash(byte[] hash)
        {
            string hexString = String.Empty;

            foreach (byte b in hash)
                hexString += b.ToString("x2");

            return hexString;
        }

        // converts a string into a SHA256 hash value
        private string GetSHA256(string text)
        {
            byte[] textToBytes = Encoding.UTF8.GetBytes(text);

            SHA256Managed mySHA256 = new SHA256Managed();

            byte[] hashValue = mySHA256.ComputeHash(textToBytes);

            return GetHexStringFromHash(hashValue);
        }
    }
}
