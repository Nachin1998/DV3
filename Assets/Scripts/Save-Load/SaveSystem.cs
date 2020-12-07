using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Player player, PlayerMovement pm, BaseWeapon bw)
    {
        string path = Application.persistentDataPath + "player.ftw";

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player, pm, bw);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "player.ftw";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
