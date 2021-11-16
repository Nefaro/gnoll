using System.IO;

namespace InstallerCore
{
    public abstract class Installable
    {
        public abstract void Install(string destination);
    }

    public class CopyInstallable : Installable
    {
        private byte[] _payload;

        public CopyInstallable(byte[] payload)
        {
            this._payload = payload;
        }

        public override void Install(string destination)
        {
            File.WriteAllBytes(destination, _payload);
        }

        public override string ToString()
        {
            return $"(CopyInstallable {this._payload.GetHashCode()})";
        }
    }
}
