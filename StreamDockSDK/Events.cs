namespace StreamDockSDK;

internal static class Events
{
    public static class PluginEvents
    {
        public const string DidReceiveGlobalSettings = "didReceiveGlobalSettings";
        public const string WillAppear = "willAppear";
        public const string WillDisappear = "willDisappear";
        public const string DidReceiveSettings = "didReceiveSettings";
        public const string TitleParametersDidChange = "titleParametersDidChange";
        public const string RegisterPlugin = "registerPlugin";
    }

    public static class ContextEvents
    {
        public const string KeyDown = "keyDown";
        public const string KeyUp = "keyUp";
        public const string DialDown = "dialDown";
        public const string DialUp = "dialUp";
        public const string DialRotate = "dialRotate";
    }

    public static class GlobalEvents
    {
        public const string DeviceDidConnect = "deviceDidConnect";
        public const string DeviceDidDisconnect = "deviceDidDisconnect";
        public const string ApplicationDidLaunch = "applicationDidLaunch";
        public const string ApplicationDidTerminate = "applicationDidTerminate";
        public const string SystemDidWakeUp = "systemDidWakeUp";
    }

    public static class InteractionEvents
    {
        public const string PropertyInspectorDidAppear = "propertyInspectorDidAppear";
        public const string PropertyInspectorDidDisappear = "propertyInspectorDidDisappear";
        public const string SendToPlugin = "sendToPlugin";
    }
}