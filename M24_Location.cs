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

            var result = await mem.AoBScan("19 00 00 00 69 00 6e 00 67 00 61 00 6d 00 65 00 2f 00 70 00 69 00 63 00 6b 00 75 00 70 00 2f 00 70 00 69 00 63 00 6b 00 75 00 70 00 5f 00 62 00 6d 00 39 00 34 00 00 00").ConfigureAwait(false);

            if (result is IEnumerable<long> addresses)
            {
                var addressList = addresses.ToList();

                if (addressList.Count > 0 && addressList.Count < 2)
                {
                    foreach (var num in addressList)
                    {
                        mem.WriteMemory(num.ToString("X"), "bytes", "19 00 00 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 2f 00 76 00 66 00 78 00 5f 00 69 00 6e 00 67 00 61 00 6d 00 65 00 5f 00 6c 00 61 00 73 00 65 00 72 00 00 00 00 00", string.Empty, null);
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
