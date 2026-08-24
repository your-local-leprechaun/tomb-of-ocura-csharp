using System.Text.Json;
using Rooms;

/// <summary>
/// Proof-of-concept save/load - Room1 only for now, just to verify the
/// MarkDone/IsDone/Replay mechanism actually round-trips through a real file.
/// Inventory/RoomRegistry/Game will get folded into this same save file later.
/// </summary>
public static class SaveManager
{
    private const string FileName = "save.json";

    public static string Save()
    {
        RoomSaveData data = Room1.Get.Save();
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
        return "Game saved.";
    }

    public static string Load()
    {
        if (!File.Exists(FileName))
        {
            return "No save file found.";
        }

        string json = File.ReadAllText(FileName);
        RoomSaveData? data = JsonSerializer.Deserialize<RoomSaveData>(json);
        if (data is null)
        {
            return "Save file was empty or unreadable.";
        }

        Room1.Get.Load(data);
        return "Game loaded.";
    }
}
