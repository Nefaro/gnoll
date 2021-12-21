using System;
using System.IO;
using NVcdiff;

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

    // Patch must be generated like this: xdelta3 -S -s Gnomoria.exe Gnoll.exe Gnoll.xdelta
    // (until we get a better VCDIFF library)
    public class PatchInstallable : Installable
    {
        private byte[] _patchData;
        private string _srcPath;
        private string _patchVersion;
        public string PatchVersion { get => _patchVersion; }

        public PatchInstallable(byte[] payload, string srcPath, string patchVersion)
        {
            _patchData = payload;
            _srcPath = srcPath;
            _patchVersion = patchVersion;
        }

        public override void Install(string destination)
        {
            using (var original = File.OpenRead(_srcPath))
            using (var patch = new MemoryStream(_patchData))
            using (var target = File.Open(destination, FileMode.Create, FileAccess.ReadWrite))
            {
                VcdiffDecoder.Decode(original, patch, target);
            }
        }

        public override string ToString()
        {
            return $"(PatchInstallable {this._patchData.GetHashCode()}+{_srcPath})";
        }
    }
}
