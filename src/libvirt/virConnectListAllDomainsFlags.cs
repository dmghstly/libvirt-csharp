using System;

namespace libvirt
{
    [Flags]
    public enum virConnectListAllDomainsFlags : uint
    {
        VIR_CONNECT_LIST_DOMAINS_ACTIVE 	= 	1,
        VIR_CONNECT_LIST_DOMAINS_INACTIVE 	= 	2,
        VIR_CONNECT_LIST_DOMAINS_PERSISTENT 	= 	4,
        VIR_CONNECT_LIST_DOMAINS_TRANSIENT 	= 	8,
        VIR_CONNECT_LIST_DOMAINS_RUNNING 	= 	16,
        VIR_CONNECT_LIST_DOMAINS_PAUSED 	= 	32,
        VIR_CONNECT_LIST_DOMAINS_SHUTOFF 	= 	64,
        VIR_CONNECT_LIST_DOMAINS_OTHER 	= 	128,
        VIR_CONNECT_LIST_DOMAINS_MANAGEDSAVE 	= 	256,
        VIR_CONNECT_LIST_DOMAINS_NO_MANAGEDSAVE 	= 	512,
        VIR_CONNECT_LIST_DOMAINS_AUTOSTART 	= 	1024,
        VIR_CONNECT_LIST_DOMAINS_NO_AUTOSTART 	= 	2048,
        VIR_CONNECT_LIST_DOMAINS_HAS_SNAPSHOT 	= 	4096,
        VIR_CONNECT_LIST_DOMAINS_NO_SNAPSHOT 	= 	8192,
        VIR_CONNECT_LIST_DOMAINS_HAS_CHECKPOINT 	= 	16384,
        VIR_CONNECT_LIST_DOMAINS_NO_CHECKPOINT 	= 	32768
    }
}