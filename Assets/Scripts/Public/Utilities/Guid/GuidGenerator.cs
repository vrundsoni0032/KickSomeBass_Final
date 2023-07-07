using System;

public enum GuidAppendedID { One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9}
public static class GuidGenerator
{
    public static Guid GenerateGuid() { return Guid.NewGuid(); }
    private static UInt64 Internal_GenerateUInt64Guid() { return BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0); }
    public static UInt64 GenerateUInt64Guid(GuidAppendedID appendedID) 
    {
        string guid = Internal_GenerateUInt64Guid().ToString();
        if(guid.Length > 18) { guid = guid.Substring(0, 18); }

        return UInt64.Parse((int)appendedID + guid);
    }
}