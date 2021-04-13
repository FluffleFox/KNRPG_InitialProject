using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;
    public string[] saveSlots = { "auto_save", "save_slot_1", "save_slot_2", "save_slot_3", "save_slot_4", "save_slot_5" };
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
    }

    public bool IsSaveFolder()
    {
        return Directory.Exists(Application.persistentDataPath + "/Saved_Games");
    }


    public void SaveGame(int index)
    {
        if (!IsSaveFolder())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved_Games");
        }
        if (File.Exists(Application.persistentDataPath + "/Saved_Games/" + saveSlots[index] + ".json"))
        {
            //TODO Handle overwriting of save files
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Saved_Games/" + saveSlots[index] + ".json");
        //TODO replace placeholder with serializable class
        string json = JsonUtility.ToJson(placeHolder);
        json = Encryption.Encrypt(json);
        binaryFormatter.Serialize(file, json);
        Debug.Log("Game saved at: " + file.Name);
        file.Close();
    }

    public void LoadGame(int index)
    {
        if (Directory.Exists(Application.persistentDataPath + "/Saved_Games")) ;
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved_Games");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Saved_Games/" + saveSlots[index] + ".json"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Saved_Games/" + saveSlots[index] + ".json", FileMode.Open);
            //TODO replace placeholder with serializable class
            try
            {
                JsonUtility.FromJsonOverwrite(Encryption.Decrypt((string)binaryFormatter.Deserialize(file)), placeHolder);
                Debug.Log("Game loaded at: " + file.Name);
                file.Close();
            }
            catch(CryptographicException)
            {
                //TODO handle cryptography exception
                file.Close();
                Debug.Log("File has been corrupted, loading failed the file will be deleted");
                File.Delete(Application.persistentDataPath + "/Saved_Games/" + saveSlots[index] + ".json");
                Debug.Log("File deleted");
            }
       
        }
        //Loading Testing
        Debug.Log("Place holder string: " + placeHolder.kappa);
        Debug.Log("Place holder int: " + placeHolder.dupa);
        Debug.Log("Place holder bool: " + placeHolder.xd);
    }
}
 

