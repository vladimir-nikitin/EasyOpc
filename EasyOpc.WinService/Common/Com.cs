using System;
using System.Runtime.InteropServices;

namespace EasyOpc.WinService
{
	/// <summary>
	/// COM
	/// </summary>
	public class Com
	{
		public enum RpcAuthnLevel
		{
			Default = 0,
			None = 1,
			Connect = 2,
			Call = 3,
			Pkt = 4,
			PktIntegrity = 5,
			PktPrivacy = 6
		}

		public enum RpcImpLevel
		{
			Default = 0,
			Anonymous = 1,
			Identify = 2,
			Impersonate = 3,
			Delegate = 4
		}

		public enum EoAuthnCap
		{
			None = 0x00,
			MutualAuth = 0x01,
			StaticCloaking = 0x20,
			DynamicCloaking = 0x40,
			AnyAuthority = 0x80,
			MakeFullSIC = 0x100,
			Default = 0x800,
			SecureRefs = 0x02,
			AccessControl = 0x04,
			AppID = 0x08,
			Dynamic = 0x10,
			RequireFullSIC = 0x200,
			AutoImpersonate = 0x400,
			NoCustomMarshal = 0x2000,
			DisableAAA = 0x1000
		}

        public enum COINIT : uint //tagCOINIT
        {
            COINIT_MULTITHREADED = 0x0, //Initializes the thread for multi-threaded object concurrency.
            COINIT_APARTMENTTHREADED = 0x2, //Initializes the thread for apartment-threaded object concurrency
            COINIT_DISABLE_OLE1DDE = 0x4, //Disables DDE for OLE1 support
            COINIT_SPEED_OVER_MEMORY = 0x8, //Trade memory for speed
        }

        [DllImport("Ole32.dll")]
		public static extern int CoInitializeSecurity(IntPtr pVoid, int
			cAuthSvc, IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level,
			RpcImpLevel impers, IntPtr pAuthList,
			EoAuthnCap dwCapabilities, IntPtr
			pReserved3);

        [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int CoInitializeEx([In, Optional] IntPtr pvReserved, [In]  COINIT dwCoInit);

        public static int CoInitializeEx()
        {
            return CoInitializeEx(dwCoInit: COINIT.COINIT_MULTITHREADED);
        }

        public static int CoInitializeSecurity()
		{
			try
			{
				//Bootstrap.Initialize();
				// System.Threading.Thread.CurrentThread.ApartmentState = ApartmentState.STA;
				return CoInitializeSecurity(IntPtr.Zero, -1, IntPtr.Zero,
					   IntPtr.Zero, RpcAuthnLevel.None,
					   RpcImpLevel.Impersonate, IntPtr.Zero,
					   EoAuthnCap.None, IntPtr.Zero);

				//System.Windows.Forms.MessageBox.Show(t.ToString()); //*/
			}
			catch (Exception ex) { /*System.Windows.Forms.MessageBox.Show(ex.Message);*/ }

			return -1;
		}
	}
}
