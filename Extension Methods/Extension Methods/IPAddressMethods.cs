using System;
using System.Net;

namespace Extension_Methods
{
    public static class IPAddressMethods
    {
        /// <summary> Checks whether an IP address may belong to LAN (192.168.0.0/16 or 10.0.0.0/24). </summary>
        public static bool IsLAN(this IPAddress addr)
        {
            if (addr == null) throw new ArgumentNullException(nameof(addr));
            byte[] bytes = addr.GetAddressBytes();
            return (bytes[0] == 192 && bytes[1] == 168) ||
                   (bytes[0] == 10);
        }

        public static uint AsUInt(this IPAddress thisAddr)
        {
            if (thisAddr == null) throw new ArgumentNullException(nameof(thisAddr));
            return (uint)IPAddress.HostToNetworkOrder(BitConverter.ToInt32(thisAddr.GetAddressBytes(), 0));
        }

        public static int AsInt(this IPAddress thisAddr)
        {
            if (thisAddr == null) throw new ArgumentNullException(nameof(thisAddr));
            return IPAddress.HostToNetworkOrder(BitConverter.ToInt32(thisAddr.GetAddressBytes(), 0));
        }

        public static bool Match(this IPAddress thisAddr, uint otherAddr, uint mask)
        {
            if (thisAddr == null) throw new ArgumentNullException(nameof(thisAddr));
            uint thisAsUInt = thisAddr.AsUInt();
            return (thisAsUInt & mask) == (otherAddr & mask);
        }

        public static IPAddress RangeMin(this IPAddress thisAddr, byte range)
        {
            if (thisAddr == null) throw new ArgumentNullException(nameof(thisAddr));
            if (range > 32) throw new ArgumentOutOfRangeException(nameof(range));
            int thisAsInt = thisAddr.AsInt();
            int mask = (int)NetMask(range);
            return new IPAddress(IPAddress.HostToNetworkOrder(thisAsInt & mask));
        }

        public static IPAddress RangeMax(this IPAddress thisAddr, byte range)
        {
            if (thisAddr == null) throw new ArgumentNullException(nameof(thisAddr));
            if (range > 32) throw new ArgumentOutOfRangeException(nameof(range));
            int thisAsInt = thisAddr.AsInt();
            int mask = (int)~NetMask(range);
            return new IPAddress((uint)IPAddress.HostToNetworkOrder(thisAsInt | mask));
        }

        public static uint NetMask(byte range)
        {
            if (range > 32) throw new ArgumentOutOfRangeException(nameof(range));

            return range == 0 ? 0 : 0xffffffff << (32 - range);
        }
    }
}
