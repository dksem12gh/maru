using Doozy.Runtime.Signals;

public class SignalController
{    
    public void SendSignal(string streamCategory = null,string streamName = null)
    {
        SignalStream.Get(streamCategory, streamName).SendSignal();
    }
    public void SendSignal_GameScene(string sceneName)
    {
        SendSignal("MainFlow", sceneName);
    }
    public void Restart_GameScene(string sceneName)
    {
        SendSignal("RestartGame", sceneName);
    }
}
