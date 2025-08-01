using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

public class AMMO_Location
{
    public static async Task Run(dynamic mem, dynamic PID)
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Beep(100, 200);
            PID.Text = "ᴀᴘᴘʟʏɪɴɢ AMMO ʟᴏᴄᴀᴛꞮᴏɴ";

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            mem.OpenProcess(proc);

            var resultObj = await mem.AoBScan("22 00 00 00 69 00 6E 00 67 00 61 00 6D 00 65 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 2F 00 61 00 6D 00 6D 00 6F 00 2F 00 70 00 69 00 63 00 6B 00 75 00 70 00 5F 00 61 00 6D 00");

            if (resultObj is IEnumerable<long> result)
            {
                var resultList = result.ToList();

                if (resultList.Count != 0 && resultList.Count < 2)
                {
                    foreach (long num in resultList)
                    {
                        mem.WriteMemory(num.ToString("X"), "bytes",
                            "1C 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2F 00 76 00 66 00 78 00 5F 00 69 00 6E 00 67 00 61 00 6D 00 65 00 5F 00 6C 00 61 00 73 00 65 00 72 00 5F 00 72 00 65 00 64 00",
                            string.Empty, null);
                    }

                    stopwatch.Stop();
                    double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                    Console.Beep(200, 300);
                    PID.Text = $"Ammo-ʟᴏᴄᴀᴛꞮᴏɴ=ᴏɴ,ᴛꞮᴍᴇ: {elapsedSeconds:F2} Seconds";
                }
                else
                {
                    PID.Text = "❌ Ammo value not found or too many results.";
                    if (resultList.Count > 2)
                    {
                        MessageBox.Show("ᴛʜꞮꜱ ᴄᴏᴅᴇ ɴᴏᴛ ꜱᴀꜰᴇ.", "ᴇƦƦᴏƦ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                PID.Text = "❌ Scan failed. Invalid result.";
            }
        }
        catch (Exception ex)
        {
            PID.Text = $"Patch Failed: {ex.Message}";
        }
    }
}
