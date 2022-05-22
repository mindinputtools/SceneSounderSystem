namespace ObjDetectYOLO
{
    public static class State
    {
        public static bool AutoSpeakerRunning { get; set; } = false;
        public static Thread AutoSpeakerThread { get; internal set; }
        public static AutoResetEvent SpeechCompleted { get; set; } = new AutoResetEvent(false);
    }
}
