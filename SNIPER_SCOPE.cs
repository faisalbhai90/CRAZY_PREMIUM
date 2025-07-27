using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using Gma.System.MouseKeyHook;

public class SNIPER_SCOPE
{
    public static async Task Run(dynamic mem, dynamic PID, dynamic ButtonLabel)
    {
        meowSniper memo = new meowSniper();
        List<long> addrs = new List<long>();
        string orig = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40 00 00 B0 3F";
        string patch = "FF FF FF FF 08 00 00 00 00 00 60 40 CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 06 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 13 40";
        string[] proc = { "HD-Player" };

        IKeyboardMouseEvents hook = Hook.GlobalEvents();
        Keys key = Keys.None;
        MouseButtons btn = MouseButtons.None;
        int wheel = 0;
        DateTime last = DateTime.MinValue;
        bool listen = false;
        const int delay = 74, cooldown = 200;

        void Scope(bool on)
        {
            if (addrs.Count == 0) return;
            foreach (var a in addrs) memo.AobReplace(a, on ? patch : orig);
        }

        async void Toggle()
        {
            if ((DateTime.Now - last).TotalMilliseconds < cooldown || addrs.Count == 0) return;
            last = DateTime.Now;
            Scope(true);
            await Task.Delay(delay);
            Scope(false);
        }

        hook.KeyDown += (a, b) =>
        {
            if (listen)
            {
                key = b.KeyCode; btn = MouseButtons.None; wheel = 0;
                PID.Text = $"Key: {key}"; ButtonLabel.Text = $", {key}"; listen = false;
            }
            else if (b.KeyCode == key) Toggle();
        };

        hook.MouseDown += (a, b) =>
        {
            if (listen)
            {
                btn = b.Button; key = Keys.None; wheel = 0;
                PID.Text = $"Mouse: {btn}"; ButtonLabel.Text = $", {btn}"; listen = false;
            }
            else if (b.Button == btn) Toggle();
        };

        hook.MouseWheel += (a, b) =>
        {
            if (listen)
            {
                wheel = b.Delta > 0 ? 1 : -1; key = Keys.None; btn = MouseButtons.None;
                PID.Text = wheel == 1 ? "Wheel UP" : "Wheel DOWN";
                ButtonLabel.Text = wheel == 1 ? "Wheel UP" : "Wheel DOWN";
                listen = false;
            }
            else if ((wheel == 1 && b.Delta > 0) || (wheel == -1 && b.Delta < 0)) Toggle();
        };

        // Start Process
        if (!memo.SetProcess(proc)) return;
        var r = await memo.AoBScan(orig);
        addrs = r.ToList();
        PID.Text = addrs.Count > 0 ? "Sniper Scope Load Success" : "Sniper Scope Failed";
        if (addrs.Count == 0) MessageBox.Show("Sniper Scope Failed X");
    }
}
