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

            var result = await mem.AoBScan("CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F 00 00").ConfigureAwait(false);

            if (result is IEnumerable<long> addresses)
            {
                var addressList = addresses.ToList();

                if (addressList.Count > 0 && addressList.Count < 2)
                {
                    foreach (var num in addressList)
                    {
                        mem.WriteMemory(num.ToString("X"), "bytes", "CC 3D 06 00 00 00 00 00 80 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 33 13 40 00 00 B0 3F 00 00", string.Empty, null);
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
