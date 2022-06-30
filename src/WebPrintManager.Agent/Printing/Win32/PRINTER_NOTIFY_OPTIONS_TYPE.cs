using System.Runtime.InteropServices;

namespace WebPrintManager.Agent.Printing.Win32
{
#pragma warning disable
    [StructLayout(LayoutKind.Sequential)]
    internal class PRINTER_NOTIFY_OPTIONS_TYPE
    {
        public const int JOB_FIELDS_COUNT = 24;
        public const int PRINTER_FIELDS_COUNT = 23;

        public short wJobType;
        public short wJobReserved0;
        public int dwJobReserved1;
        public int dwJobReserved2;
        public int JobFieldCount;
        public IntPtr pJobFields;
        public short wPrinterType;
        public short wPrinterReserved0;
        public int dwPrinterReserved1;
        public int dwPrinterReserved2;
        public int PrinterFieldCount;
        public IntPtr pPrinterFields;

        private void SetupFields()
        {
            if (this.pJobFields.ToInt32() != 0)
            {
                Marshal.FreeHGlobal(this.pJobFields);
            }

            if (this.wJobType == (short)PRINTERNOTIFICATIONTYPES.JOB_NOTIFY_TYPE)
            {
                this.JobFieldCount = JOB_FIELDS_COUNT;
                this.pJobFields = Marshal.AllocHGlobal((JOB_FIELDS_COUNT * 2) - 1);

                Marshal.WriteInt16(pJobFields, 0, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_PRINTER_NAME);
                Marshal.WriteInt16(pJobFields, 2, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_MACHINE_NAME);
                Marshal.WriteInt16(pJobFields, 4, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_PORT_NAME);
                Marshal.WriteInt16(pJobFields, 6, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_USER_NAME);
                Marshal.WriteInt16(pJobFields, 8, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_NOTIFY_NAME);
                Marshal.WriteInt16(pJobFields, 10, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_DATATYPE);
                Marshal.WriteInt16(pJobFields, 12, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_PRINT_PROCESSOR);
                Marshal.WriteInt16(pJobFields, 14, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_PARAMETERS);
                Marshal.WriteInt16(pJobFields, 16, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_DRIVER_NAME);
                Marshal.WriteInt16(pJobFields, 18, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_DEVMODE);
                Marshal.WriteInt16(pJobFields, 20, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_STATUS);
                Marshal.WriteInt16(pJobFields, 22, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_STATUS_STRING);
                Marshal.WriteInt16(pJobFields, 24, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_SECURITY_DESCRIPTOR);
                Marshal.WriteInt16(pJobFields, 26, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_DOCUMENT);
                Marshal.WriteInt16(pJobFields, 28, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_PRIORITY);
                Marshal.WriteInt16(pJobFields, 30, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_POSITION);
                Marshal.WriteInt16(pJobFields, 32, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_SUBMITTED);
                Marshal.WriteInt16(pJobFields, 34, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_START_TIME);
                Marshal.WriteInt16(pJobFields, 36, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_UNTIL_TIME);
                Marshal.WriteInt16(pJobFields, 38, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_TIME);
                Marshal.WriteInt16(pJobFields, 40, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_TOTAL_PAGES);
                Marshal.WriteInt16(pJobFields, 42, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_PAGES_PRINTED);
                Marshal.WriteInt16(pJobFields, 44, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_TOTAL_BYTES);
                Marshal.WriteInt16(pJobFields, 46, (short)PRINTERJOBNOTIFICATIONTYPES.JOB_NOTIFY_FIELD_BYTES_PRINTED);
            }

            if (this.pPrinterFields.ToInt32() != 0)
            {
                Marshal.FreeHGlobal(this.pPrinterFields);
            }

            if (this.wPrinterType == (short)PRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_TYPE)
            {
                this.PrinterFieldCount = PRINTER_FIELDS_COUNT;
                this.pPrinterFields = Marshal.AllocHGlobal((PRINTER_FIELDS_COUNT - 1) * 2);

                Marshal.WriteInt16(pPrinterFields, 0, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_SERVER_NAME);
                Marshal.WriteInt16(pPrinterFields, 2, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_PRINTER_NAME);
                Marshal.WriteInt16(pPrinterFields, 4, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_SHARE_NAME);
                Marshal.WriteInt16(pPrinterFields, 6, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_PORT_NAME);
                Marshal.WriteInt16(pPrinterFields, 8, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_DRIVER_NAME);
                Marshal.WriteInt16(pPrinterFields, 10, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_COMMENT);
                Marshal.WriteInt16(pPrinterFields, 12, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_LOCATION);
                Marshal.WriteInt16(pPrinterFields, 14, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_SEPFILE);
                Marshal.WriteInt16(pPrinterFields, 16, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_PRINT_PROCESSOR);
                Marshal.WriteInt16(pPrinterFields, 18, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_PARAMETERS);
                Marshal.WriteInt16(pPrinterFields, 20, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_DATATYPE);
                Marshal.WriteInt16(pPrinterFields, 22, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_ATTRIBUTES);
                Marshal.WriteInt16(pPrinterFields, 24, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_PRIORITY);
                Marshal.WriteInt16(pPrinterFields, 26, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_DEFAULT_PRIORITY);
                Marshal.WriteInt16(pPrinterFields, 28, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_START_TIME);
                Marshal.WriteInt16(pPrinterFields, 30, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_UNTIL_TIME);
                Marshal.WriteInt16(pPrinterFields, 32, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_STATUS_STRING);
                Marshal.WriteInt16(pPrinterFields, 34, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_CJOBS);
                Marshal.WriteInt16(pPrinterFields, 36, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_AVERAGE_PPM);
                Marshal.WriteInt16(pPrinterFields, 38, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_TOTAL_PAGES);
                Marshal.WriteInt16(pPrinterFields, 40, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_PAGES_PRINTED);
                Marshal.WriteInt16(pPrinterFields, 42, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_TOTAL_BYTES);
                Marshal.WriteInt16(pPrinterFields, 44, (short)PRINTERPRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_FIELD_BYTES_PRINTED);
            }
        }

        public PRINTER_NOTIFY_OPTIONS_TYPE()
        {
            this.wJobType = (short)PRINTERNOTIFICATIONTYPES.JOB_NOTIFY_TYPE;
            this.wPrinterType = (short)PRINTERNOTIFICATIONTYPES.PRINTER_NOTIFY_TYPE;

            this.SetupFields();
        }
    }
#pragma warning enable
}