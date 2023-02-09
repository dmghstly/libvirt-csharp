using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static libvirt.virTypes;

namespace libvirt
{
    public static class Libvirt
    {
        public const string Name = "libvirt";

        static Libvirt()
        {
            NativeLibrary.SetDllImportResolver(typeof(Libvirt).Assembly, ImportResolver);
        }

        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr handle = IntPtr.Zero;
            
            if (libraryName == Libvirt.Name)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    NativeLibrary.TryLoad("libvirt.so.0", assembly, searchPath, out handle);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    NativeLibrary.TryLoad("libvirt-0.dll", assembly, searchPath, out handle);
                }
            }

            return handle;
        }

        public const int VIR_UUID_BUFLEN = 36;

        public static Version Version
        {
            get
            {
                LibvirtHelper.ThrowExceptionOnError(virGetVersion(out ulong libVer, null, out _));

                int release = (int) (libVer % 1000);
                int minor = (int) ((libVer % 1000000) / 1000);
                int major = (int) (libVer / 1000000);

                return new Version(major, minor, release);
            }
        }

        #region Library

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virGetVersion")]
        public static extern int virGetVersion([Out] out ulong libVer, [In] string type, [Out] out ulong typeVer);

        #endregion

        #region Connect

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectOpen")]
        public static extern IntPtr virConnectOpen(string name);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectOpenReadOnly")]
        public static extern IntPtr virConnectOpenReadOnly(string name);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectClose")]
        public static extern int virConnectClose(IntPtr conn);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectGetCapabilities")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string virConnectGetCapabilities(IntPtr conn);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectGetHostname")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string virConnectGetHostname(IntPtr conn);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectGetType")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StaticStringMarshaler))]
        public static extern string virConnectGetType(IntPtr conn);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virConnectListAllDomains")]
        public static extern int virConnectListAllDomains(IntPtr conn, [Out] out IntPtr domains, virConnectListAllDomainsFlags flags);

        #endregion

        #region Pool
        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolBuild")]
        public static extern int virStoragePoolBuild(IntPtr pool, StoragePoolBuildFlags flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolCreate")]
        public static extern int virStoragePoolCreate(IntPtr pool, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolCreateXML")]
        public static extern IntPtr virStoragePoolCreateXML(IntPtr conn, string xmlDesc, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolDefineXML")]
        public static extern IntPtr virStoragePoolDefineXML(IntPtr conn, string xml, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolDelete")]
        public static extern int virStoragePoolDelete(IntPtr pool, StoragePoolDeleteFlags flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolDestroy")]
        public static extern int virStoragePoolDestroy(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolFree")]
        public static extern int virStoragePoolFree(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetAutostart")]
        public static extern int virStoragePoolGetAutostart(IntPtr pool, out int autotart);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetConnect")]
        public static extern IntPtr virStoragePoolGetConnect(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetInfo")]
        public static extern int virStoragePoolGetInfo(IntPtr pool, ref StoragePoolInfo info);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetName")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringWithoutNativeCleanUpMarshaler))]
        public static extern string virStoragePoolGetName(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetUUID")]
        public static extern int virStoragePoolGetUUID(IntPtr pool, [Out] char[] uuid);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetUUIDString")]
        private static extern int virStoragePoolGetUUIDString(IntPtr pool, [Out] char[] uuid);

        public static string virStoragePoolGetUUIDString(IntPtr pool)
        {
            char[] uuidArray = new char[36];
            virStoragePoolGetUUIDString(pool, uuidArray);
            return new string(uuidArray);
        }

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolGetXMLDesc")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringWithoutNativeCleanUpMarshaler))]
        public static extern string virStoragePoolGetXMLDesc(IntPtr pool, DomainXMLFlags flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolIsActive")]
        public static extern int virStoragePoolIsActive(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolIsPersistent")]
        public static extern int virStoragePoolIsPersistent(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolListVolumes")]
        private static extern int virStoragePoolListVolumes(IntPtr pool, IntPtr names, int maxnames);

        public static int virStoragePoolListVolumes(IntPtr pool, ref string[] names, int maxnames)
        {
            IntPtr namesPtr = Marshal.AllocHGlobal(1024);
            int count = virStoragePoolListVolumes(pool, namesPtr, maxnames);
            if (count > 0)
                names = MarshalHelper.ptrToStringArray(namesPtr, count);
            Marshal.FreeHGlobal(namesPtr);
            return count;
        }

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolLookupByName")]
        public static extern IntPtr virStoragePoolLookupByName(IntPtr conn, string name);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolLookupByUUID")]
        public static extern IntPtr virStoragePoolLookupByUUID(IntPtr conn, char[] uuid);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolLookupByUUIDString")]
        public static extern IntPtr virStoragePoolLookupByUUIDString(IntPtr conn, string uuidstr);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolLookupByVolume")]
        public static extern IntPtr virStoragePoolLookupByVolume(IntPtr vol);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolNumOfVolumes")]
        public static extern int virStoragePoolNumOfVolumes(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolRef")]
        public static extern int virStoragePoolRef(IntPtr pool);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolRefresh")]
        public static extern int virStoragePoolRefresh(IntPtr pool, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolSetAutostart")]
        public static extern int virStoragePoolSetAutostart(IntPtr pool, int autostart);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStoragePoolUndefine")]
        public static extern int virStoragePoolUndefine(IntPtr pool);

        #endregion

        #region Volume
        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolCreateXML")]
        public static extern IntPtr virStorageVolCreateXML(IntPtr pool, string xmldesc, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolCreateXMLFrom")]
        public static extern IntPtr virStorageVolCreateXMLFrom(IntPtr pool, string xmldesc, IntPtr clonevol, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolDelete")]
        public static extern int virStorageVolDelete(IntPtr vol, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolFree")]
        public static extern int virStorageVolFree(IntPtr vol);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolGetConnect")]
        public static extern IntPtr virStorageVolGetConnect(IntPtr vol);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolGetInfo")]
        public static extern int virStorageVolGetInfo(IntPtr vol, ref StorageVolInfo info);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolGetKey")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringWithoutNativeCleanUpMarshaler))]
        public static extern string virStorageVolGetKey(IntPtr vol);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolGetName")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringWithoutNativeCleanUpMarshaler))]
        public static extern string virStorageVolGetName(IntPtr vol);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolGetPath")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringWithoutNativeCleanUpMarshaler))]
        public static extern string virStorageVolGetPath(IntPtr vol);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolGetXMLDesc")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringWithoutNativeCleanUpMarshaler))]
        public static extern string virStorageVolGetXMLDesc(IntPtr vol, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolLookupByKey")]
        public static extern IntPtr virStorageVolLookupByKey(IntPtr conn, string key);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolLookupByName")]
        public static extern IntPtr virStorageVolLookupByName(IntPtr pool, string name);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolLookupByPath")]
        public static extern IntPtr virStorageVolLookupByPath(IntPtr conn, string path);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virStorageVolRef")]
        public static extern int virStorageVolRef(IntPtr vol);
        #endregion

        #region Domain
        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainCreate")]
        public static extern int virDomainCreate(IntPtr domain);


        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainDefineXML")]
        public static extern IntPtr virDomainDefineXML(IntPtr conn, string xml);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainGetID")]
        public static extern int virDomainGetID(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainGetName")]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StaticStringMarshaler))]
        public static extern string virDomainGetName(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainGetUUIDString")]
        public static extern int virDomainGetUUIDString(IntPtr domain, [Out] char[] uuid);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainGetOSType")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string virDomainGetOSType(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainGetInfo")]
        public static extern int virDomainGetInfo(IntPtr domain, [Out] virDomainInfo info);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainGetXMLDesc")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string virDomainGetXMLDesc(IntPtr domain, int flags = 0);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainCreateXML")]
        public static extern IntPtr virDomainCreateXML(IntPtr conn, string xmlDesc, uint flags = 0);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainDestroy")]
        public static extern int virDomainDestroy(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainFree")]
        public static extern int virDomainFree(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainShutdown")]
        public static extern int virDomainShutdown(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainSuspend")]
        public static extern int virDomainSuspend(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainResume")]
        public static extern int virDomainResume(IntPtr domain);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainReboot")]
        public static extern int virDomainReboot(IntPtr domain, uint flags = 0);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainSave")]
        public static extern int virDomainSave(IntPtr domain, string to);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainRestore")]
        public static extern int virDomainRestore(IntPtr conn, string from);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainAttachDeviceFlags")]
        public static extern int virDomainAttachDeviceFlags(IntPtr domain, string xml, uint flags);

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virDomainDetachDeviceFlags")]
        public static extern int virDomainDetachDeviceFlags(IntPtr domain, string xml, uint flags);

        #endregion

        #region Error

        [DllImport(Libvirt.Name, CallingConvention = CallingConvention.Cdecl, EntryPoint = "virGetLastError")]
        public static extern IntPtr virGetLastError();

        #endregion
    }

    /// <summary>
    /// Marshals a char* string without freeing the memory
    /// </summary>
    /// <see cref="https://stackoverflow.com/questions/17667611/how-to-marshal-to-ansi-string-via-attribute"/>
    internal class StaticStringMarshaler : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException(nameof(cookie));
            }

            var result = new StaticStringMarshaler();

            return result;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj) =>  Marshal.StringToHGlobalAnsi((string) ManagedObj);

        public object MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringAnsi(pNativeData);

        public void CleanUpManagedData(object ManagedObj) { }

        public void CleanUpNativeData(IntPtr pNativeData) { }

        public int GetNativeDataSize() => -1;
    }
}