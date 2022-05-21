using SpeechClient;

namespace SystemApi.Services
{
    public class SysService
    {
        private readonly IConfiguration configuration;
        private readonly Speaker speaker;

        public SysService(IConfiguration configuration)
        {
            this.configuration = configuration;
            speaker = new Speaker();
        }

        public async Task Poweroff()
        {
            var t = new Thread(PwrThread);
            await speaker.SpeakText("Powering off SceneSounder..");
            await Task.Delay(2000);
            t.Start();
        }
        private void PwrThread()
        {
            string command = "sudo systemctl poweroff";
            ExecuteProcess(command);
        }
        public async Task Reboot()
        {
            var t = new Thread(RebootThread);
            await speaker.SpeakText("Rebooting SceneSounder..");
            await Task.Delay(2000);
            t.Start();
        }
        private void RebootThread()
        {
            string command = "sudo systemctl reboot";
            ExecuteProcess(command);
        }

        internal async Task SetAudioVolume(int vol)
        {
            string command = $"pactl set-sink-volume 0 0x{vol * 100}";
            await ExecuteProcessAsync(command);
        }

        private static string ExecuteProcess(string command)
        {
            string result = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }
            return result;
        }
        private static async Task<string> ExecuteProcessAsync(string command)
        {
            string result = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                await proc.WaitForExitAsync();
            }
            return result;
        }

    }
}
