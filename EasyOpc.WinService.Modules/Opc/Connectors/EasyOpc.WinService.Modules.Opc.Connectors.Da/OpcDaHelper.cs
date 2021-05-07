using System;
using System.Net;
using System.Runtime.InteropServices;
using OPC = Opc;
using OPCDA = Opc.Da;

namespace EasyOpc.WinService.Modules.Opc.Connectors.Da
{
    /// <summary>
    /// DA helper
    /// </summary>
    public static class OpcDaHelper
    {
        /// <summary>
        /// Return OPC-URL for server
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="serverName">OPC.DA server name</param>
        /// <param name="clsid">OPC.DA server CLSID</param>
        /// <returns>URL</returns>
        public static OPC.URL CreateURL(string host, string serverName, string clsid = null)
        {
            if (!string.IsNullOrEmpty(host) && host.Length > 0)
            {
                host = host.StartsWith("\\") ? "localhost" : host;
            }

            clsid = string.IsNullOrEmpty(clsid) ? GetClsid(host, serverName) : clsid;

            if (string.IsNullOrEmpty(clsid))
                return new OPC.URL("opcda://" + host + "/" + serverName);
            else
                return new OPC.URL("opcda://" + host + "/" + serverName + "/" + "{" + clsid + "}");
        }

        /// <summary>
        /// Return CLSID for OPC.DA server
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="serverName">OPC.DA server name</param>
        /// <returns>CLSID</returns>
        public static string GetClsid(string host, string serverName)
        {
            string clsid = null;
            try
            {
                clsid = new OpcCom.ServerEnumerator()?.CLSIDFromProgID(serverName, host, new OPC.ConnectData(new NetworkCredential())).ToString();
            }
            catch
            {
                switch (serverName)
                {
                    case ("RSLinx Remote OPC Server"):
                        clsid = "A05BB6D5-2F8A-11D1-9BB0-080009D01446";
                        break;
                    case ("OPCTechs.ModbusTCP30DA.3"):
                        clsid = "73E187E3-C8A9-4FA4-8101-2A5B66B6AC8B";
                        break;
                    case ("OPCTechs.DataExchange30DA.3"):
                        clsid = "F978E55E-A24C-4D75-ABBF-7D41F958BEA1";
                        break;
                    case ("OPCTechs.Modbus30DA.3"):
                        clsid = "00C20757-FF05-430F-B389-B60EEBDF3E29";
                        break;
                }
            }

            return clsid;
        }

        /// <summary>
        /// Converts .NET type to a VarEnum.
        /// </summary>
        /// <param name="type">The .NET type.</param>
        /// <returns>
        /// The VarEnum.
        /// </returns>
        public static VarEnum ToVarEnum(Type type)
        {
            if (type == null)
                return VarEnum.VT_EMPTY;

            if (type == typeof(sbyte))
                return VarEnum.VT_I1;

            if (type == typeof(byte))
                return VarEnum.VT_UI1;

            if (type == typeof(short))
                return VarEnum.VT_I2;

            if (type == typeof(ushort))
                return VarEnum.VT_UI2;

            if (type == typeof(int))
                return VarEnum.VT_I4;

            if (type == typeof(uint))
                return VarEnum.VT_UI4;

            if (type == typeof(long))
                return VarEnum.VT_I8;

            if (type == typeof(ulong))
                return VarEnum.VT_UI8;

            if (type == typeof(float))
                return VarEnum.VT_R4;

            if (type == typeof(double))
                return VarEnum.VT_R8;

            if (type == typeof(decimal))
                return VarEnum.VT_CY;

            if (type == typeof(bool))
                return VarEnum.VT_BOOL;

            if (type == typeof(DateTime))
                return VarEnum.VT_DATE;

            if (type == typeof(string))
                return VarEnum.VT_BSTR;

            if (type == typeof(object))
                return VarEnum.VT_EMPTY;

            if (type == typeof(sbyte[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I1;

            if (type == typeof(byte[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI1;

            if (type == typeof(short[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I2;

            if (type == typeof(ushort[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI2;

            if (type == typeof(int[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I4;

            if (type == typeof(uint[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI4;

            if (type == typeof(long[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_I8;

            if (type == typeof(ulong[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_UI8;

            if (type == typeof(float[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_R4;

            if (type == typeof(double[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_R8;

            if (type == typeof(decimal[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_CY;

            if (type == typeof(bool[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_BOOL;

            if (type == typeof(DateTime[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_DATE;

            if (type == typeof(string[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_BSTR;

            if (type == typeof(object[]))
                return VarEnum.VT_ARRAY | VarEnum.VT_VARIANT;

            // Special cases
            /*if (type == typeof(IllegalType))
                return (VarEnum)Enum.ToObject(typeof(VarEnum), 0x7FFF);*/

            if (type == typeof(Type))
                return VarEnum.VT_I2;

            return VarEnum.VT_EMPTY;
        }

        /// <summary>
        /// Get description for .NET type
        /// </summary>
        /// <param name="id">Type id</param>
        /// <returns>Description for .NET type</returns>
        public static string GetDataTypeDescription(string id)
        {
            switch (id)
            {
                case ("0"): return "Default/Empty";
                case ("2"): return "\"" + "Short Integer" + "\"";
                case ("3"): return "\"" + "Long Integer" + "\"";
                case ("4"): return "\"" + "Single Float" + "\"";
                case ("5"): return "\"" + "Double Float" + "\"";
                case ("6"): return "Currency";
                case ("7"): return "Date";
                case ("8"): return "String";
                case ("10"): return "Error";
                case ("11"): return "Boolean";//Boolean (TRUE = -1, FALSE = 0)
                case ("12"): return "Variant";
                case ("13"): return "Unknown";
                case ("14"): return "Decimal";
                case ("16"): return "Character";
                case ("17"): return "\"" + "Unsigned Character" + "\"";//byte
                case ("18"): return "\"" + "Unsigned Short" + "\"";
                case ("19"): return "\"" + "Unsigned Long" + "\"";
                case ("20"): return "\"" + "64 Bit Integer" + "\"";
                case ("21"): return "\"" + "Unsigned 64 Bit Integer" + "\"";
                case ("22"): return "Integer";
                case ("23"): return "\"" + "Unsigned Integer" + "\"";

                case ("8194"): return "\"" + "Array of Short Integers" + "\"";
                case ("8195"): return "\"" + "Array of Long Integers" + "\"";
                case ("8196"): return "\"" + "Array of Single Floats" + "\"";
                case ("8197"): return "\"" + "Array of Double Floats" + "\"";
                case ("8198"): return "\"" + "Array of Currency Values" + "\"";
                case ("8199"): return "\"" + "Array of Dates" + "\"";
                case ("8200"): return "\"" + "Array of Strings" + "\"";
                case ("8202"): return "\"" + "Array of Error" + "\"";
                case ("8203"): return "\"" + "Array of Booleans" + "\"";

                case ("8204"): return "\"" + "Array of Variants" + "\"";
                case ("8206"): return "\"" + "Array of Decimals" + "\"";

                case ("8208"): return "\"" + "Array of Character" + "\"";
                case ("8209"): return "\"" + "Array of Unsigned Character" + "\"";
                case ("8210"): return "\"" + "Array of Unsigned Short" + "\"";
                case ("8211"): return "\"" + "Array of Unsigned Long" + "\"";
            }
            return "Unknown";
        }

        /// <summary>
        /// Convert int to quality
        /// </summary>
        /// <param name="qualityVal">Quality value</param>
        /// <param name="limitVal">Limit</param>
        /// <returns>Quality</returns>
        public static string GetQuality(int qualityVal, int limitVal)
        {
            int quality = qualityVal & 192;//QQSSSSLL - QQ
            string strQuality = "";

            if (quality == 0)//QQ=00
                strQuality = "Bad, " + GetQualityUnreliable(qualityVal);
            else if (quality == 64)//QQ=01
                strQuality = "Uncertain, " + GetQualityUnreliable(qualityVal);
            else if (quality == 128)//QQ=10
                strQuality = "Unknown";
            else if (quality == 192)//QQ=11
                strQuality = "Good, " + GetQualityUnreliable(qualityVal);

            strQuality += GetQualityLimit(limitVal);

            return "\"" + strQuality + "\"";
        }

        /// <summary>
        /// Convert to quality limit
        /// </summary>
        /// <param name="val">Value</param>
        /// <returns>Quality limit</returns>
        public static string GetQualityLimit(int val)
        {
            int quality = val & 3;//QQSSSSLL - LL

            switch (quality)
            {
                case (1):
                    return ", low limited";
                case (2):
                    return ", high limited";
                case (3):
                    return ", constant";
            }

            return "";
        }

        /// <summary>
        /// Convert to quality limit
        /// </summary>
        /// <param name="val">Value</param>
        /// <returns>Quality limit</returns>
        public static string GetQualityUnreliable(int val)//Недостоверные данные QQ=00
        {
            int quality = val & 60;//QQSSSSLL - SSSS

            switch (quality)
            {
                case (0):
                    return "non-specific";
                case (4):
                    return "bad configuration";//bad configuration
                case (8):
                    return "not connected";
                case (12):
                    return "device failure";
                case (16):
                    return "sensor failure";
                case (20):
                    return "last known value";
                case (24):
                    return "comm failure";
                case (28):
                    return "out of service";
            }

            return "unknown";
        }

        /// <summary>
        /// Convert to quality limit
        /// </summary>
        /// <param name="val">Value</param>
        /// <returns>Quality limit</returns>
        public static string GetQualityInaccurate(int val)//Неточные данные данные QQ=01
        {
            int quality = val & 60;//QQSSSSLL - SSSS

            switch (quality)
            {
                case (0):
                    return "non-specific";
                case (4):
                    return "last usable value";
                case (16):
                    return "sensor not accurate";
                case (20):
                    return "EGU exceeded";
                case (24):
                    return "sub-normal";
            }

            return "unknown";
        }

        /// <summary>
        /// Convert to quality limit
        /// </summary>
        /// <param name="val">Value</param>
        /// <returns>Quality limit</returns>
        public static string GetQualityExact(int val)//Достоверные данные данные QQ=11
        {
            int quality = val & 60;//QQSSSSLL - SSSS

            switch (quality)
            {
                case (0):
                    return "non-specific";
                case (24):
                    return "local override";
            }

            return "unknown";
        }
    }
}
