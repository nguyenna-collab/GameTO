using System.Collections.Generic;
using Save;


public class UserData : IData
{
    // Gameplay
    public int TimeBonus;
    public int Hints;
    
    // Settings
    public bool Music;
    public bool Sound;
    public bool Vibration;

    public UserData(int hints, int timeBonus, bool music, bool sound, bool vibration)
    {
        Hints = hints;
        TimeBonus = timeBonus;
        Music = music;
        Sound = sound;
        Vibration = vibration;
    }
}