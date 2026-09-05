using System;

public interface IFinishRoad
{
    public event Action<IFinishRoad> OnConnected;

    public void Connect();
    public void Disconnect();
}