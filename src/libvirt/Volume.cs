using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libvirt
{
    public class Volume
    {
        private readonly IntPtr _ptrVol;
        private readonly IntPtr _ptrPool;
        private readonly uint _flags;

        internal Volume(IntPtr ptrPool, IntPtr ptrVol, uint flags)
        {
            if (ptrPool == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptrPool));
            }

            if (ptrVol == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(ptrVol));
            }

            _ptrVol = ptrVol;
            _ptrPool = ptrPool;
            _flags = flags;
        }
    }
}
