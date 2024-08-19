using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace WiinUPro
{
    public class ScpDirector //todo: change director name
    {
        public const int MAX_XINPUT_INSTANCES = 4;

        #region Access
        public static ScpDirector Access { get; protected set; }

        static ScpDirector()
        {
            Access = new ScpDirector();
            Access.Available = true;
        }
        #endregion
        
        protected List<XInputBus> _xInstances;
        protected bool[] _deviceStatus;

        /// <summary>
        /// Gets the desired XInputBus (0 to 3).
        /// </summary>
        /// <param name="index">Any out of range index will return the first device.</param>
        /// <returns>The XInputBus</returns>
        //protected XInputBus this[int index]
        //{
        //    get
        //    {
        //        if (index < 0 || index >= MAX_XINPUT_INSTANCES)
        //        {
        //            index = 0;
        //        }
        //        
        //        while (index >= _xInstances.Count)
        //        {
        //            _xInstances.Add(new XInputBus(index));
        //        }
        //
        //        return _xInstances[index];
        //    }
        //}

        public bool Available { get; protected set; }
        public int Instances { get { return _xInstances.Count; } }

        // Left motor is the larger one
        public delegate void RumbleChangeDelegate(byte leftMotor, byte rightMotor);

        public ScpDirector()
        {
            _xInstances = new List<XInputBus>
            {
                new XInputBus((int)XInput_Device.Device_A),
                new XInputBus((int)XInput_Device.Device_B),
                new XInputBus((int)XInput_Device.Device_C),
                new XInputBus((int)XInput_Device.Device_D)
            };
            _deviceStatus = new bool[] { false, false, false, false };
        }

        public void SetButton(Xbox360Button button, bool pressed)
        {
            SetButton(button, pressed, XInput_Device.Device_A);
        }

        public void SetButton(Xbox360Button button, bool pressed, XInput_Device device)
        {
            //this[(int)device].SetInput(button, pressed);
            _xInstances[(int)device - 1].SetInput(button, pressed);
        }

        public void SetAxis(Xbox360Axis axis, float value)
        {
            SetAxis(axis, value, XInput_Device.Device_A);
        }

        public void SetAxis(Xbox360Axis axis, float value, XInput_Device device)
        {
            //this[(int)device].SetInput(axis, value);
            _xInstances[(int)device - 1].SetInput(axis, value);
        }

        public void SetSlider(Xbox360Slider slider, float value)
        {
            SetSlider(slider, value, XInput_Device.Device_A);
        }

        public void SetSlider(Xbox360Slider slider, float value, XInput_Device device)
        {
            //this[(int)device].SetInput(slider, value);
            _xInstances[(int)device - 1].SetInput(slider, value);
        }

        /// <summary>
        /// Connects up to the given device.
        /// Ex: If C is used then A, B, & C will be connected.
        /// </summary>
        /// <param name="device">The highest device to be connected.</param>
        /// <returns>If all connections are successful.</returns>
        public bool ConnectDevice(XInput_Device device)
        {
            bool result = _deviceStatus[(int)device - 1];

            if (!result)
            {
                result = _xInstances[(int)device - 1].Connect();
            }

            return result;
        }

        /// <summary>
        /// Disconnects down to the given device.
        /// Ex: If A is used then all of the devices will be disconnected.
        /// </summary>
        /// <param name="device">The lowest device to be disconnected.</param>
        /// <returns>If all devices were disconnected</returns>
        public bool DisconnectDevice(XInput_Device device)
        {
            if (_deviceStatus[(int)device - 1])
            {
                return true;
            }
            else
            {
                return _xInstances[(int)device - 1].Disconnect();
            }
        }

        public bool IsConnected(XInput_Device device)
        {
            return _xInstances[(int)device - 1].PluggedIn;
        }

        public void Apply(XInput_Device device)
        {
            //this[(int)device].Update();
            _xInstances[(int)device - 1].Update();
        }

        public void ApplyAll()
        {
            foreach (var bus in _xInstances.ToArray())
            {
                if (bus.PluggedIn)
                bus.Update();
            }
        }

        public void SetModifier(int value)
        {
            XInputBus.Modifier = value;
        }

        public void SubscribeToRumble(XInput_Device device, RumbleChangeDelegate callback)
        {
            _xInstances[(int)device - 1].RumbleEvent += callback;
        }

        public void UnSubscribeToRumble(XInput_Device device, RumbleChangeDelegate callback)
        {
            _xInstances[(int)device - 1].RumbleEvent -= callback;
        }

        public enum XInput_Device : int
        {
            Device_A = 1,
            Device_B = 2,
            Device_C = 3,
            Device_D = 4
        }

        public struct XInputState
        {
            public bool A { get; set; }
            public bool B { get; set; }
            public bool X { get; set; }
            public bool Y { get; set; }
            public bool Up { get; set; }
            public bool Down { get; set; }
            public bool Left { get; set; }
            public bool Right { get; set; }
            public bool LB { get; set; }
            public bool RB { get; set; }
            public bool LS { get; set; }
            public bool RS { get; set; }
            public bool Start { get; set; }
            public bool Back { get; set; }
            public bool Guide { get; set; }

            public float LX { get; set; }
            public float LY { get; set; }
            public float LT { get; set; }
            public float RX { get; set; }
            public float RY { get; set; }
            public float RT { get; set; }

        public void Reset()
            {
                A = B = X = Y = false;
                Up = Down = Left = Right = false;
                LB = RB = LS = RS = false;
                Start = Back = Guide = false;
                LX = LY = LT = 0;
                RX = RY = RT = 0;
            }
        }

        protected class BusAccess
        {
            public static BusAccess Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new BusAccess();
                    }
                    return _instance;
                }
            }

            public static BusAccess _instance;
            private ViGEmClient viGEmClient;
            private Dictionary<int, IXbox360Controller> targets;
            private List<IXbox360Controller> connected;

            public ViGEmClient Client
            {
                get
                {
                    return viGEmClient;
                }
                private set
                {
                    viGEmClient = value;
                }
            }

            protected BusAccess()
            {
                Client = new ViGEmClient();
                targets = new Dictionary<int, IXbox360Controller>();
                connected = new List<IXbox360Controller>();
                App.Current.Exit += App_Exit;
        }

            private void App_Exit(object sender, System.Windows.ExitEventArgs e)
            {
                if (_instance != null)
        {
                    foreach (IXbox360Controller controller in targets.Values)
                    {
                        if (connected.Contains(controller))
                        {
                            controller.Disconnect();
                            connected.Remove(controller);
                        }
                    }
                    Client.Dispose();
                }
            }
            public bool Plugin(int id, ushort vid = 0, ushort pid = 0)
            {
                id -= 1;
                if (vid != 0 && pid != 0)
                {
                    targets.Add(id, Client.CreateXbox360Controller(vid, pid));
                }
                else
                {
                    targets.Add(id, Client.CreateXbox360Controller());
                }
                targets[id].AutoSubmitReport = false;
                targets[id].Connect();
                connected.Add(targets[id]);
                return true;
            }
            public bool Unplug(int id)
            {
                id -= 1;
                if (targets.Count > id && targets[id] != null)
                {
                    if (connected.Contains(targets[id]))
                    {
                        targets[id].Disconnect();
                        connected.Remove(targets[id]);
                        targets.Remove(id);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            public IXbox360Controller GetController(int id)
            {
                if (!targets.TryGetValue(id, out var controller))
                {
                    return null;
                }
                return controller;
            }

            internal bool Report(byte[] output, byte[] rumble)
            {
                throw new NotImplementedException(); //todo
            }
        }

        protected class XInputBus
        {
            public static int Modifier;

            public XInputState inputs;
            public event RumbleChangeDelegate RumbleEvent;

            public int ID
            {
                get { return _id + Modifier; }
                protected set { _id = value; }
            }
            public bool PluggedIn { get; protected set; }

            protected BusAccess busRef;

            private int _id;
            private float tempLX = -10;
            private float tempLY = -10;
            private float tempRX = -10;
            private float tempRY = -10;

            public XInputBus(int id)
            {
                inputs = new XInputState();
                ID = id;
                busRef = BusAccess.Instance;
            }

            public bool Connect()
            {
                if (!PluggedIn)
                {
                    busRef.Unplug(ID);
                    PluggedIn = busRef.Plugin(ID);
                }

                return PluggedIn;
            }

            public bool Disconnect()
            {
                if (PluggedIn)
                {
                    PluggedIn = !busRef.Unplug(ID);
                    RumbleEvent?.Invoke(0, 0);
                }

                return PluggedIn == false;
            }

            public void SetInput(Xbox360Button button, bool state)
            {
                if (button.Equals(Xbox360Button.A)) { inputs.A = state; }
                else if (button.Equals(Xbox360Button.B)) { inputs.B = state; }
                else if (button.Equals(Xbox360Button.X)) { inputs.X = state; }
                else if (button.Equals(Xbox360Button.Y)) { inputs.Y = state; }
                else if (button.Equals(Xbox360Button.Up)) { inputs.Up = state; }
                else if (button.Equals(Xbox360Button.Down)) { inputs.Down = state; }
                else if (button.Equals(Xbox360Button.Left)) { inputs.Left = state; }
                else if (button.Equals(Xbox360Button.Right)) { inputs.Right = state; }
                else if (button.Equals(Xbox360Button.LeftShoulder)) { inputs.LB = state; }
                else if (button.Equals(Xbox360Button.RightShoulder)) { inputs.RB = state; }
                else if (button.Equals(Xbox360Button.LeftThumb)) { inputs.LS = state; }
                else if (button.Equals(Xbox360Button.RightThumb)) { inputs.RS = state; }
                else if (button.Equals(Xbox360Button.Start)) { inputs.Start = state; }
                else if (button.Equals(Xbox360Button.Back)) { inputs.Back = state; }
                else if (button.Equals(Xbox360Button.Guide)) { inputs.Guide = state; }
            }

            public void SetInput(Xbox360Axis axis, float value)
            {
                if (axis.Equals(Xbox360Axis.LeftThumbX))
                {
                    if (value > tempLX)
                    {
                        tempLX = value;
                    }
                    inputs.LX = value;
                }
                else if (axis.Equals(Xbox360Axis.LeftThumbY))
                {
                    if (value > tempLY)
                    {
                        tempLY = value;
                    }
                    inputs.LY = value;
                }
                else if (axis.Equals(Xbox360Axis.RightThumbX))
                {
                    if (value > tempRX)
                    {
                        tempRX = value;
                    }
                    inputs.RX = value;
                }
                else if (axis.Equals(Xbox360Axis.RightThumbY))
                {
                    if (value > tempRY)
                    {
                        tempRY = value;
                    }
                    inputs.RY = value;
                }
            }

            public void SetInput(Xbox360Slider slider, float value)
            {
                if (slider.Equals(Xbox360Slider.LeftTrigger)) {  inputs.LT = value; }
                else if (slider.Equals(Xbox360Slider.RightTrigger)) { inputs.RT = value; }
            }

            public void Update()
            {
                //if (!Started) return;

                // reset temps
                tempLX = -10;
                tempLY = -10;
                tempRX = -10;
                tempRY = -10;

                var controller = busRef.GetController(ID);
                if (controller == null)
                {
                    return;
                }

                controller.SetButtonState(Xbox360Button.A, inputs.A);
                controller.SetButtonState(Xbox360Button.B, inputs.B);
                controller.SetButtonState(Xbox360Button.X, inputs.X);
                controller.SetButtonState(Xbox360Button.Y, inputs.Y);

                controller.SetButtonState(Xbox360Button.Up, inputs.Up);
                controller.SetButtonState(Xbox360Button.Down, inputs.Down);
                controller.SetButtonState(Xbox360Button.Left, inputs.Left);
                controller.SetButtonState(Xbox360Button.Right, inputs.Right);

                controller.SetButtonState(Xbox360Button.LeftShoulder, inputs.LB);
                controller.SetButtonState(Xbox360Button.RightShoulder, inputs.RB);
                controller.SetButtonState(Xbox360Button.LeftThumb, inputs.LS);
                controller.SetButtonState(Xbox360Button.RightThumb, inputs.RS);

                controller.SetButtonState(Xbox360Button.Start, inputs.Start);
                controller.SetButtonState(Xbox360Button.Back, inputs.Back);
                controller.SetButtonState(Xbox360Button.Guide, inputs.Guide);

                controller.SetAxisValue(Xbox360Axis.LeftThumbX, GetRawAxis(inputs.LX));
                controller.SetAxisValue(Xbox360Axis.LeftThumbY, GetRawAxis(inputs.LY));
                controller.SetAxisValue(Xbox360Axis.RightThumbX, GetRawAxis(inputs.RX));
                controller.SetAxisValue(Xbox360Axis.RightThumbY, GetRawAxis(inputs.RY));

                controller.SetSliderValue(Xbox360Slider.LeftTrigger, GetRawTrigger(inputs.LT));
                controller.SetSliderValue(Xbox360Slider.RightTrigger, GetRawTrigger(inputs.RT));

                controller.SubmitReport();
            }
            private void OnRumble(object sender, Xbox360FeedbackReceivedEventArgs args) //todo
            {
                int strength = (args.LargeMotor << 8) | args.SmallMotor;
                //Flags[Inputs.Flags.RUMBLE] = strength > minRumble;
                //RumbleAmount = strength > minRumble ? strength : 0;
            }

            public short GetRawAxis(float axis)
            {
                if (axis > 1.0f)
                {
                    return 32767;
                }
                if (axis < -1.0f)
                {
                    return -32767;
                }

                return (short)(axis * 32767);
            }

            public byte GetRawTrigger(float trigger)
            {
                if (trigger > 1.0f)
                {
                    return 0xFF;
                }
                if (trigger < 0.0f)
                {
                    return 0;
                }

                return (byte)(trigger * 0xFF);
            }
        }
    }
}
