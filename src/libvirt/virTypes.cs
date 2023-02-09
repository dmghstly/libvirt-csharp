using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libvirt
{
    public class virTypes
    {
        /// <summary>
        /// Flags for storage pool building
        /// </summary>
        public enum StoragePoolBuildFlags
        {
            /// <summary>
            /// Regular build from scratch.
            /// </summary>
            VIR_STORAGE_POOL_BUILD_NEW = 0,
            /// <summary>
            /// Repair / reinitialize.
            /// </summary>
            VIR_STORAGE_POOL_BUILD_REPAIR = 1,
            /// <summary>
            /// Extend existing pool.
            /// </summary>
            VIR_STORAGE_POOL_BUILD_RESIZE = 2
        }

        ///<summary>
        /// Flags for storage pool deletion
        ///</summary>
        public enum StoragePoolDeleteFlags
        {
            /// <summary>
            /// Delete metadata only (fast).
            /// </summary>
            VIR_STORAGE_POOL_DELETE_NORMAL = 0,
            /// <summary>
            /// Clear all data to zeros (slow).
            /// </summary>
            VIR_STORAGE_POOL_DELETE_ZEROED = 1
        }

        /// <summary>
        /// Structure to handle storage pool informations
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct StoragePoolInfo
        {
            /// <summary>
            /// virStoragePoolState flags
            /// </summary>
            [MarshalAs(UnmanagedType.I4)]
            public StoragePoolState state;
            /// <summary>
            /// Logical size bytes
            /// </summary>
            [MarshalAs(UnmanagedType.U8)]
            public ulong capacity;
            /// <summary>
            /// Current allocation bytes.
            /// </summary>
            [MarshalAs(UnmanagedType.U8)]
            public ulong allocation;
            /// <summary>
            /// Remaining free space bytes.
            /// </summary>
            [MarshalAs(UnmanagedType.U8)]
            public ulong available;
        }

        /// <summary>
        /// States of storage pool
        /// </summary>
        public enum StoragePoolState
        {
            /// <summary>
            /// Not running.
            /// </summary>
            VIR_STORAGE_POOL_INACTIVE = 0,
            /// <summary>
            /// Initializing pool, not available.
            /// </summary>
            VIR_STORAGE_POOL_BUILDING = 1,
            /// <summary>
            /// Running normally.
            /// </summary>
            VIR_STORAGE_POOL_RUNNING = 2,
            /// <summary>
            /// Running degraded.
            /// </summary>
            VIR_STORAGE_POOL_DEGRADED = 3,
        }

        /// <summary>
        /// Flasg for XML domain rendering
        /// </summary>
        [Flags]
        public enum DomainXMLFlags
        {
            /// <summary>
            /// Dump security sensitive information too.
            /// </summary>
            VIR_DOMAIN_XML_SECURE = 1,
            /// <summary>
            /// Dump inactive domain information.
            /// </summary>
            VIR_DOMAIN_XML_INACTIVE = 2
        }

        ///<summary>
        /// Structure to handle volume informations
        ///</summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct StorageVolInfo
        {
            /// <summary>
            /// virStorageVolType flags.
            /// </summary>
            [MarshalAs(UnmanagedType.I4)]
            public StorageVolType type;
            /// <summary>
            /// Logical size bytes.
            /// </summary>
            [MarshalAs(UnmanagedType.U8)]
            public ulong capacity;
            /// <summary>
            /// Current allocation bytes.
            /// </summary>
            [MarshalAs(UnmanagedType.U8)]
            public ulong allocation;
        }

        ///<summary>
        /// Types of storage volume
        ///</summary>
        public enum StorageVolType
        {
            /// <summary>
            /// Regular file based volumes.
            /// </summary>
            VIR_STORAGE_VOL_FILE = 0,
            /// <summary>
            /// Block based volumes.
            /// </summary>
            VIR_STORAGE_VOL_BLOCK = 1
        }
    }
}
