namespace PlazaConnectivityChecker.Settings
{
    public class PortscanSettings
    {
        public string PortOwner { get; protected set; }
        public int[] PortNumbers { get; protected set; }
        public string PortProtocol { get; protected set; }
        public string PortFunction { get; protected set; }
        public string PortRole { get; protected set; }
    }
}