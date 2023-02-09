using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libvirt
{
    public class Pool : LibvirtObject
    {
        private readonly IntPtr _ptrConnect;
        private readonly IntPtr _ptrPool;
        private readonly uint _flags;

        internal Pool(IntPtr ptrConnect, IntPtr ptrPool, uint flags)
        {
            if (ptrConnect == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptrConnect));
            }

            if (ptrPool == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptrPool));
            }

            _ptrConnect = ptrConnect;
            _ptrPool = ptrPool;
            _flags = flags;
        }

        // Methods
        // autostarts pool
        public void SetAutoStart(int autostart)
        {
            if (_ptrPool == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(_ptrPool));
            }

            int start = Libvirt.virStoragePoolSetAutostart(_ptrPool, autostart);

            if (start == -1)
            {
                throw new Exception("Failed to set storage pool to autostart");
            }
        }

        // creates pool
        public void Create(int autostart)
        {
            if (_ptrPool == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(_ptrPool));
            }

            int start = Libvirt.virStoragePoolCreate(_ptrPool, _flags);

            if (start == -1)
            {
                throw new Exception("Storage pool cannot be created");
            }
        }

        // creates volume
        public Volume CreateXMLVolume(string xml, uint flags)
        {
            IntPtr result = Libvirt.virStorageVolCreateXML(_ptrPool, xml, flags);

            ThrowExceptionOnError(result);

            return new Volume(_ptrPool, result, flags);
        }

        // Properties
        // returning the name of a Pool
        public string GetName()
        {
            return Libvirt.virStoragePoolGetName(_ptrPool);
        }

        // returning the id of a Pool
        public string GetId()
        {
            return Libvirt.virStoragePoolGetUUIDString(_ptrPool);
        }
    }
}
