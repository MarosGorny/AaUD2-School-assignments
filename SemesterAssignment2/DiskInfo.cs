using System;
using System.IO;
using System.Management;

public class DiskInfo
{
    public static int GetSectorSizeForPath(string path)
    {
        string driveLetter = Path.GetPathRoot(path).TrimEnd('\\');
        string driveQuery = $"SELECT * FROM Win32_DiskDrive WHERE DeviceID LIKE '%{driveLetter.Replace("\\", "\\\\")}%'";

        ManagementObjectSearcher searcher = new ManagementObjectSearcher(driveQuery);

        foreach (ManagementObject disk in searcher.Get())
        {
            return Convert.ToInt32(disk["BytesPerSector"]);
        }

        throw new Exception("Disk not found for the provided path.");
    }

}
