using System.Collections.Generic;

public static class PlayerSelectionStore
{
    private static Dictionary<ulong, int> selections = new Dictionary<ulong, int>();

    public static void SetSelection(ulong clientId, int characterId)
    {
        selections[clientId] = characterId;
    }

    public static int GetSelection(ulong clientId)
    {
        if (selections.TryGetValue(clientId, out int id))
            return id;

        return -1;
    }

    public static void Clear()
    {
        selections.Clear();
    }
}
