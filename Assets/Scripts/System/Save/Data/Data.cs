using Save;

public class UserData : IData, ISaveable
{
    public static readonly string FILE_NAME = "user_data";
    
    public int TimeBonus;
    public int Hints;

    public UserData(int hints, int timeBonus)
    {
        Hints = hints;
        TimeBonus = timeBonus;
    }

    public void Accept(ISaveVisitor visitor)
    {
        visitor.Visit(this);   
    }

    public void Accept(ILoadVisitor loadVisitor)
    {
        loadVisitor.Visit(this);
    }
}

public class SettingsData : IData, ISaveable
{
    public static readonly string FILE_NAME = "settings";
    
    public bool Music;
    public bool Sound;
    public bool Vibration;

    public SettingsData(bool music, bool sound, bool vibration)
    {
        Music = music;
        Sound = sound;
        Vibration = vibration;
    }

    public void Accept(ISaveVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ILoadVisitor loadVisitor)
    {
        loadVisitor.Visit(this);
    }
}