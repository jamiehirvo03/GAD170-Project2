using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewmateGenerator : MonoBehaviour
{
    private List<string> firstNames = new List<string>(15) { "Bob", "Dougie", "Hannah", "Jenny", "Peter", "Melissa", "Hugo", "Natasha", "Timothy", "Nancy", "Johnny", "Robert", "Chloe", "Emily", "Mike" };
    private List<string> lastNames = new List<string>(26) { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H.", "I.", "J.", "K.", "L.", "M.", "N.", "O.", "P.", "Q.", "R.", "S.", "T.", "U.", "V.", "W.", "X.", "Y.", "Z." };
    private List<string> humanHobbies = new List<string>(5) { "Bowling", "Surfing", "Fishing", "Kareoke", "Reading" };
    private List<string> parasiteHobbies = new List<string>(5) { "Invasion", "Stargazing", "Acting", "Espionage", "Birdwatching" };


    public string GetFirstName()
    {
        return firstNames[Random.Range(0,firstNames.Count)];
    }

    public string GetLastName()
    {
        return lastNames[Random.Range(0,lastNames.Count)];    
    }

    public string GetHumanHobby()
    {
        return humanHobbies[Random.Range(0,humanHobbies.Count)];
    }

    public string GetParasiteHobby()
    {
        return parasiteHobbies[Random.Range(0,parasiteHobbies.Count)];
    }
}
