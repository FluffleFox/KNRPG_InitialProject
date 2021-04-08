using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;
    public PlaceHolderSaveClass placeHolder;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        LoadGame();
    }

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/Saved_Games");
    }


    public void SaveGame()
    {
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved_Games");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Saved_Games/SaveGame.json");
        //TODO replace placeholder with serializable class
        string json = JsonUtility.ToJson(placeHolder);
        json = Encryption.Encrypt(json);
        binaryFormatter.Serialize(file, json);
        Debug.Log("Game saved at: " + file.Name);
        file.Close();
    }

    public void LoadGame()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Saved_Games")) ;
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved_Games");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Saved_Games/SaveGame.json"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Saved_Games/SaveGame.json", FileMode.Open);
            //TODO replace placeholder with serializable class
            JsonUtility.FromJsonOverwrite(Encryption.Decrypt((string)binaryFormatter.Deserialize(file)), placeHolder);
            Debug.Log("Game loaded at: " + file.Name);
            file.Close();
        }

        //Loading Testing
        Debug.Log("Place holder string: " + placeHolder.kappa);
        Debug.Log("Place holder int: " + placeHolder.dupa);
        Debug.Log("Place holder bool: " + placeHolder.xd);
    }
}
 

