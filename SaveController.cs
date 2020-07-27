using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveController{
    public static void saveGame(GameController gameController) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "player.save");

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameController);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerData loadGame() {
        string path = Path.Combine(Application.persistentDataPath, "player.save");
        Debug.Log(path);

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else {
            return null;
        }
    }
}
