using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.DirectX.DirectInput;
using JoystickInterface;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using Multimedia;

namespace GameControllerHack
{
    public partial class Form1 : Form
    {
        private Multimedia.Timer mmTimer = new Multimedia.Timer();
        private Device joystickDevice;
        private JoystickState state;
        private JoystickState prevState;
        private OutputDevice outDevice;
        private InputDevice inDevice;
        private byte[] buttons;
        private int [] dpad;
        private int[] axis = new int[4];
        private int[] iNotePlayed = new int[128];
        private int[] iOctavePlayed = new int[128];
        private int[] iPendingNotes = new int[128];
        private int[] iSwingPendingNotes = new int[128];
        private int[] iSwingPendingMsec = new int[128];
        private int[] chordModeNotesI = new int[4];
        private int[] chordModeNotesIV = new int[4];
        private int[] chordModeNotesV = new int[4];
        private int[] chordModeNotesVI = new int[4]; 
        private int[] chordModeNotesActive = new int[4];
        private bool bOctaveLocked = false;
        private bool bTransportRunning = false;
        private bool bInputQuantize = true;
        private bool bPlayingTrip = false;
        private bool bSkipMode = false;
        private bool bDblSkipMode = false;
        private bool bChromaOverride = false;
        private bool bChordMode = false;
        private bool bDrumMode = false;
        private bool bScaleMode = true;
        private bool bBassMode = false;
        private bool bAdvancedMode = false;
        private bool bMajor = false;
        private bool bAutoOctaveUp = false;
        private bool bAutoOctaveDown = false;
        private int outDeviceID;
        private int inDeviceID;
        private double swing = .0001;
        private int Octave = 3;
        private int channel = 0;
        private int root = 0;
        private int msec = 0;
        private int msec16 = 0;
        private int msecT = 0;
        private int clockmsectest = 0;
        private int bpm = 120;
        private int sixteenth = 0;
        private int trip = 0;
        private int iBpmDifferenceSeries = 0;
        private int clockpulse = 0;
        private int iBassSpeed = 1;
        private int LatestNote = 12;
        private int PreviousNote = 12;
        private int ascending = 0;
        private int decending = 0;
        private int LatestButton = 4;
        private int PreviousButton = 4;
        private int LastPitchValue = 0;
        private int LastModValue = 0;
        private int velocity = 127;
        private bool[] bValidNotes = new bool[12];
        private bool[] bNotePlaying = new bool[128];

        public Form1()
        {
            this.FormClosing += Form1_FormClosing;

            InitializeComponent();

            for (int i = 0; i < iNotePlayed.Length; i++)
            {
                iNotePlayed[i] = -1;
            }
            for (int i = 0; i < iOctavePlayed.Length; i++)
            {
                iOctavePlayed[i] = 0;
            }
            for (int i = 0; i < iPendingNotes.Length; i++)
            {
                iPendingNotes[i] = -1;
            }
            for (int i = 0; i < iSwingPendingNotes.Length; i++)
            {
                iSwingPendingNotes[i] = -1;
            }
            for (int i = 0; i < iSwingPendingMsec.Length; i++)
            {
                iSwingPendingMsec[i] = -1;
            }
            chordModeNotesActive[0] = 0 + 48;
            chordModeNotesActive[1] = 3 + 48;
            chordModeNotesActive[2] = 7 + 48;
            chordModeNotesActive[3] = 10 + 48;

            RefreshChords();

            //Minor Pentatonic
            bValidNotes[0] = true;
            bValidNotes[1] = false;
            bValidNotes[2] = false;
            bValidNotes[3] = true;
            bValidNotes[4] = false;
            bValidNotes[5] = true;
            bValidNotes[6] = false;
            bValidNotes[7] = true;
            bValidNotes[8] = false;
            bValidNotes[9] = false;
            bValidNotes[10] = true;
            bValidNotes[11] = false;

            ModeComboBox.Items.Add("Natural Minor");
            ModeComboBox.Items.Add("Minor Pentatonic");
            ModeComboBox.Items.Add("Harmonic Minor");
            ModeComboBox.Items.Add("Major");
            ModeComboBox.Items.Add("Major Pentatonic");
            ModeComboBox.Text = "Natural Minor";

            comboBox1.Items.Add("C");
            comboBox1.Items.Add("C#");
            comboBox1.Items.Add("D");
            comboBox1.Items.Add("D#");
            comboBox1.Items.Add("E");
            comboBox1.Items.Add("F");
            comboBox1.Items.Add("F#");
            comboBox1.Items.Add("G");
            comboBox1.Items.Add("G#");
            comboBox1.Items.Add("A");
            comboBox1.Items.Add("A#");
            comboBox1.Items.Add("B");
            comboBox1.Text = "C";

            chordComboBox.Items.Add("I");
            chordComboBox.Items.Add("IV");
            chordComboBox.Items.Add("V");
            chordComboBox.Items.Add("VI");
            chordComboBox.Text = "I";

            label5.Text = "";
            label7.Text = "";

            this.Text = "Lead Mode";
            
            mmTimer.Mode = TimerMode.Periodic;
            mmTimer.Period = 10;
            mmTimer.Resolution = 10000;
            mmTimer.SynchronizingObject = this;
            mmTimer.Tick += new System.EventHandler(this.mmTimer_Tick);
        }

        protected override void OnLoad(EventArgs e)
        {

            //Set the out and in device to last saved.
            //If it's out of range (removed?), use 0.
            if (OutputDevice.DeviceCount <= Properties.Settings.Default.OutPort)
            {
                outDeviceID = 0;
            }
            else
            {
                outDeviceID = Properties.Settings.Default.OutPort;
            }
            for (int i = 0; i < OutputDevice.DeviceCount; i++)
            {
                outputComboBox1.Items.Add(OutputDevice.GetDeviceCapabilities(i).name.ToString());
            }

            if (InputDevice.DeviceCount <= Properties.Settings.Default.InPort)
            {
                inDeviceID = 0;
            }
            else
            {
                inDeviceID = Properties.Settings.Default.InPort;
            }
            for (int i = 0; i < InputDevice.DeviceCount; i++)
            {
                inputComboBox1.Items.Add(InputDevice.GetDeviceCapabilities(i).name.ToString());
            }

            //Set up last saved values
            comboBox1.Text = Properties.Settings.Default.Root;
            ModeComboBox.Text = Properties.Settings.Default.Scale;
            RefreshScale();
            SetController();
            SetOutput();
            SetInput();

            base.OnLoad(e);
        }

        private void SetController()
        {
            // Find all the GameControl devices that are attached.
            DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl,
                EnumDevicesFlags.AttachedOnly);
            // check that we have at least one device.
            if (gameControllerList.Count > 0)
            {
                // Move to the first device
                gameControllerList.MoveNext();
                DeviceInstance deviceInstance = (DeviceInstance)
                    gameControllerList.Current;

                // create a device from this controller.
                joystickDevice = new Device(deviceInstance.InstanceGuid);
                joystickDevice.SetCooperativeLevel(this,
                    CooperativeLevelFlags.Background |
                    CooperativeLevelFlags.NonExclusive);
            }

            try
            {
                // Tell DirectX that this is a Joystick.
                joystickDevice.SetDataFormat(DeviceDataFormat.Joystick);
                // Finally, acquire the device.
                joystickDevice.Acquire();
                // Find the capabilities of the joystick
                DeviceCaps cps = joystickDevice.Caps;
                // number of Axes
                Debug.WriteLine("Joystick Axis: " + cps.NumberAxes);
                // number of Buttons
                Debug.WriteLine("Joystick Buttons: " + cps.NumberButtons);
                // number of PoV hats
                Debug.WriteLine("Joystick PoV hats: " + cps.NumberPointOfViews);
                Debug.WriteLine("Force Feedback: " + cps.ForceFeedback);
            }
            catch
            {
                if (MessageBox.Show("No game controller found. Please connect one and click \"Retry.\"", "Couldn't open controller!", 
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop) == DialogResult.Retry)
                {
                    SetController();
                }
                else
                {
                    mmTimer.Stop();
                    mmTimer.Dispose();
                    Application.Exit();
                }
            }
            mmTimer.Start();
        }

        private void SetOutput()
        {
            mmTimer.Stop();
            //Assign output device
            if (outDevice != null)
            {
                outDevice.Dispose();
            }

            try
            {
                outDevice = new OutputDevice(outDeviceID);
            }
            catch
            {
                if (MessageBox.Show("If SONAR's engine is running, turn it off and click \"Retry.\"", "Couldn't open MIDI device!",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop) == DialogResult.Retry)
                {
                    SetOutput();
                }
                else
                {
                    Application.Exit();
                }
            }

            outputComboBox1.Text = OutputDevice.GetDeviceCapabilities(outDeviceID).name.ToString();
            mmTimer.Start();
        }

        private void outputComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set output port to what the user chose
            outDeviceID = outputComboBox1.SelectedIndex;
            SetOutput();
        }

        private void SetInput()
        {
            //Set the input device and start listening for trigger CC data
            if (inDevice != null)
            {
                inDevice.Dispose();
            }

            try
            {
                inDevice = new InputDevice(inDeviceID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Close();
            }
            inputComboBox1.Text = InputDevice.GetDeviceCapabilities(inDeviceID).name.ToString();
            inDevice.StartRecording();
            inDevice.SysRealtimeMessageReceived += delegate(object sender, SysRealtimeMessageEventArgs er)
            {
                Debug.WriteLine("Message: " + er.Message.Message.ToString());
                Debug.WriteLine("MessageType: " + er.Message.MessageType.ToString());
                Debug.WriteLine("Status: " + er.Message.Status.ToString());
                Debug.WriteLine("SysRealtimeType: " + er.Message.SysRealtimeType.ToString());
                if (bDrumMode || bBassMode)
                {
                    if (er.Message.Status == 250 && !bTransportRunning)
                    {
                        bTransportRunning = true;
                        Debug.WriteLine("--START--");
                        clockpulse = 0;
                        msec16 = 0;
                        sixteenth = 0;
                        msecT = 0;
                        trip = 0;
                    }

                    else if (er.Message.Status == 252 && bTransportRunning)
                    {
                        bTransportRunning = false;
                        Debug.WriteLine("--STOP--");
                    }

                    if (er.Message.Message == 248)
                    {
                        clockpulse++;
                        if (clockpulse % 24 == 0)
                        { 
                            bpm = Clamp((int)Math.Ceiling(6000 / (double)(msec - clockmsectest)), 30, 300);
                            Debug.WriteLine("Tempo: " + bpm);
                            if (bpm.ToString() != textBox1.Text)
                            {
                                iBpmDifferenceSeries++;
                                if (iBpmDifferenceSeries > 1)
                                {
                                    textBox1.Text = bpm.ToString();
                                    iBpmDifferenceSeries = 0;
                                }
                                else
                                {
                                    bpm = Convert.ToInt16(textBox1.Text);
                                }
                            }
                            else
                            {
                                iBpmDifferenceSeries = 0;
                            }
                            clockmsectest = msec;
                        }
                        if (clockpulse % 6 == 0)
                        {
                            msec16 = 0;
                        }
                        if (clockpulse % 8 == 0)
                        {
                            msecT = 0;
                        }
                        if (clockpulse == int.MaxValue)
                        {
                            clockpulse = 0;
                        }
                    }
                }
            };
        }

        private bool Poll()
        {
            try
            {
                prevState = state;
                // poll the joystick
                joystickDevice.Poll();
                // update the joystick state field
                state = joystickDevice.CurrentJoystickState;
                if (!state.Equals(prevState))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception err)
            {
                // we probably lost connection to the joystick
                // was it unplugged or locked by another application?
                Debug.WriteLine(err.Message);
                return false;
            }
        }

        private void mmTimer_Tick(object sender, EventArgs e)
        {
            if (bDrumMode || bBassMode)
            {
                Random r = new Random();
                velocity = r.Next(110, 127);
                if (msec == int.MaxValue)
                    msec = 0;
                msec++; 
                msec16++;
                msecT++;
                int swingMsec = (int)Math.Ceiling((6000 / (bpm * 4) * iBassSpeed) * swing);
                int sixteenthMsec = (6000 / (bpm * 4));

                if (msec16 >= 6000 / (bpm * 4))
                {
                    sixteenth++;
                    if (sixteenth > 16)
                        sixteenth = 1;
                        msec16 = 0;
                    if (!bPlayingTrip)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            StopNote(iNotePlayed[i], iOctavePlayed[i]);
                            iNotePlayed[i] = -1;
                        }
                    }
                }

                if (msecT >= 6000 / (bpm * 6))
                {
                    trip++;
                    if (trip > 24)
                        trip = 1;
                    msecT = 0;
                    if (bPlayingTrip)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            StopNote(iNotePlayed[i], iOctavePlayed[i]);
                            iNotePlayed[i] = -1;
                        }
                    }
                }

                if (sixteenth % 4 == 0)
                {
                    label5.Text = "Click!";
                    //PlayNote(0);
                }
                else
                {
                    label5.Text = "";
                }
                //Play pending notes
                if (msec16 <= sixteenthMsec / 2 || !bInputQuantize)
                {
                    for (int i = 0; i < iPendingNotes.Length; i++)
                    {
                        if (iPendingNotes[i] != -1)
                        {
                            PlayNote(iPendingNotes[i]);
                            iPendingNotes[i] = -1;
                        }
                    }
                }

                for (int i = 0; i < iSwingPendingNotes.Length; i++)
                {
                    if (iSwingPendingNotes[i] != -1)
                    {
                        if (msec == iSwingPendingMsec[i] + swingMsec)
                        {
                            PlayNote(iSwingPendingNotes[i]);
                            iSwingPendingNotes[i] = -1;
                            iSwingPendingMsec[i] = -1;
                        }
                    }
                }
                
            }
            else
            {
                msec = 0;
                velocity = 127;
            }
            
            if (Poll())
            {
            
                buttons = state.GetButtons();
                dpad = state.GetPointOfView();

                axis[0] = state.Rz;
                axis[1] = state.X;
                axis[2] = state.Y;
                axis[3] = state.Z;
                string sButtonReport = "";
                for (int i = 0; i < 12; i++)
                {
                    sButtonReport += "Button " + i.ToString() + " = " + buttons[i].ToString() + "\n";
                }

                sButtonReport += "D-Pad = " + dpad[0].ToString() + "\n";
                sButtonReport += "Left X = " + axis[1].ToString() + "\n";
                sButtonReport += "Left Y = " + axis[2].ToString() + "\n";
                sButtonReport += "Right X = " + axis[3].ToString() + "\n";
                sButtonReport += "Right Y = " + axis[0].ToString() + "\n";

                if (bBassMode)
                {

                    if ((buttons[0] == 128) && (iNotePlayed[0] == -1) && (trip % iBassSpeed == 0))
                    {
                        bPlayingTrip = true;
                        StopNote(iNotePlayed[0], Octave);
                        iNotePlayed[0] = -1;
                        PlayNote(GetBassNote() + 12);
                        iNotePlayed[0] = GetBassNote() + 12;
                        iOctavePlayed[0] = Octave;
                    }
                    else if ((buttons[1] == 128) && (iNotePlayed[1] == -1) && (trip % iBassSpeed == 0))
                    {
                        bPlayingTrip = true;
                        StopNote(iNotePlayed[1], Octave);
                        iNotePlayed[1] = -1;
                        PlayNote(GetBassNote());
                        iNotePlayed[1] = GetBassNote();
                        iOctavePlayed[1] = Octave;
                    }
                    else if ((buttons[2] == 128) && (iNotePlayed[2] == -1) && (sixteenth % iBassSpeed == 0))
                    {
                        bPlayingTrip = false;
                        StopNote(iNotePlayed[2], Octave);
                        iNotePlayed[2] = GetBassNote();
                        iOctavePlayed[2] = Octave;
                        if ((sixteenth / iBassSpeed) % 2 == 0)
                        {
                            PlayNote(GetBassNote());
                        }
                        else
                        {
                            iSwingPendingNotes[2] = GetBassNote();
                            iSwingPendingMsec[2] = msec;
                        }
                    }
                    else if ((buttons[3] == 128) && (iNotePlayed[3] == -1) && (sixteenth % iBassSpeed == 0))
                    {
                        bPlayingTrip = false;
                        StopNote(iNotePlayed[3], Octave);
                        iNotePlayed[3] = -1;
                        iNotePlayed[3] = GetBassNote() + 12;
                        iOctavePlayed[3] = Octave;
                        if ((sixteenth / iBassSpeed) % 2 == 0)
                        {
                            PlayNote(GetBassNote() + 12);
                        }
                        else
                        {
                            iSwingPendingNotes[3] = GetBassNote() + 12;
                            iSwingPendingMsec[3] = msec;
                        }
                    }
                }

                iBassSpeed = 2;

                if ((buttons[8] == 128) && (iNotePlayed[8] == -1))
                {
                    iNotePlayed[8] = 128;
                    if (bChordMode)
                    {
                        bDrumMode = true;
                        bChordMode = false;
                        bScaleMode = false;
                        bBassMode = false;
                        channel = 9;
                        Octave = 3;
                        textBox1.Visible = true;
                        label7.Text = "Swing: " + ((int)Math.Floor(swing * 100)).ToString() + "%";
                        this.Text = "Drum Mode";
                    }
                    else if (bScaleMode)
                    {
                        bDrumMode = false;
                        bChordMode = true;
                        bScaleMode = false;
                        bBassMode = false;
                        bAdvancedMode = false;
                        Octave = 0;
                        channel = 1;
                        textBox1.Visible = false;
                        label7.Text = "";
                        this.Text = "Chord Mode";
                        RefreshChords();
                    }
                    else if (bDrumMode)
                    {
                        bDrumMode = false;
                        bChordMode = false;
                        bScaleMode = false;
                        bBassMode = true;
                        bAdvancedMode = false;
                        Octave = 4;
                        channel = 2;
                        textBox1.Visible = true;
                        label7.Text = "Swing: " + ((int)Math.Floor(swing * 100)).ToString() + "%";
                        this.Text = "Bass Mode";
                    }
                    else if (bBassMode)
                    {
                        bDrumMode = false;
                        bChordMode = false;
                        bScaleMode = false;
                        bBassMode = false;
                        bAdvancedMode = true;
                        Octave = 3;
                        channel = 0;
                        textBox1.Visible = false;
                        label7.Text = "";
                        this.Text = "Advanced Mode";
                    }
                    else if (bAdvancedMode)
                    {
                        bDrumMode = false;
                        bChordMode = false;
                        bScaleMode = true;
                        bBassMode = false;
                        bAdvancedMode = false;
                        Octave = 3;
                        channel = 0;
                        textBox1.Visible = false;
                        label7.Text = "";
                        this.Text = "Lead Mode";
                    }    
                }
                if ((buttons[8] == 0) && (iNotePlayed[8] != -1))
                {
                    iNotePlayed[8] = -1;
                }

                if ((buttons[9] == 128) && (iNotePlayed[9] == -1))
                {
                    iNotePlayed[9] = 128;
                    if (ModeComboBox.Text == "Natural Minor")
                    {
                        ModeComboBox.Text = "Minor Pentatonic";
                    }
                    else if (ModeComboBox.Text == "Minor Pentatonic")
                    {
                        ModeComboBox.Text = "Natural Minor";
                    }
                    else if (ModeComboBox.Text == "Major Pentatonic")
                    {
                        ModeComboBox.Text = "Major";
                    }
                    else if (ModeComboBox.Text == "Major")
                    {
                        ModeComboBox.Text = "Major Pentatonic";
                    }
                }
                if ((buttons[9] == 0) && (iNotePlayed[9] != -1))
                {
                    iNotePlayed[9] = -1;
                }

                if (dpad[0] == -1)
                {
                    bOctaveLocked = false;
                    bAutoOctaveDown = false;
                    bAutoOctaveUp = false;
                }
                if ((dpad[0] == 0) && (bOctaveLocked == false))
                {
                    Octave++;
                    bOctaveLocked = true;
                }

                if ((dpad[0] == 18000) && (bOctaveLocked == false))
                {
                    Octave--;
                    bOctaveLocked = true;
                }
                if (dpad[0] == 9000)
                {
                    if (bChordMode && !bOctaveLocked)
                    {
                        Invert(true);
                    }
                    if ((bDrumMode || bBassMode) && !bOctaveLocked)
                    {
                        swing += .025;
                        if (swing > .5)
                            swing = .5;
                        label7.Text = "Swing: " + ((int)Math.Floor((double)swing * 100)).ToString() + "%";
                    }
                        bOctaveLocked = true;
                }
                if (dpad[0] == 27000)
                {
                    if (bChordMode && !bOctaveLocked)
                    {
                        Invert(false);
                    }
                    if ((bDrumMode || bBassMode) && !bOctaveLocked)
                    {
                        swing -= .025;
                        if (swing < .01)
                            swing = .0001;
                        label7.Text = "Swing: " + ((int)Math.Floor((double)swing * 100)).ToString() + "%";
                    }
                    bOctaveLocked = true;
                }

                if (axis[3] != LastPitchValue)
                {
                    int nValue = axis[3] / 4;
                    nValue = nValue / 128;
                    ChannelMessage PitchMessage = new ChannelMessage(ChannelCommand.PitchWheel, 0, 0, nValue);
                    outDevice.Send(PitchMessage);
                    LastPitchValue = axis[3];
                }
                if ((axis[0] != LastModValue) && (axis[0] < 32768))
                {
                    int nValue = axis[0] / 256;
                    nValue = 127 - nValue;
                    Debug.WriteLine("ModValue: " + nValue.ToString());
                    ChannelMessage ModMessage = new ChannelMessage(ChannelCommand.Controller, 0, 1, nValue);
                    outDevice.Send(ModMessage);
                    LastModValue = axis[0];
                }

                if (axis[2] < 32767)
                    iBassSpeed = 1;
                if (axis[2] > 32767)
                    iBassSpeed = 4;

                PreviousNote = LatestNote;
                int note = LatestNote;

                if ((buttons[10] == 128) && (iNotePlayed[10] == -1) && (bScaleMode))
                {
                }
                if ((buttons[10] == 0) && (iNotePlayed[10] != -1))
                {
                }
                if ((buttons[11] == 128) && (iNotePlayed[11] == -1))
                {
                }
                if ((buttons[11] == 0) && (iNotePlayed[11] != -1))
                {
                }

                if ((buttons[0] == 128) && (iNotePlayed[0] == -1))
                {
                    if (bChordMode)
                    {
                        iNotePlayed[0] = 128;
                        StashCurrentChord();
                        chordComboBox.Text = "VI";
                        GetRecentChord();
                    }
                    else if (bScaleMode)
                    { 
                        iNotePlayed[0] = 128;
                        bChromaOverride = true;
                        label5.Text = "Chromatic";
                    }
                    else if (bDrumMode && trip % iBassSpeed == 0 && iNotePlayed[0] == -1)
                    {
                        bPlayingTrip = true;
                        if (axis[1] > 32511)
                        {
                            PlayNote(15);
                            iNotePlayed[0] = 17;
                            iOctavePlayed[0] = Octave;
                        }
                        else
                        {
                            PlayNote(17);
                            iNotePlayed[0] = 15;
                            iOctavePlayed[0] = Octave;
                        }
                    }
                    else if (bAdvancedMode)
                    {
                        PlayNote(GetBassNote() + 36);
                        iNotePlayed[0] = GetBassNote() + 36;
                        iOctavePlayed[0] = Octave;
                    }
                }
                if (buttons[0] == 0 && iNotePlayed[0] != -1)
                {
                    StopNote(iNotePlayed[0], iOctavePlayed[0]);
                    iNotePlayed[0] = -1;
                    bChromaOverride = false;
                    if (bScaleMode)
                        label5.Text = "";
                }

                if (buttons[1] == 128 && iNotePlayed[1] == -1)
                {
                    if (bChordMode)
                    {
                        StashCurrentChord();
                        chordComboBox.Text = "V";
                        GetRecentChord();
                    }
                    else if (bScaleMode)
                    {
                        int change = 0;
                        int nearestgoal = LatestNote % 12;
                        int goal = GetScaleNote(0);

                        if (dpad[0] == 9000)
                            goal = GetScaleNote(4);
                        else if (dpad[0] == 27000)
                            goal = GetScaleNote(2);

                        while (goal < nearestgoal)
                            goal += 12;

                        while (nearestgoal != goal && nearestgoal != goal - 12)
                        {
                            change++;
                            nearestgoal++;
                        }

                        if (nearestgoal != LatestNote)
                        {
                            nearestgoal = LatestNote + change;
                            if (LatestButton == 2 && LatestNote % 12 == nearestgoal % 12)
                                Octave++;
                            if (bNotePlaying[nearestgoal] == true)
                                StopNote(nearestgoal, Octave);
                            PreviousButton = LatestButton;
                            LatestButton = 1;
                            PlayNote(nearestgoal);
                            iNotePlayed[1] = nearestgoal;
                            iOctavePlayed[1] = Octave;
                        }
                    }
                    else if (bDrumMode && trip % iBassSpeed == 0 && iNotePlayed[1] == -1)
                    {
                        bPlayingTrip = true;
                        if (axis[1] > 32511)
                        {
                            PlayNote(6);
                            iNotePlayed[1] = 6;
                            iOctavePlayed[1] = Octave;
                        }
                        else
                        {
                            PlayNote(10);
                            iNotePlayed[1] = 10;
                            iOctavePlayed[1] = Octave;
                        }
                    }
                    else if (bAdvancedMode)
                    {
                        PlayNote(GetBassNote() + 24);
                        iNotePlayed[1] = GetBassNote() + 24;
                        iOctavePlayed[1] = Octave;
                    }
                }

                if (buttons[1] == 0 && iNotePlayed[1] != -1)
                {
                    StopNote(iNotePlayed[1], iOctavePlayed[1]);
                    iNotePlayed[1] = -1;
                    bPlayingTrip = false;
                }

                if (buttons[2] == 128 && iNotePlayed[2] == -1)
                {
                    if (bChordMode)
                    {
                        StashCurrentChord();
                        chordComboBox.Text = "I";
                        GetRecentChord();
                    }
                    else if (bScaleMode && iNotePlayed[2] == -1)
                    {
                        int change = 0;
                        int nearestgoal = LatestNote % 12;
                        int goal = GetScaleNote(0);

                        if (dpad[0] == 9000)
                            goal = GetScaleNote(4);
                        else if (dpad[0] == 27000)
                            goal = GetScaleNote(2);

                        while (goal > nearestgoal)
                            nearestgoal += 12;

                        while (nearestgoal != goal && nearestgoal != goal - 12)
                        {
                            change--;
                            nearestgoal--;
                        }

                        if (nearestgoal != LatestNote)
                        {
                            if (LatestButton == 1 && LatestNote % 12 == nearestgoal % 12)
                                Octave--;
                            nearestgoal = LatestNote + change;
                            if (nearestgoal < 0)
                            {
                                Octave--;
                                nearestgoal += 12;
                            }
                            if (bNotePlaying[nearestgoal] == true)
                                StopNote(nearestgoal, Octave);
                            PreviousButton = LatestButton;
                            LatestButton = 2;
                            PlayNote(nearestgoal);
                            iNotePlayed[2] = nearestgoal;
                            iOctavePlayed[2] = Octave;
                        }
                    }
                    else if (bDrumMode && sixteenth % iBassSpeed == 0 && iNotePlayed[2] == -1)
                    {
                        bPlayingTrip = false;
                        int hatNote;
                        if (axis[1] > 32511)
                        {
                            hatNote = 6;
                        }
                        else
                        {
                            hatNote = 10;
                        }
                        if ((sixteenth / iBassSpeed) % 2 == 0)
                        {
                            PlayNote(hatNote);
                        }
                        else
                        {
                            iSwingPendingNotes[2] = hatNote;
                            iSwingPendingMsec[2] = msec;
                        }
                        iNotePlayed[2] = hatNote;
                        iOctavePlayed[2] = Octave;
                    }
                    else if (bAdvancedMode)
                    {
                        PlayNote(GetBassNote());
                        iNotePlayed[2] = GetBassNote();
                        iOctavePlayed[2] = Octave;
                    }
                }
                if ((buttons[2] == 0) && (iNotePlayed[2] != -1))
                {
                    StopNote(iNotePlayed[2], iOctavePlayed[2]);
                    iNotePlayed[2] = -1;
                }


                if (buttons[3] == 128)
                {
                    if (bChordMode)
                    {
                        StashCurrentChord();
                        chordComboBox.Text = "IV";
                        GetRecentChord();
                    }
                    else if (bDrumMode && sixteenth % iBassSpeed == 0 && iNotePlayed[3] == -1)
                    {
                        bPlayingTrip = false;
                        int rideNote;
                        if (axis[1] > 32511)
                        {
                            rideNote = 15;
                        }
                        else
                        {
                            rideNote = 17;
                        }
                        if ((sixteenth / iBassSpeed) % 2 == 0)
                        {
                            PlayNote(rideNote);
                        }
                        else
                        {
                            iSwingPendingNotes[3] = rideNote;
                            iSwingPendingMsec[3] = msec;
                        }
                        iNotePlayed[3] = rideNote;
                        iOctavePlayed[3] = Octave;
                    }
                    else if (bAdvancedMode && iNotePlayed[3] == -1)
                    {
                        PlayNote(GetBassNote() + 12);
                        iNotePlayed[3] = GetBassNote() + 12;
                        iOctavePlayed[3] = Octave;
                    }
                }
                if (buttons[3] == 0 && iNotePlayed[3] != -1)
                {
                    StopNote(iNotePlayed[3], iOctavePlayed[3]);
                    iNotePlayed[3] = -1;
                }

                if ((buttons[4] == 128) && (iNotePlayed[4] == -1))
                {
                    if (bChordMode)
                    {
                        note = chordModeNotesActive[0];
                        PlayNote(note);
                    }
                    else if (bScaleMode)
                    {
                        if (LatestButton != 4)
                        {
                            note = GetNextNote(LatestNote, true);
                            if (bSkipMode || bDblSkipMode)
                            {
                                note = GetNextNote(note, true);
                                if (bDblSkipMode)
                                    note = GetNextNote(note, true);
                            }
                        }
                        PlayNote(note);
                    }
                    else if (bDrumMode && iNotePlayed[4] == -1)
                    {
                        note = 0;
                        iPendingNotes[4] = note;
                    }
                    iNotePlayed[4] = note;
                    iOctavePlayed[4] = Octave;
                    PreviousButton = LatestButton;
                    LatestButton = 4;

                }
                if ((buttons[4] == 0) && (iNotePlayed[4] != -1))
                {
                    StopNote(iNotePlayed[4], iOctavePlayed[4]);

                    iNotePlayed[4] = -1;
                }

                if ((buttons[5] == 128) && (iNotePlayed[5] == -1))
                {
                    if (bChordMode)
                    {
                        note = chordModeNotesActive[1];
                        PlayNote(note);
                    }
                    else if (bScaleMode)
                    {
                        if (LatestButton != 5)
                        {
                            note = GetNextNote(LatestNote, true);
                            if (bSkipMode || bDblSkipMode)
                            {
                                note = GetNextNote(note, true);
                                if (bDblSkipMode)
                                    note = GetNextNote(note, true);
                            }
                        }
                        PlayNote(note);
                    }
                    else if (bDrumMode && iNotePlayed[5] == -1)
                    {
                        note = 2;
                        iPendingNotes[5] = note;
                    }

                    iNotePlayed[5] = note;
                    iOctavePlayed[5] = Octave;
                    PreviousButton = LatestButton;
                    LatestButton = 5;
                }
                if ((buttons[5] == 0) && (iNotePlayed[5] != -1))
                {
                    StopNote(iNotePlayed[5], iOctavePlayed[5]);

                    iNotePlayed[5] = -1;
                }

                if ((buttons[6] == 128) && (iNotePlayed[6] == -1))
                {
                    if (bChordMode)
                    {
                        note = chordModeNotesActive[2];
                        PlayNote(note);
                    }
                    else if (bScaleMode)
                    {
                        if (LatestButton != 6)
                        {
                            note = GetNextNote(LatestNote, false);
                            if (bSkipMode || bDblSkipMode)
                            {
                                note = GetNextNote(note, false);
                                if (bDblSkipMode)
                                    note = GetNextNote(note, false);
                            }
                        }
                        PlayNote(note);
                    }
                    else if (bDrumMode &&  iNotePlayed[6] == -1)
                    {
                        note = 16;
                        iPendingNotes[6] = note;
                    }


                    iNotePlayed[6] = note;
                    iOctavePlayed[6] = Octave;
                    PreviousButton = LatestButton;
                    LatestButton = 6;
                }
                if ((buttons[6] == 0) && (iNotePlayed[6] != -1))
                {
                    StopNote(iNotePlayed[6], iOctavePlayed[6]);

                    iNotePlayed[6] = -1;
                }
                if ((buttons[7] == 128) && (iNotePlayed[7] == -1))
                {
                    if (bChordMode)
                    {
                        note = chordModeNotesActive[3];
                        PlayNote(note);
                    }
                    else if (bScaleMode)
                    {
                        if (LatestButton != 7)
                        {
                            note = GetNextNote(LatestNote, false);
                            if (bSkipMode || bDblSkipMode)
                            {
                                note = GetNextNote(note, false);
                                if (bDblSkipMode)
                                    note = GetNextNote(note, false);
                            }
                        }
                        PlayNote(note);
                    }
                    else if (bDrumMode && iNotePlayed[7] == -1)
                    {
                        note = 13;
                        iPendingNotes[7] = note;
                    }

                    iNotePlayed[7] = note;
                    iOctavePlayed[7] = Octave;
                    PreviousButton = LatestButton;
                    LatestButton = 7;
                }
                if ((buttons[7] == 0) && (iNotePlayed[7] != -1))
                {
                    StopNote(iNotePlayed[7], iOctavePlayed[7]);

                    iNotePlayed[7] = -1;
                }
                if ((bChordMode) && (buttons[10] != 128))
                {
                    if (PreviousNote < LatestNote)
                    {
                        ascending++;
                        decending = 0;
                    }
                    if (PreviousNote > LatestNote)
                    {
                        decending++;
                        ascending = 0;
                    }
                    if (ascending > 2)
                    {
                        ascending = 0;
                        decending = 0;
                        if (bAutoOctaveUp)
                            Octave++;
                    }
                    if (decending > 2)
                    {
                        ascending = 0;
                        decending = 0;
                        if (bAutoOctaveDown)
                            Octave--;
                    }
                }
                richTextBox1.Text = sButtonReport;   
            }
        }

        private void PlayNote(int note)
        {
            LatestNote = note;
            note = note + (Octave * 12);

            if (!bDrumMode)
                note = note + root;
            
            if (note < 127 && note >= 0)
            {
                ChannelMessage noteOn = new ChannelMessage(ChannelCommand.NoteOn, channel, note, velocity);
                outDevice.Send(noteOn);
                if (note < pianoControl1.HighNoteID && note > pianoControl1.LowNoteID)
                    pianoControl1.PressPianoKey(note);
                bNotePlaying[note] = true;
            }

        }
        private void StopNote(int note, int offOctave)
        {
            note = note + (offOctave * 12); ;
            
            if (!bDrumMode)
                note = (note + root); 

            if (note < 127 && note >= 0)
            {
                ChannelMessage noteOff = new ChannelMessage(ChannelCommand.NoteOff, channel, note, 127);
                outDevice.Send(noteOff);
                if (note < pianoControl1.HighNoteID && note > pianoControl1.LowNoteID)
                    pianoControl1.ReleasePianoKey(note);
                bNotePlaying[note] = false;
            }
        }

        private int GetBassNote()
        {
            int ScaleDegree = 0;

            if (buttons[4] == 128 && buttons[5] == 128)
            {
                ScaleDegree = 2;
            }
            else if (buttons[5] == 128 && buttons[6] == 128)
            {
                ScaleDegree = 4;
            }
            else if (buttons[6] == 128 && buttons[7] == 128)
            {
                ScaleDegree = 6;
            }
            else if (buttons[4] == 128)
            {
                ScaleDegree = 1;
            }
            else if (buttons[5] == 128)
            {
                ScaleDegree = 3;
            }
            else if (buttons[6] == 128)
            {
                ScaleDegree = 5;
            }
            else if (buttons[7] == 128)
            {
                ScaleDegree = 7;
            }

            return GetScaleNote(ScaleDegree);
        }

        private int GetScaleNote(int ScaleDegree)
        {
            int Note = 0;

            for (int i = 0; i < ScaleDegree; i++)
            {
                Note = GetNextNote(Note, true);
            }
            return Note;
        }

        private int GetNextNote(int currentNote, bool up)
        {
            bool done = false;
            int newNote = 0;
            int modCurrentNote = currentNote % 12;
            if (up)
            {
                while (!done)
                {
                    newNote++;
                    modCurrentNote = (currentNote + (12 * Octave) + newNote) % 12;
                    if ((modCurrentNote >= 0) && (bValidNotes[modCurrentNote] == true || bChromaOverride))
                    {
                        done = true;
                    }
                }
                newNote = currentNote + newNote;
            }
            else
            {
                while (!done)
                {    
                    newNote++;
                    modCurrentNote = (currentNote + (12 * Octave) - newNote) % 12;
                    if ((modCurrentNote >= 0) && (bValidNotes[modCurrentNote] == true || bChromaOverride))
                    {
                        done = true;
                    }
                }
                newNote = currentNote - newNote;
            }
            return newNote;
        }

        private void ModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshScale();
        }

        private void RefreshScale()
        {
            if (ModeComboBox.Text == "Minor Pentatonic")
            {
                //Minor Pentatonic
                bMajor = false;
                bValidNotes[0] = true;
                bValidNotes[1] = false;
                bValidNotes[2] = false;
                bValidNotes[3] = true;
                bValidNotes[4] = false;
                bValidNotes[5] = true;
                bValidNotes[6] = false;
                bValidNotes[7] = true;
                bValidNotes[8] = false;
                bValidNotes[9] = false;
                bValidNotes[10] = true;
                bValidNotes[11] = false;
            }
            else if (ModeComboBox.Text == "Major Pentatonic")
            {
                bMajor = true;
                bValidNotes[0] = true;
                bValidNotes[1] = false;
                bValidNotes[2] = true;
                bValidNotes[3] = false;
                bValidNotes[4] = false;
                bValidNotes[5] = true;
                bValidNotes[6] = false;
                bValidNotes[7] = true;
                bValidNotes[8] = false;
                bValidNotes[9] = true;
                bValidNotes[10] = false;
                bValidNotes[11] = false;
            }
            else if (ModeComboBox.Text == "Major")
            {
                bMajor = true;
                bValidNotes[0] = true;
                bValidNotes[1] = false;
                bValidNotes[2] = true;
                bValidNotes[3] = false;
                bValidNotes[4] = true;
                bValidNotes[5] = true;
                bValidNotes[6] = false;
                bValidNotes[7] = true;
                bValidNotes[8] = false;
                bValidNotes[9] = true;
                bValidNotes[10] = false;
                bValidNotes[11] = true;
            }
            else if (ModeComboBox.Text == "Harmonic Minor")
            {
                bMajor = false;
                bValidNotes[0] = true;
                bValidNotes[1] = false;
                bValidNotes[2] = true;
                bValidNotes[3] = true;
                bValidNotes[4] = false;
                bValidNotes[5] = true;
                bValidNotes[6] = false;
                bValidNotes[7] = true;
                bValidNotes[8] = true;
                bValidNotes[9] = false;
                bValidNotes[10] = false;
                bValidNotes[11] = true;
            }
            else
            {
                //Natural Minor
                bMajor = false;
                bValidNotes[0] = true;
                bValidNotes[1] = false;
                bValidNotes[2] = true;
                bValidNotes[3] = true;
                bValidNotes[4] = false;
                bValidNotes[5] = true;
                bValidNotes[6] = false;
                bValidNotes[7] = true;
                bValidNotes[8] = true;
                bValidNotes[9] = false;
                bValidNotes[10] = true;
                bValidNotes[11] = false;
            }
            RefreshChords();

        }
        private void RefreshChords()
        {

                if (bMajor)
                {
                    //vi
                    chordModeNotesVI[0] = 9 + 48;
                    chordModeNotesVI[1] = 12 + 48;
                    chordModeNotesVI[2] = 16 + 48;
                    chordModeNotesVI[3] = 19 + 48;
                }
                else
                {
                    //VI
                    chordModeNotesVI[0] = 8 + 48;
                    chordModeNotesVI[1] = 12 + 48;
                    chordModeNotesVI[2] = 15 + 48;
                    chordModeNotesVI[3] = 19 + 48;
                }

                if (bMajor)
                {
                    //IV
                    chordModeNotesIV[0] = 5 + 48;
                    chordModeNotesIV[1] = 9 + 48;
                    chordModeNotesIV[2] = 12 + 48;
                    chordModeNotesIV[3] = 16 + 48;
                }
                else
                {
                    //iv
                    chordModeNotesIV[0] = 5 + 48;
                    chordModeNotesIV[1] = 8 + 48;
                    chordModeNotesIV[2] = 12 + 48;
                    chordModeNotesIV[3] = 15 + 48;
                }

                if (bMajor)
                {
                    //I
                    chordModeNotesI[0] = 0 + 48;
                    chordModeNotesI[1] = 4 + 48;
                    chordModeNotesI[2] = 7 + 48;
                    chordModeNotesI[3] = 11 + 48;
                }
                else
                {
                    //i
                    chordModeNotesI[0] = 0 + 48;
                    chordModeNotesI[1] = 3 + 48;
                    chordModeNotesI[2] = 7 + 48;
                    chordModeNotesI[3] = 10 + 48;
                }

                if (bMajor)
                {
                    //V
                    chordModeNotesV[0] = 7 + 48;
                    chordModeNotesV[1] = 11 + 48;
                    chordModeNotesV[2] = 14 + 48;
                    chordModeNotesV[3] = 17 + 48;
                }
                else
                {
                    //v
                    chordModeNotesV[0] = 7 + 48;
                    chordModeNotesV[1] = 10 + 48;
                    chordModeNotesV[2] = 14 + 48;
                    chordModeNotesV[3] = 17 + 48;
                }
            
        }
        private void StashCurrentChord()
        {
            if (chordComboBox.Text == "I")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesI[i] = chordModeNotesActive[i];
                }
            }
            if (chordComboBox.Text == "IV")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesIV[i] = chordModeNotesActive[i];
                }
            }
            if (chordComboBox.Text == "VI")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesVI[i] = chordModeNotesActive[i];
                }
            }
            if (chordComboBox.Text == "V")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesV[i] = chordModeNotesActive[i];
                }
            }
        }

        private void GetRecentChord()
        {
            if (chordComboBox.Text == "I")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesActive[i] = chordModeNotesI[i];
                }
            }
            if (chordComboBox.Text == "IV")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesActive[i] = chordModeNotesIV[i];
                }
            }
            if (chordComboBox.Text == "VI")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesActive[i] = chordModeNotesVI[i];
                }
            }
            if (chordComboBox.Text == "V")
            {
                for (int i = 0; i < chordModeNotesActive.Length; i++)
                {
                    chordModeNotesActive[i] = chordModeNotesV[i];
                }
            }
        }

        private void Invert(bool up)
        {
            if (up)
            {
                int minValue = chordModeNotesActive.Min();
                int minIndex = chordModeNotesActive.ToList().IndexOf(minValue);
                chordModeNotesActive[minIndex] += 12;
            }
            else
            {

                int maxValue = chordModeNotesActive.Max();
                int maxIndex = chordModeNotesActive.ToList().IndexOf(maxValue);
                chordModeNotesActive[maxIndex] -= 12;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            root = comboBox1.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bpm = Convert.ToInt16(textBox1.Text);
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mmTimer.Stop();
            mmTimer.Dispose();

            Properties.Settings.Default.OutPort = outDeviceID;
            Properties.Settings.Default.InPort = inDeviceID;
            Properties.Settings.Default.Scale = ModeComboBox.Text;
            Properties.Settings.Default.Root = comboBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void inputComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

    }
}