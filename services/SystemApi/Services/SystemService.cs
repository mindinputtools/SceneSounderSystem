using SpeechClient;

namespace SystemApi.Services
{
    public class SystemService
    {
        private readonly IConfiguration configuration;
        private readonly Speaker speaker;

        public SystemService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            speaker = new Speaker(new Uri(configuration["SpeechApiAddress"]), httpClientFactory);
        }

        public void Poweroff()
        {
            var t = new Thread(PwrThread);
            speaker.SpeakText("Powering off SceneSounder..");
            Task.Delay(2000);
            t.Start();
        }
        private void PwrThread()
        {
            string command = "sudo systemctl poweroff";
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
//            return result;
        }
    }
}
