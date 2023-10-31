using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;
using SerialToKeyboard.Control;

namespace SerialToKeyboard
{
    public partial class FrmMain : Form
    {
        private bool _isListening;
        private readonly int[] _bauds = new[] { 4800, 9600, 19200, 38400, 57600, 115200 };
        private ComToKey _transfer;

        public FrmMain()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Return)
            {
                Debug.Print("Enter received");
            }
            //keyEventArgs.SuppressKeyPress = true;
            //keyEventArgs.Handled = true;
        }

        private void FrmMainLoad(object sender, EventArgs e)
        {
            Debug.Print("FrmMainLoad");
            FillPortList();
            FillBaudList();

            BtnTransferClick(sender, e);
        }

        private void FillBaudList()
        {
            foreach (var baud in _bauds)
            {
                cmbBaud.Items.Add(baud);
            }
            if (cmbBaud.Items.Count > 0)
                cmbBaud.SelectedItem = 9600;
        }

        private void FillPortList()
        {
            cmbPort.Sorted = true;
            var s = SerialPort.GetPortNames();
            foreach (var s1 in s)
            {
                cmbPort.Items.Add(s1);
            }
            if (cmbPort.Items.Count > 0)
                cmbPort.SelectedIndex = 0;
        }

        private void BtnTransferClick(object sender, EventArgs e)
        {
            if (e is MouseEventArgs)
            {
                if (!_isListening)
                {
                    _isListening = true;
                    btnTransfer.Text = "Disable";
                    StartListening();
                }
                else
                {
                    _isListening = false;
                    btnTransfer.Text = "Enable";
                    StopListening();
                }
            }
        }

        private void StopListening()
        {
            _transfer.Stop();
            _transfer.Dispose();
            SetInterfaceEnable(true);
        }

        private void StartListening()
        {
            if (_transfer != null)
                _transfer.Dispose();

            SetInterfaceEnable(false);
            var pName = cmbPort.SelectedItem.ToString();
            int pBaud;
            int.TryParse(cmbBaud.SelectedItem.ToString(), out pBaud);
            _transfer = new ComToKey(new SerialPort(pName, pBaud, Parity.None, 8, StopBits.One));
            _transfer.Start();
        }

        private void SetInterfaceEnable(bool b)
        {
            cmbBaud.Enabled = b;
            cmbPort.Enabled = b;
        }

        private void FrmMainActivated(object sender, EventArgs e)
        {
            Debug.Print("FrmMainActivated");
        }
    }
}
