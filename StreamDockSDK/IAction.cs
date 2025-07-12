namespace StreamDockSDK;

internal interface IAction
{
    void OnDidReceiveGlobalSettings(Dictionary<string, object> settings);

    void OnWillDisappear();

    void OnDidReceiveSettings(Dictionary<string, object> payloadSettings);

    void OnTitleParametersDidChange(Payload messagePayload);

    void OnKeyUp(Payload payload);

    void OnKeyDown(Payload payload);

    void OnDialUp(Payload payload);

    void OnDialDown(Payload payload);

    void OnDialRotate(Payload payload);

    void OnDeviceDidConnect(Message message);

    void OnDeviceDidDisconnect(Message message);

    void OnApplicationDidLaunch(Message message);

    void OnApplicationDidTerminate(Message message);

    void OnSystemDidWakeUp(Message message);

    void OnPropertyInspectorDidAppear(Message message);

    void OnPropertyInspectorDidDisappear(Message message);

    void OnSendToPlugin(Message message);
}