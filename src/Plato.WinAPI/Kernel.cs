// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Runtime.InteropServices;

namespace Plato.WinAPI
{
    /// <summary>
    ///
    /// </summary>
    public static class Kernel
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        public const int ERROR_SUCCESS = 0;
        /// <summary>
        /// No more data is available.
        /// </summary>
        public const int ERROR_NO_MORE_ITEMS = 259;
        /// <summary>
        /// Incorrect function.
        /// </summary>
        public const int ERROR_INVALID_FUNCTION = 1;
        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        public const int ERROR_FILE_NOT_FOUND = 2;
        /// <summary>
        /// The system cannot find the path specified.
        /// </summary>
        public const int ERROR_PATH_NOT_FOUND = 3;
        /// <summary>
        /// The system cannot open the file.
        /// </summary>
        public const int ERROR_TOO_MANY_OPEN_FILES = 4;
        /// <summary>
        /// Access is denied.
        /// </summary>
        public const int ERROR_ACCESS_DENIED = 5;
        /// <summary>
        /// The handle is invalid.
        /// </summary>
        public const int ERROR_INVALID_HANDLE = 6;
        /// <summary>
        /// The storage control blocks were destroyed.
        /// </summary>
        public const int ERROR_ARENA_TRASHED = 7;
        /// <summary>
        /// Not enough storage is available to process this command.
        /// </summary>
        public const int ERROR_NOT_ENOUGH_MEMORY = 8;
        /// <summary>
        /// The storage control block address is invalid.
        /// </summary>
        public const int ERROR_INVALID_BLOCK = 9;
        /// <summary>
        /// The environment is incorrect.
        /// </summary>
        public const int ERROR_BAD_ENVIRONMENT = 10;
        /// <summary>
        /// An attempt was made to load a program with an incorrect format.
        /// </summary>
        public const int ERROR_BAD_FORMAT = 11;
        /// <summary>
        /// The access code is invalid.
        /// </summary>
        public const int ERROR_INVALID_ACCESS = 12;
        /// <summary>
        /// The data is invalid.
        /// </summary>
        public const int ERROR_INVALID_DATA = 13;
        /// <summary>
        /// Not enough storage is available to complete this operation.
        /// </summary>
        public const int ERROR_OUTOFMEMORY = 14;
        /// <summary>
        /// The system cannot find the drive specified.
        /// </summary>
        public const int ERROR_INVALID_DRIVE = 15;
        /// <summary>
        /// The directory cannot be removed.
        /// </summary>
        public const int ERROR_CURRENT_DIRECTORY = 16;
        /// <summary>
        /// The system cannot move the file to a different disk drive.
        /// </summary>
        public const int ERROR_NOT_SAME_DEVICE = 17;
        /// <summary>
        /// There are no more files.
        /// </summary>
        public const int ERROR_NO_MORE_FILES = 18;
        /// <summary>
        /// The media is write protected.
        /// </summary>
        public const int ERROR_WRITE_PROTECT = 19;
        /// <summary>
        /// The system cannot find the device specified.
        /// </summary>
        public const int ERROR_BAD_UNIT = 20;
        /// <summary>
        /// The device is not ready.
        /// </summary>
        public const int ERROR_NOT_READY = 21;
        /// <summary>
        /// The device does not recognize the command.
        /// </summary>
        public const int ERROR_BAD_COMMAND = 22;
        /// <summary>
        /// Data error (cyclic redundancy check).
        /// </summary>
        public const int ERROR_CRC = 23;
        /// <summary>
        /// The program issued a command but the command length is incorrect.
        /// </summary>
        public const int ERROR_BAD_LENGTH = 24;
        /// <summary>
        /// The drive cannot locate a specific area or track on the disk.
        /// </summary>
        public const int ERROR_SEEK = 25;
        /// <summary>
        /// The specified disk or diskette cannot be accessed.
        /// </summary>
        public const int ERROR_NOT_DOS_DISK = 26;
        /// <summary>
        /// The drive cannot find the sector requested.
        /// </summary>
        public const int ERROR_SECTOR_NOT_FOUND = 27;
        /// <summary>
        /// The printer is out of paper.
        /// </summary>
        public const int ERROR_OUT_OF_PAPER = 28;
        /// <summary>
        /// The system cannot write to the specified device.
        /// </summary>
        public const int ERROR_WRITE_FAULT = 29;
        /// <summary>
        /// The system cannot read from the specified device.
        /// </summary>
        public const int ERROR_READ_FAULT = 30;
        /// <summary>
        /// A device attached to the system is not functioning.
        /// </summary>
        public const int ERROR_GEN_FAILURE = 31;
        /// <summary>
        /// The specified network password is not correct.
        /// </summary>
        public const int ERROR_INVALID_PASSWORD = 86;
        /// <summary>
        /// The filename, directory name, or volume label syntax is incorrect.
        /// </summary>
        public const int ERROR_INVALID_NAME = 123;
        /// <summary>
        /// The system call level is not correct.
        /// </summary>
        public const int ERROR_INVALID_LEVEL = 124;
        /// <summary>
        /// The disk has no volume label.
        /// </summary>
        public const int ERROR_NO_VOLUME_LABEL = 125;
        /// <summary>
        /// The specified module could not be found.
        /// </summary>
        public const int ERROR_MOD_NOT_FOUND = 126;
        /// <summary>
        /// The specified procedure could not be found.
        /// </summary>
        public const int ERROR_PROC_NOT_FOUND = 127;
        /// <summary>
        /// There are no child processes to wait for.
        /// </summary>
        public const int ERROR_WAIT_NO_CHILDREN = 128;
        /// <summary>
        /// A tape access reached a filemark.
        /// </summary>
        public const int ERROR_FILEMARK_DETECTED = 1101;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_RESPONSE_CODES_BASE = 9000;
        /// <summary>
        /// DNS server unable to interpret format.
        /// </summary>
        public const int DNS_ERROR_RCODE_FORMAT_ERROR = 9001;
        /// <summary>
        /// DNS server failure.
        /// </summary>
        public const int DNS_ERROR_RCODE_SERVER_FAILURE = 9002;
        /// <summary>
        /// DNS name does not exist.
        /// </summary>
        public const int DNS_ERROR_RCODE_NAME_ERROR = 9003;
        /// <summary>
        /// DNS request not supported by name server.
        /// </summary>
        public const int DNS_ERROR_RCODE_NOT_IMPLEMENTED = 9004;
        /// <summary>
        /// DNS operation refused.
        /// </summary>
        public const int DNS_ERROR_RCODE_REFUSED = 9005;
        /// <summary>
        /// DNS name that ought not exist, does exist.
        /// </summary>
        public const int DNS_ERROR_RCODE_YXDOMAIN = 9006;
        /// <summary>
        /// DNS RR set that ought not exist, does exist.
        /// </summary>
        public const int DNS_ERROR_RCODE_YXRRSET = 9007;
        /// <summary>
        /// DNS RR set that ought to exist, does not exist.
        /// </summary>
        public const int DNS_ERROR_RCODE_NXRRSET = 9008;
        /// <summary>
        /// DNS server not authoritative for zone.
        /// </summary>
        public const int DNS_ERROR_RCODE_NOTAUTH = 9009;
        /// <summary>
        /// DNS name in update or prereq is not in zone.
        /// </summary>
        public const int DNS_ERROR_RCODE_NOTZONE = 9010;
        /// <summary>
        /// DNS signature failed to verify.
        /// </summary>
        public const int DNS_ERROR_RCODE_BADSIG = 9016;
        /// <summary>
        /// DNS bad key.
        /// </summary>
        public const int DNS_ERROR_RCODE_BADKEY = 9017;
        /// <summary>
        /// DNS signature validity expired.
        /// </summary>
        public const int DNS_ERROR_RCODE_BADTIME = 9018;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_PACKET_FMT_BASE = 9500;
        /// <summary>
        /// No records found for given DNS query.
        /// </summary>
        public const int DNS_INFO_NO_RECORDS = 9501;
        /// <summary>
        /// Bad DNS packet.
        /// </summary>
        public const int DNS_ERROR_BAD_PACKET = 9502;
        /// <summary>
        /// No DNS packet.
        /// </summary>
        public const int DNS_ERROR_NO_PACKET = 9503;
        /// <summary>
        /// DNS error, check rcode.
        /// </summary>
        public const int DNS_ERROR_RCODE = 9504;
        /// <summary>
        /// Unsecured DNS packet.
        /// </summary>
        public const int DNS_ERROR_UNSECURE_PACKET = 9505;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_NO_MEMORY = ERROR_OUTOFMEMORY;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_INVALID_NAME = ERROR_INVALID_NAME;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_INVALID_DATA = ERROR_INVALID_DATA;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_GENERAL_API_BASE = 9550;
        /// <summary>
        /// Invalid DNS type.
        /// </summary>
        public const int DNS_ERROR_INVALID_TYPE = 9551;
        /// <summary>
        /// Invalid IP address.
        /// </summary>
        public const int DNS_ERROR_INVALID_IP_ADDRESS = 9552;
        /// <summary>
        /// Invalid property.
        /// </summary>
        public const int DNS_ERROR_INVALID_PROPERTY = 9553;
        /// <summary>
        /// Try DNS operation again later.
        /// </summary>
        public const int DNS_ERROR_TRY_AGAIN_LATER = 9554;
        /// <summary>
        /// Record for given name and type is not unique.
        /// </summary>
        public const int DNS_ERROR_NOT_UNIQUE = 9555;
        /// <summary>
        /// DNS name does not comply with RFC specifications.
        /// </summary>
        public const int DNS_ERROR_NON_RFC_NAME = 9556;
        /// <summary>
        /// DNS name is a fully-qualified DNS name.
        /// </summary>
        public const int DNS_STATUS_FQDN = 9557;
        /// <summary>
        /// DNS name is dotted (multi-label).
        /// </summary>
        public const int DNS_STATUS_DOTTED_NAME = 9558;
        /// <summary>
        /// DNS name is a single-part name.
        /// </summary>
        public const int DNS_STATUS_SINGLE_PART_NAME = 9559;
        /// <summary>
        /// DNS name contains an invalid character.
        /// </summary>
        public const int DNS_ERROR_INVALID_NAME_CHAR = 9560;
        /// <summary>
        /// DNS name is entirely numeric.
        /// </summary>
        public const int DNS_ERROR_NUMERIC_NAME = 9561;
        /// <summary>
        /// The operation requested is not permitted on a DNS root server.
        /// </summary>
        public const int DNS_ERROR_NOT_ALLOWED_ON_ROOT_SERVER = 9562;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_NOT_ALLOWED_UNDER_DELEGATION = 9563;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_CANNOT_FIND_ROOT_HINTS = 9564;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_INCONSISTENT_ROOT_HINTS = 9565;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_ZONE_BASE = 9600;
        /// <summary>
        /// DNS zone does not exist.
        /// </summary>
        public const int DNS_ERROR_ZONE_DOES_NOT_EXIST = 9601;
        /// <summary>
        /// DNS zone information not available.
        /// </summary>
        public const int DNS_ERROR_NO_ZONE_INFO = 9602;
        /// <summary>
        /// Invalid operation for DNS zone.
        /// </summary>
        public const int DNS_ERROR_INVALID_ZONE_OPERATION = 9603;
        /// <summary>
        /// Invalid DNS zone configuration.
        /// </summary>
        public const int DNS_ERROR_ZONE_CONFIGURATION_ERROR = 9604;
        /// <summary>
        /// DNS zone has no start of authority (SOA) record.
        /// </summary>
        public const int DNS_ERROR_ZONE_HAS_NO_SOA_RECORD = 9605;
        /// <summary>
        /// DNS zone has no Name Server (NS) record.
        /// </summary>
        public const int DNS_ERROR_ZONE_HAS_NO_NS_RECORDS = 9606;
        /// <summary>
        /// DNS zone is locked.
        /// </summary>
        public const int DNS_ERROR_ZONE_LOCKED = 9607;
        /// <summary>
        /// DNS zone creation failed.
        /// </summary>
        public const int DNS_ERROR_ZONE_CREATION_FAILED = 9608;
        /// <summary>
        /// DNS zone already exists.
        /// </summary>
        public const int DNS_ERROR_ZONE_ALREADY_EXISTS = 9609;
        /// <summary>
        /// DNS automatic zone already exists.
        /// </summary>
        public const int DNS_ERROR_AUTOZONE_ALREADY_EXISTS = 9610;
        /// <summary>
        /// Invalid DNS zone type.
        /// </summary>
        public const int DNS_ERROR_INVALID_ZONE_TYPE = 9611;
        /// <summary>
        /// Secondary DNS zone requires master IP address.
        /// </summary>
        public const int DNS_ERROR_SECONDARY_REQUIRES_MASTER_IP = 9612;
        /// <summary>
        /// DNS zone not secondary.
        /// </summary>
        public const int DNS_ERROR_ZONE_NOT_SECONDARY = 9613;
        /// <summary>
        /// Need secondary IP address.
        /// </summary>
        public const int DNS_ERROR_NEED_SECONDARY_ADDRESSES = 9614;
        /// <summary>
        /// WINS initialization failed.
        /// </summary>
        public const int DNS_ERROR_WINS_INIT_FAILED = 9615;
        /// <summary>
        /// Need WINS servers.
        /// </summary>
        public const int DNS_ERROR_NEED_WINS_SERVERS = 9616;
        /// <summary>
        /// NBTSTAT initialization call failed.
        /// </summary>
        public const int DNS_ERROR_NBSTAT_INIT_FAILED = 9617;
        /// <summary>
        /// Invalid delete of start of authority (SOA)
        /// </summary>
        public const int DNS_ERROR_SOA_DELETE_INVALID = 9618;
        /// <summary>
        /// A conditional forwarding zone already exists for that name.
        /// </summary>
        public const int DNS_ERROR_FORWARDER_ALREADY_EXISTS = 9619;
        /// <summary>
        /// This zone must be configured with one or more master DNS server IP addresses.
        /// </summary>
        public const int DNS_ERROR_ZONE_REQUIRES_MASTER_IP = 9620;
        /// <summary>
        /// The operation cannot be performed because this zone is shutdown.
        /// </summary>
        public const int DNS_ERROR_ZONE_IS_SHUTDOWN = 9621;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_DATAFILE_BASE = 9650;
        /// <summary>
        /// Primary DNS zone requires datafile.
        /// </summary>
        public const int DNS_ERROR_PRIMARY_REQUIRES_DATAFILE = 9651;
        /// <summary>
        /// Invalid datafile name for DNS zone.
        /// </summary>
        public const int DNS_ERROR_INVALID_DATAFILE_NAME = 9652;
        /// <summary>
        /// Failed to open datafile for DNS zone.
        /// </summary>
        public const int DNS_ERROR_DATAFILE_OPEN_FAILURE = 9653;
        /// <summary>
        /// Failed to write datafile for DNS zone.
        /// </summary>
        public const int DNS_ERROR_FILE_WRITEBACK_FAILED = 9654;
        /// <summary>
        /// Failure while reading datafile for DNS zone.
        /// </summary>
        public const int DNS_ERROR_DATAFILE_PARSING = 9655;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_DATABASE_BASE = 9700;
        /// <summary>
        /// DNS record does not exist.
        /// </summary>
        public const int DNS_ERROR_RECORD_DOES_NOT_EXIST = 9701;
        /// <summary>
        /// DNS record format error.
        /// </summary>
        public const int DNS_ERROR_RECORD_FORMAT = 9702;
        /// <summary>
        /// Node creation failure in DNS.
        /// </summary>
        public const int DNS_ERROR_NODE_CREATION_FAILED = 9703;
        /// <summary>
        /// Unknown DNS record type.
        /// </summary>
        public const int DNS_ERROR_UNKNOWN_RECORD_TYPE = 9704;
        /// <summary>
        /// DNS record timed out.
        /// </summary>
        public const int DNS_ERROR_RECORD_TIMED_OUT = 9705;
        /// <summary>
        /// Name not in DNS zone.
        /// </summary>
        public const int DNS_ERROR_NAME_NOT_IN_ZONE = 9706;
        /// <summary>
        /// CNAME loop detected.
        /// </summary>
        public const int DNS_ERROR_CNAME_LOOP = 9707;
        /// <summary>
        /// Node is a CNAME DNS record.
        /// </summary>
        public const int DNS_ERROR_NODE_IS_CNAME = 9708;
        /// <summary>
        /// A CNAME record already exists for given name.
        /// </summary>
        public const int DNS_ERROR_CNAME_COLLISION = 9709;
        /// <summary>
        /// Record only at DNS zone root.
        /// </summary>
        public const int DNS_ERROR_RECORD_ONLY_AT_ZONE_ROOT = 9710;
        /// <summary>
        /// DNS record already exists.
        /// </summary>
        public const int DNS_ERROR_RECORD_ALREADY_EXISTS = 9711;
        /// <summary>
        /// Secondary DNS zone data error.
        /// </summary>
        public const int DNS_ERROR_SECONDARY_DATA = 9712;
        /// <summary>
        /// Could not create DNS cache data.
        /// </summary>
        public const int DNS_ERROR_NO_CREATE_CACHE_DATA = 9713;
        /// <summary>
        /// DNS name does not exist.
        /// </summary>
        public const int DNS_ERROR_NAME_DOES_NOT_EXIST = 9714;
        /// <summary>
        /// Could not create pointer (PTR) record.
        /// </summary>
        public const int DNS_WARNING_PTR_CREATE_FAILED = 9715;
        /// <summary>
        /// DNS domain was undeleted.
        /// </summary>
        public const int DNS_WARNING_DOMAIN_UNDELETED = 9716;
        /// <summary>
        /// The directory service is unavailable.
        /// </summary>
        public const int DNS_ERROR_DS_UNAVAILABLE = 9717;
        /// <summary>
        /// DNS zone already exists in the directory service.
        /// </summary>
        public const int DNS_ERROR_DS_ZONE_ALREADY_EXISTS = 9718;
        /// <summary>
        /// DNS server not creating or reading the boot file for the directory service integrated DNS zone.
        /// </summary>
        public const int DNS_ERROR_NO_BOOTFILE_IF_DS_ZONE = 9719;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_OPERATION_BASE = 9750;
        /// <summary>
        /// DNS AXFR (zone transfer) complete.
        /// </summary>
        public const int DNS_INFO_AXFR_COMPLETE = 9751;
        /// <summary>
        /// DNS zone transfer failed.
        /// </summary>
        public const int DNS_ERROR_AXFR = 9752;
        /// <summary>
        /// Added local WINS server.
        /// </summary>
        public const int DNS_INFO_ADDED_LOCAL_WINS = 9753;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_SECURE_BASE = 9800;
        /// <summary>
        /// Secure update call needs to continue update request.
        /// </summary>
        public const int DNS_STATUS_CONTINUE_NEEDED = 9801;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_SETUP_BASE = 9850;
        /// <summary>
        /// TCP/IP network protocol not installed.
        /// </summary>
        public const int DNS_ERROR_NO_TCPIP = 9851;
        /// <summary>
        /// No DNS servers configured for local system.
        /// </summary>
        public const int DNS_ERROR_NO_DNS_SERVERS = 9852;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_DP_BASE = 9900;
        /// <summary>
        /// The specified directory partition does not exist.
        /// </summary>
        public const int DNS_ERROR_DP_DOES_NOT_EXIST = 9901;
        /// <summary>
        /// The specified directory partition already exists.
        /// </summary>
        public const int DNS_ERROR_DP_ALREADY_EXISTS = 9902;
        /// <summary>
        /// The DS is not enlisted in the specified directory partition.
        /// </summary>
        public const int DNS_ERROR_DP_NOT_ENLISTED = 9903;
        /// <summary>
        /// The DS is already enlisted in the specified directory partition.
        /// </summary>
        public const int DNS_ERROR_DP_ALREADY_ENLISTED = 9904;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int DNS_ERROR_DP_NOT_AVAILABLE = 9905;
        /// <summary>
        /// No information avialable.
        /// </summary>
        public const int WSABASEERR = 10000;
        /// <summary>
        /// A blocking operation was interrupted by a call to WSACancelBlockingCall.
        /// </summary>
        public const int WSAEINTR = 10004;
        /// <summary>
        /// The file handle supplied is not valid.
        /// </summary>
        public const int WSAEBADF = 10009;
        /// <summary>
        /// An attempt was made to access a socket in a way forbidden by its access permissions.
        /// </summary>
        public const int WSAEACCES = 10013;
        /// <summary>
        /// The system detected an invalid pointer address in attempting to use a pointer argument in a call.
        /// </summary>
        public const int WSAEFAULT = 10014;
        /// <summary>
        /// An invalid argument was supplied.
        /// </summary>
        public const int WSAEINVAL = 10022;
        /// <summary>
        /// Too many open sockets.
        /// </summary>
        public const int WSAEMFILE = 10024;
        /// <summary>
        /// A non-blocking socket operation could not be completed immediately.
        /// </summary>
        public const int WSAEWOULDBLOCK = 10035;
        /// <summary>
        /// A blocking operation is currently executing.
        /// </summary>
        public const int WSAEINPROGRESS = 10036;
        /// <summary>
        /// An operation was attempted on a non-blocking socket that already had an operation in progress.
        /// </summary>
        public const int WSAEALREADY = 10037;
        /// <summary>
        /// An operation was attempted on something that is not a socket.
        /// </summary>
        public const int WSAENOTSOCK = 10038;
        /// <summary>
        /// A required address was omitted from an operation on a socket.
        /// </summary>
        public const int WSAEDESTADDRREQ = 10039;
        /// <summary>
        /// A message sent on a datagram socket was larger than the internal message buffer or some other network limit, or the buffer used to receive a datagram into was smaller than the datagram itself.
        /// </summary>
        public const int WSAEMSGSIZE = 10040;
        /// <summary>
        /// A protocol was specified in the socket function call that does not support the semantics of the socket type requested.
        /// </summary>
        public const int WSAEPROTOTYPE = 10041;
        /// <summary>
        /// An unknown, invalid, or unsupported option or level was specified in a getsockopt or setsockopt call.
        /// </summary>
        public const int WSAENOPROTOOPT = 10042;
        /// <summary>
        /// The requested protocol has not been configured into the system, or no implementation for it exists.
        /// </summary>
        public const int WSAEPROTONOSUPPORT = 10043;
        /// <summary>
        /// The support for the specified socket type does not exist in this address family.
        /// </summary>
        public const int WSAESOCKTNOSUPPORT = 10044;
        /// <summary>
        /// The attempted operation is not supported for the type of object referenced.
        /// </summary>
        public const int WSAEOPNOTSUPP = 10045;
        /// <summary>
        /// The protocol family has not been configured into the system or no implementation for it exists.
        /// </summary>
        public const int WSAEPFNOSUPPORT = 10046;
        /// <summary>
        /// An address incompatible with the requested protocol was used.
        /// </summary>
        public const int WSAEAFNOSUPPORT = 10047;
        /// <summary>
        /// Only one usage of each socket address (protocol/network address/port) is normally permitted.
        /// </summary>
        public const int WSAEADDRINUSE = 10048;
        /// <summary>
        /// The requested address is not valid in its context.
        /// </summary>
        public const int WSAEADDRNOTAVAIL = 10049;
        /// <summary>
        /// A socket operation encountered a dead network.
        /// </summary>
        public const int WSAENETDOWN = 10050;
        /// <summary>
        /// A socket operation was attempted to an unreachable network.
        /// </summary>
        public const int WSAENETUNREACH = 10051;
        /// <summary>
        /// The connection has been broken due to keep-alive activity detecting a failure while the operation was in progress.
        /// </summary>
        public const int WSAENETRESET = 10052;
        /// <summary>
        /// An established connection was aborted by the software in your host machine.
        /// </summary>
        public const int WSAECONNABORTED = 10053;
        /// <summary>
        /// An existing connection was forcibly closed by the remote host.
        /// </summary>
        public const int WSAECONNRESET = 10054;
        /// <summary>
        /// An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full.
        /// </summary>
        public const int WSAENOBUFS = 10055;
        /// <summary>
        /// A connect request was made on an already connected socket.
        /// </summary>
        public const int WSAEISCONN = 10056;
        /// <summary>
        /// A request to send or receive data was disallowed because the socket is not connected and (when sending on a datagram socket using a sendto call) no address was supplied.
        /// </summary>
        public const int WSAENOTCONN = 10057;
        /// <summary>
        /// A request to send or receive data was disallowed because the socket had already been shut down in that direction with a previous shutdown call.
        /// </summary>
        public const int WSAESHUTDOWN = 10058;
        /// <summary>
        /// Too many references to some kernel object.
        /// </summary>
        public const int WSAETOOMANYREFS = 10059;
        /// <summary>
        /// A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.
        /// </summary>
        public const int WSAETIMEDOUT = 10060;
        /// <summary>
        /// No connection could be made because the target machine actively refused it.
        /// </summary>
        public const int WSAECONNREFUSED = 10061;
        /// <summary>
        /// Cannot translate name.
        /// </summary>
        public const int WSAELOOP = 10062;
        /// <summary>
        /// Name component or name was too long.
        /// </summary>
        public const int WSAENAMETOOInt32 = 10063;
        /// <summary>
        /// A socket operation failed because the destination host was down.
        /// </summary>
        public const int WSAEHOSTDOWN = 10064;
        /// <summary>
        /// A socket operation was attempted to an unreachable host.
        /// </summary>
        public const int WSAEHOSTUNREACH = 10065;
        /// <summary>
        /// Cannot remove a directory that is not empty.
        /// </summary>
        public const int WSAENOTEMPTY = 10066;
        /// <summary>
        /// A Windows Sockets implementation may have a limit on the number of applications that may use it simultaneously.
        /// </summary>
        public const int WSAEPROCLIM = 10067;
        /// <summary>
        /// Ran out of quota.
        /// </summary>
        public const int WSAEUSERS = 10068;
        /// <summary>
        /// Ran out of disk quota.
        /// </summary>
        public const int WSAEDQUOT = 10069;
        /// <summary>
        /// File handle reference is no longer available.
        /// </summary>
        public const int WSAESTALE = 10070;
        /// <summary>
        /// Item is not available locally.
        /// </summary>
        public const int WSAEREMOTE = 10071;
        /// <summary>
        /// WSAStartup cannot function at this time because the underlying system it uses to provide network services is currently unavailable.
        /// </summary>
        public const int WSASYSNOTREADY = 10091;
        /// <summary>
        /// The Windows Sockets version requested is not supported.
        /// </summary>
        public const int WSAVERNOTSUPPORTED = 10092;
        /// <summary>
        /// Either the application has not called WSAStartup, or WSAStartup failed.
        /// </summary>
        public const int WSANOTINITIALISED = 10093;
        /// <summary>
        /// Returned by WSARecv or WSARecvFrom to indicate the remote party has initiated a graceful shutdown sequence.
        /// </summary>
        public const int WSAEDISCON = 10101;
        /// <summary>
        /// No more results can be returned by WSALookupServiceNext.
        /// </summary>
        public const int WSAENOMORE = 10102;
        /// <summary>
        /// A call to WSALookupServiceEnd was made while this call was still processing. The call has been canceled.
        /// </summary>
        public const int WSAECANCELLED = 10103;
        /// <summary>
        /// The procedure call table is invalid.
        /// </summary>
        public const int WSAEINVALIDPROCTABLE = 10104;
        /// <summary>
        /// The requested service provider is invalid.
        /// </summary>
        public const int WSAEINVALIDPROVIDER = 10105;
        /// <summary>
        /// The requested service provider could not be loaded or initialized.
        /// </summary>
        public const int WSAEPROVIDERFAILEDINIT = 10106;
        /// <summary>
        /// A system call that should never fail has failed.
        /// </summary>
        public const int WSASYSCALLFAILURE = 10107;
        /// <summary>
        /// No such service is known. The service cannot be found in the specified name space.
        /// </summary>
        public const int WSASERVICE_NOT_FOUND = 10108;
        /// <summary>
        /// The specified class was not found.
        /// </summary>
        public const int WSATYPE_NOT_FOUND = 10109;
        /// <summary>
        /// No more results can be returned by WSALookupServiceNext.
        /// </summary>
        public const int WSA_E_NO_MORE = 10110;
        /// <summary>
        /// A call to WSALookupServiceEnd was made while this call was still processing. The call has been canceled.
        /// </summary>
        public const int WSA_E_CANCELLED = 10111;
        /// <summary>
        /// A database query failed because it was actively refused.
        /// </summary>
        public const int WSAEREFUSED = 10112;
        /// <summary>
        /// No such host is known.
        /// </summary>
        public const int WSAHOST_NOT_FOUND = 11001;
        /// <summary>
        /// This is usually a temporary error during hostname resolution and means that the local server did not receive a response from an authoritative server.
        /// </summary>
        public const int WSATRY_AGAIN = 11002;
        /// <summary>
        /// A non-recoverable error occurred during a database lookup.
        /// </summary>
        public const int WSANO_RECOVERY = 11003;
        /// <summary>
        /// The requested name is valid and was found in the database, but it does not have the correct associated data being resolved for.
        /// </summary>
        public const int WSANO_DATA = 11004;
        /// <summary>
        /// At least one reserve has arrived.
        /// </summary>
        public const int WSA_QOS_RECEIVERS = 11005;
        /// <summary>
        /// At least one path has arrived.
        /// </summary>
        public const int WSA_QOS_SENDERS = 11006;
        /// <summary>
        /// There are no senders.
        /// </summary>
        public const int WSA_QOS_NO_SENDERS = 11007;
        /// <summary>
        /// There are no receivers.
        /// </summary>
        public const int WSA_QOS_NO_RECEIVERS = 11008;
        /// <summary>
        /// Reserve has been confirmed.
        /// </summary>
        public const int WSA_QOS_REQUEST_CONFIRMED = 11009;
        /// <summary>
        /// Error due to lack of resources.
        /// </summary>
        public const int WSA_QOS_ADMISSION_FAILURE = 11010;
        /// <summary>
        /// Rejected for administrative reasons - bad credentials.
        /// </summary>
        public const int WSA_QOS_POLICY_FAILURE = 11011;
        /// <summary>
        /// Unknown or conflicting style.
        /// </summary>
        public const int WSA_QOS_BAD_STYLE = 11012;
        /// <summary>
        /// Problem with some part of the filterspec or providerspecific buffer in general.
        /// </summary>
        public const int WSA_QOS_BAD_OBJECT = 11013;
        /// <summary>
        /// Problem with some part of the flowspec.
        /// </summary>
        public const int WSA_QOS_TRAFFIC_CTRL_ERROR = 11014;
        /// <summary>
        /// General QOS error.
        /// </summary>
        public const int WSA_QOS_GENERIC_ERROR = 11015;
        /// <summary>
        /// An invalid or unrecognized service type was found in the flowspec.
        /// </summary>
        public const int WSA_QOS_ESERVICETYPE = 11016;
        /// <summary>
        /// An invalid or inconsistent flowspec was found in the QOS structure.
        /// </summary>
        public const int WSA_QOS_EFLOWSPEC = 11017;
        /// <summary>
        /// Invalid QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_EPROVSPECBUF = 11018;
        /// <summary>
        /// An invalid QOS filter style was used.
        /// </summary>
        public const int WSA_QOS_EFILTERSTYLE = 11019;
        /// <summary>
        /// An invalid QOS filter type was used.
        /// </summary>
        public const int WSA_QOS_EFILTERTYPE = 11020;
        /// <summary>
        /// An incorrect number of QOS FILTERSPECs were specified in the FLOWDESCRIPTOR.
        /// </summary>
        public const int WSA_QOS_EFILTERCOUNT = 11021;
        /// <summary>
        /// An object with an invalid ObjectLength field was specified in the QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_EOBJLENGTH = 11022;
        /// <summary>
        /// An incorrect number of flow descriptors was specified in the QOS structure.
        /// </summary>
        public const int WSA_QOS_EFLOWCOUNT = 11023;
        /// <summary>
        /// An unrecognized object was found in the QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_EUNKOWNPSOBJ = 11024;
        /// <summary>
        /// An invalid policy object was found in the QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_EPOLICYOBJ = 11025;
        /// <summary>
        /// An invalid QOS flow descriptor was found in the flow descriptor list.
        /// </summary>
        public const int WSA_QOS_EFLOWDESC = 11026;
        /// <summary>
        /// An invalid or inconsistent flowspec was found in the QOS provider specific buffer.
        /// </summary>
        public const int WSA_QOS_EPSFLOWSPEC = 11027;
        /// <summary>
        /// An invalid FILTERSPEC was found in the QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_EPSFILTERSPEC = 11028;
        /// <summary>
        /// An invalid shape discard mode object was found in the QOS provider specific buffer.
        /// </summary>
        public const int WSA_QOS_ESDMODEOBJ = 11029;
        /// <summary>
        /// An invalid shaping rate object was found in the QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_ESHAPERATEOBJ = 11030;
        /// <summary>
        /// A reserved policy element was found in the QOS provider-specific buffer.
        /// </summary>
        public const int WSA_QOS_RESERVED_PETYPE = 11031;



        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum FormatFlags : uint
        {
            FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200
        }

        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum FileMapProtection : uint
        {
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }

        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum FileMapAccess : uint
        {
            FileMapAllAccess = 0x001f,
            FileMapCopy = 0x0001,
            fileMapExecute = 0x0020,
            FileMapRead = 0x0004,
            FileMapWrite = 0x0002,
        }

        /// <summary>
        ///
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct CRITICAL_SECTION
        {
            public IntPtr DebugInfo;
            public long LockCount;
            public long RecursionCount;
            public IntPtr OwningThread;
            public IntPtr LockSemaphore;
            public IntPtr SpinCount;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        internal struct MEMORYSTATUS32
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUS
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong dwTotalPhys;
            public ulong dwAvailPhys;
            public ulong dwTotalPageFile;
            public ulong dwAvailPageFile;
            public ulong dwTotalVirtual;
            public ulong dwAvailVirtual;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public uint nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }


        public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const uint SYNCHRONIZE = 0x00100000;
        public const uint EVENT_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x3);
        public const uint EVENT_MODIFY_STATE = 0x0002;
        public const uint DELETE = 0x00010000;
        public const uint READ_CONTROL = 0x00020000;
        public const uint WRITE_DAC = 0x00040000;
        public const uint WRITE_OWNER = 0x00080000;
        public const uint MUTEX_ALL_ACCESS = 0x001F0001;
        public const uint MUTEX_MODIFY_STATE = 0x00000001;
        public const uint SEMAPHORE_ALL_ACCESS = 0x001F0003;
        public const uint SEMAPHORE_MODIFY_STATE = 0x00000002;
        public const uint TIMER_ALL_ACCESS = 0x001F0003;
        public const uint TIMER_MODIFY_STATE = 0x00000002;
        public const uint TIMER_QUERY_STATE = 0x00000001;
        public const uint WAIT_ABANDONED = 0x00000080;
        public const uint WAIT_OBJECT_0 = 0x00000000;
        public const uint WAIT_TIMEOUT = 0x00000102;
        public const uint WAIT_FAILED = 0xFFFFFFFF;
        public const uint INFINITE = 0xFFFFFFFF;

        public const uint SECTION_QUERY = 0x0001;
        public const uint SECTION_MAP_WRITE = 0x0002;
        public const uint SECTION_MAP_READ = 0x0004;
        public const uint SECTION_MAP_EXECUTE = 0x0008;
        public const uint SECTION_EXTEND_SIZE = 0x0010;
        public const uint SECTION_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SECTION_QUERY
                                             | SECTION_MAP_WRITE | SECTION_MAP_READ
                                             | SECTION_MAP_EXECUTE | SECTION_EXTEND_SIZE;
        public const uint FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS;

        public const uint CREATE_NEW = 1;
        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_EXISTING = 3;
        public const uint OPEN_ALWAYS = 4;
        public const uint TRUNCATE_EXISTING = 5;

        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const uint FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        public const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
        public const uint FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public const uint FILE_FLAG_POSIX_SEMANTICS = 0x01000000;
        public const uint FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000;
        public const uint FILE_FLAG_OPEN_NO_RECALL = 0x00100000;
        public const uint FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000;

        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint FILE_SHARE_DELETE = 0x00000004;

        public const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
        public const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        public const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        public const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
        public const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
        public const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
        public const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
        public const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        public const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        public const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint GENERIC_EXECUTE = 0x20000000;
        public const uint GENERIC_ALL = 0x10000000;
        

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern uint WaitForMultipleObjects(uint nCount, IntPtr[] lpHandles, bool bWaitAll, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WaitForSingleObject(IntPtr Handle, uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void InitializeCriticalSection(out CRITICAL_SECTION lpCriticalSection);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void DeleteCriticalSection(ref CRITICAL_SECTION lpCriticalSection);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void EnterCriticalSection(ref CRITICAL_SECTION lpCriticalSection);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void LeaveCriticalSection(ref CRITICAL_SECTION lpCriticalSection);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool TryEnterCriticalSection(ref CRITICAL_SECTION lpCriticalSection);

        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static FileHandle LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static int FreeLibrary(FileHandle hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        [DllImport("kernel32.dll", EntryPoint = "GlobalMemoryStatus", SetLastError = true)]
        private static extern void GlobalMemoryStatus32(ref MEMORYSTATUS32 lpBuffer);

        [DllImport("kernel32.dll", EntryPoint = "GlobalMemoryStatus", SetLastError = true)]
        private static extern void GlobalMemoryStatus64(ref MEMORYSTATUS lpBuffer);

        [DllImport("kernel32.dll")]
        private static extern uint FormatMessage(FormatFlags dwFlags, IntPtr lpSource,
                                         uint dwMessageId,
                                         uint dwLanguageId,
        ref IntPtr lpBuffer,
                                         uint nSize,
                                         IntPtr Arguments);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern FileHandle CreateFile(string lpFileName,
                                                   uint dwDesiredAccess,
                                                   uint dwShareMode,
                                                   IntPtr lpSecurityAttributes,
                                                   uint dwCreationDisposition,
                                                   uint dwFlagsAndAttributes,
                                                   FileHandle hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern FileHandle CreateFile(string lpFileName,
                                                   uint dwDesiredAccess,
                                                   uint dwShareMode,
        ref SECURITY_ATTRIBUTES lpFileMappingAttributes,
                                                   uint dwCreationDisposition,
                                                   uint dwFlagsAndAttributes,
                                                   FileHandle hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern FileHandle OpenFileMapping(uint dwDesiredAccess,
                                                        bool bInheritHandle,
                                                        string lpName);

        [DllImport("kernel32.dll", EntryPoint = "FlushViewOfFile", SetLastError = true)]
        private static extern bool FlushViewOfFile32(IntPtr lpBaseAddress, uint dwNumberOfBytesToFlush);

        [DllImport("kernel32.dll", EntryPoint = "FlushViewOfFile", SetLastError = true)]
        private static extern bool FlushViewOfFile64(IntPtr lpBaseAddress, ulong dwNumberOfBytesToFlush);

        [DllImport("kernel32.dll", EntryPoint = "MapViewOfFile", SetLastError = true)]
        private static extern IntPtr MapViewOfFile32(FileHandle hFileMappingObject,
                                             FileMapAccess dwDesiredAccess,
                                             uint dwFileOffsetHigh,
                                             uint dwFileOffsetLow,
                                             uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", EntryPoint = "MapViewOfFile", SetLastError = true)]
        private static extern IntPtr MapViewOfFile64(FileHandle hFileMappingObject,
                                             FileMapAccess dwDesiredAccess,
                                             uint dwFileOffsetHigh,
                                             uint dwFileOffsetLow,
                                             ulong dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern FileHandle CreateFileMapping(FileHandle hFile,
        ref SECURITY_ATTRIBUTES lpFileMappingAttributes,
                                                          FileMapProtection flProtect,
                                                          uint dwMaximumSizeHigh,
                                                          uint dwMaximumSizeLow,
                                                          string lpName);

        /// <summary>
        /// Gets the size of the framework runtime cpu bit.
        /// </summary>
        /// <returns></returns>
        public static int GetFrameworkRuntimeCPUBitSize()
        {
            return IntPtr.Size * 8;
        }

        /// <summary>
        /// Gets the format message.
        /// </summary>
        /// <param name="lastError">The last error.</param>
        /// <returns></returns>
        public static string GetFormatMessage(int lastError)
        {
            var lpMsgBuf = IntPtr.Zero;
            try
            {
                var flags = FormatFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER | FormatFlags.FORMAT_MESSAGE_FROM_SYSTEM | FormatFlags.FORMAT_MESSAGE_IGNORE_INSERTS;
                var dwChars = FormatMessage(flags, IntPtr.Zero, (uint)lastError, 0, ref lpMsgBuf, 0, IntPtr.Zero);
                if (dwChars == 0)
                {
                    return string.Empty;
                }

                return Marshal.PtrToStringAnsi(lpMsgBuf);
            }
            finally
            {
                if (lpMsgBuf != IntPtr.Zero)
                {
                    LocalFree(lpMsgBuf);
                    lpMsgBuf = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Gets the name of the error.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static string GetErrorName(int result)
        {
            var fields = typeof(Kernel).GetFields();
            foreach (var fi in fields)
            {
                if ((int)fi.GetValue(null) == result)
                {
                    return fi.Name;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Succeeded.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool Succeeded(int result)
        {
            return result == 0;
        }

        /// <summary>
        /// Failed.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool Failed(int result)
        {
            return !Succeeded(result);
        }

        /// <summary>
        /// Globals the memory status.
        /// </summary>
        /// <param name="lpBuffer">The lp buffer.</param>
        public static void GlobalMemoryStatus(ref MEMORYSTATUS lpBuffer)
        {
            lpBuffer.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUS));

            if (GetFrameworkRuntimeCPUBitSize() == 64)
            {
                GlobalMemoryStatus64(ref lpBuffer);
            }
            else
            {
                var tmpMemStatus = new MEMORYSTATUS32() { dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUS)) };

                GlobalMemoryStatus32(ref tmpMemStatus);

                lpBuffer.dwAvailPageFile = tmpMemStatus.dwAvailPageFile;
                lpBuffer.dwAvailPhys = tmpMemStatus.dwAvailPhys;
                lpBuffer.dwAvailVirtual = tmpMemStatus.dwAvailVirtual;
                lpBuffer.dwMemoryLoad = tmpMemStatus.dwMemoryLoad;
                lpBuffer.dwTotalPageFile = tmpMemStatus.dwTotalPageFile;
                lpBuffer.dwTotalPhys = tmpMemStatus.dwTotalPhys;
                lpBuffer.dwTotalVirtual = tmpMemStatus.dwTotalVirtual;
            }
        }

        /// <summary>
        /// Maps the view of file.
        /// </summary>
        /// <param name="hFileMappingObject">The h file mapping object.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="dwFileOffsetHigh">The dw file offset high.</param>
        /// <param name="dwFileOffsetLow">The dw file offset low.</param>
        /// <param name="dwNumberOfBytesToMap">The dw number of bytes to map.</param>
        /// <returns></returns>
        public static IntPtr MapViewOfFile(FileHandle hFileMappingObject,
                                           FileMapAccess dwDesiredAccess,
                                           uint dwFileOffsetHigh,
                                           uint dwFileOffsetLow,
                                           ulong dwNumberOfBytesToMap)
        {
            if (GetFrameworkRuntimeCPUBitSize() == 64)
            {
                return MapViewOfFile64(hFileMappingObject,
                                       dwDesiredAccess,
                                       dwFileOffsetHigh,
                                       dwFileOffsetLow,
                                       dwNumberOfBytesToMap);
            }
            else
            {
                return MapViewOfFile32(hFileMappingObject,
                                       dwDesiredAccess,
                                       dwFileOffsetHigh,
                                       dwFileOffsetLow,
                                       (uint)dwNumberOfBytesToMap);
            }
        }

        /// <summary>
        /// Flushes the view of file.
        /// </summary>
        /// <param name="lpBaseAddress">The lp base address.</param>
        /// <param name="dwNumberOfBytesToFlush">The dw number of bytes to flush.</param>
        /// <returns></returns>
        public static bool FlushViewOfFile(IntPtr lpBaseAddress, ulong dwNumberOfBytesToFlush)
        {
            if (GetFrameworkRuntimeCPUBitSize() == 64)
            {
                return FlushViewOfFile64(lpBaseAddress, dwNumberOfBytesToFlush);
            }
            else
            {
                return FlushViewOfFile32(lpBaseAddress, (uint)dwNumberOfBytesToFlush);
            }
        }
    }
}
