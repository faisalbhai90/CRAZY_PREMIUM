using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

public class AWM_Patch
{
    public static async Task Run(dynamic mem, dynamic PID)
    {
        try
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.Beep(1000, 100); // শুরুতে beep
            PID.Text = "⏳ Searching for AWM value...";

            Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
            mem.OpenProcess(proc);

            var result = await mem.AoBScan("0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 00 00 00 3F 00 00 80 3E 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00").ConfigureAwait(false);

            if (result is IEnumerable<long> addresses)
            {
                var addressList = addresses.ToList();

                if (addressList.Count > 0 && addressList.Count < 2)
                {
                    foreach (var num in addressList)
                    {
                        mem.WriteMemory(num.ToString("X"), "bytes", "0A D7 A3 3D 00 00 00 00 00 00 5C 43 00 00 90 42 00 00 B4 42 96 00 00 00 00 00 00 00 EC 51 B8 3D 8F C2 F5 3C 00 00 00 00 04 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 0A D7 23 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 00 00 80 3F 00 00 80 3F 00 00", string.Empty, null);
                    }

                    stopwatch.Stop();
                    PID.Text = $"✅ AWM Fast Switch: ON (⏱ {stopwatch.Elapsed.TotalSeconds:F2}s)";
                }
                else
                {
                    PID.Text = "❌ AWM Value Not Found or Too Many Results.";
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("❌ Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
