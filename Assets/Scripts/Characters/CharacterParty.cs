using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterParty{

   public static List<PlayableCharacter> charactersParty;
   
    public static void Init()
    {
        if (charactersParty == null)
        {
            charactersParty = new List<PlayableCharacter>();
        }
    }

    public static bool JoinParty(PlayableCharacter player)
    {
        if (!charactersParty.Exists(p => p.charStats.name == player.charStats.name))
        {
            charactersParty.Add(player);
            return true;
        }
        return false;
    }

    public static void UpdateCharacter(PlayableCharacter player)
    {
      
        int index = charactersParty.FindIndex(p => p.charStats.name.Equals(player.charStats.name));
   
        charactersParty.RemoveAt(index);
        charactersParty.Insert(index, player);
    
    }

    public static PlayableCharacter GetCharacterData(string name)
    {
        return charactersParty.Find(p => p.charStats.name.Equals(name));
    }

    public static void UpdateCharacterGameObject(string name, GameObject go)
    {
        foreach (PlayableCharacter character in charactersParty)
        {
            if (character.charStats.name.Equals(name))
                character.CharGameObject = go;
        }
    }
}
