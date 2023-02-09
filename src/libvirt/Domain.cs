using System;

namespace libvirt
{
    /// <summary>
    /// Represents a Domain object
    /// </summary>
    public class Domain : LibvirtObject
    {
        private readonly IntPtr _ptrConnect;
        private readonly IntPtr _ptrDomain;

        internal Domain(IntPtr ptrConnect, IntPtr ptrDomain)
        {
            if (ptrConnect == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptrConnect));
            }

            if (ptrDomain == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptrDomain));
            }

            _ptrConnect = ptrConnect;
            _ptrDomain = ptrDomain;
        }

        public void Destroy() => ThrowExceptionOnError(Libvirt.virDomainDestroy(_ptrDomain));

        public void Reboot() => ThrowExceptionOnError(Libvirt.virDomainReboot(_ptrDomain));

        public void Shutdown() => ThrowExceptionOnError(Libvirt.virDomainShutdown(_ptrDomain));

        public void Suspend() => ThrowExceptionOnError(Libvirt.virDomainSuspend(_ptrDomain));

        public void Resume() => ThrowExceptionOnError(Libvirt.virDomainResume(_ptrDomain));

        public void Save(string file) => ThrowExceptionOnError(Libvirt.virDomainSave(_ptrDomain, file));

        public int Id => GetInt32(() => Libvirt.virDomainGetID(_ptrDomain));

        public string Name => GetString(() => Libvirt.virDomainGetName(_ptrDomain));

        public string UUID => GetUUID(uuid => Libvirt.virDomainGetUUIDString(_ptrDomain, uuid));

        public string OSType => GetString(() => Libvirt.virDomainGetOSType(_ptrDomain));

        public string Xml => GetString(() => Libvirt.virDomainGetXMLDesc(_ptrDomain));

        // attach some defined devices to your domain
        public void AttachDeviceFlags(string xml, uint flags)
        {
            if (_ptrDomain == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(_ptrDomain));
            }

            var result = Libvirt.virDomainAttachDeviceFlags(_ptrDomain, xml, flags);

            if (result == -1)
            {
                throw new Exception("Device flags cannot be attached");
            }
        }

        public void Create()
        {
            if (_ptrDomain == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(_ptrDomain));
            }

            int result = Libvirt.virDomainCreate(_ptrDomain);

            if (result == -1)
            {
                throw new Exception("Domain cannot be created");
            }
        }

        public virDomainInfo Info 
        {
            get
            {
                EnsureObjectIsNotDisposed();

                virDomainInfo domainInfo = new virDomainInfo();

                var result = Libvirt.virDomainGetInfo(_ptrDomain, domainInfo);

                ThrowExceptionOnError(result);

                return domainInfo;
            }
        }

        protected override void DisposeInternal()
        {
            Libvirt.virDomainFree(_ptrDomain);
        }
    }
}