using System;
using System.Collections.Generic;
using System.Linq;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using NintrollerLib;

namespace WiinUSoft.Holders
{
    public class XInputHolder : Holder
    {
        static internal bool[] availabe = { true, true, true, true };

        internal int minRumble = 20;
        internal int rumbleLeft = 0;
        internal int rumbleDecrement = 10;

        private XBus bus;
        private bool connected;
        private int ID;
        private Dictionary<string, float> writeReport;

        public static Dictionary<string, string> GetDefaultMapping(ControllerType type)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // TODO: finish default mapping (Acc, IR, Balance Board, ect) (not for 1st release)
            switch (type)
            {
                case ControllerType.ProController:
                    result.Add(Inputs.ProController.A,      Inputs.Xbox360.A);
                    result.Add(Inputs.ProController.B,      Inputs.Xbox360.B);
                    result.Add(Inputs.ProController.X,      Inputs.Xbox360.X);
                    result.Add(Inputs.ProController.Y,      Inputs.Xbox360.Y);

                    result.Add(Inputs.ProController.UP,     Inputs.Xbox360.UP);
                    result.Add(Inputs.ProController.DOWN,   Inputs.Xbox360.DOWN);
                    result.Add(Inputs.ProController.LEFT,   Inputs.Xbox360.LEFT);
                    result.Add(Inputs.ProController.RIGHT,  Inputs.Xbox360.RIGHT);

                    result.Add(Inputs.ProController.L,      Inputs.Xbox360.LB);
                    result.Add(Inputs.ProController.R,      Inputs.Xbox360.RB);
                    result.Add(Inputs.ProController.ZL,     Inputs.Xbox360.LT);
                    result.Add(Inputs.ProController.ZR,     Inputs.Xbox360.RT);

                    result.Add(Inputs.ProController.LUP,    Inputs.Xbox360.LUP);
                    result.Add(Inputs.ProController.LDOWN,  Inputs.Xbox360.LDOWN);
                    result.Add(Inputs.ProController.LLEFT,  Inputs.Xbox360.LLEFT);
                    result.Add(Inputs.ProController.LRIGHT, Inputs.Xbox360.LRIGHT);

                    result.Add(Inputs.ProController.RUP,    Inputs.Xbox360.RUP);
                    result.Add(Inputs.ProController.RDOWN,  Inputs.Xbox360.RDOWN);
                    result.Add(Inputs.ProController.RLEFT,  Inputs.Xbox360.RLEFT);
                    result.Add(Inputs.ProController.RRIGHT, Inputs.Xbox360.RRIGHT);

                    result.Add(Inputs.ProController.LS,     Inputs.Xbox360.LS);
                    result.Add(Inputs.ProController.RS,     Inputs.Xbox360.RS);
                    result.Add(Inputs.ProController.SELECT, Inputs.Xbox360.BACK);
                    result.Add(Inputs.ProController.START,  Inputs.Xbox360.START);
                    result.Add(Inputs.ProController.HOME,   Inputs.Xbox360.GUIDE);
                    break;

                case ControllerType.ClassicControllerPro:
                    result.Add(Inputs.ClassicControllerPro.A,      Inputs.Xbox360.A);
                    result.Add(Inputs.ClassicControllerPro.B,      Inputs.Xbox360.B);
                    result.Add(Inputs.ClassicControllerPro.X,      Inputs.Xbox360.X);
                    result.Add(Inputs.ClassicControllerPro.Y,      Inputs.Xbox360.Y);

                    result.Add(Inputs.ClassicControllerPro.UP,     Inputs.Xbox360.UP);
                    result.Add(Inputs.ClassicControllerPro.DOWN,   Inputs.Xbox360.DOWN);
                    result.Add(Inputs.ClassicControllerPro.LEFT,   Inputs.Xbox360.LEFT);
                    result.Add(Inputs.ClassicControllerPro.RIGHT,  Inputs.Xbox360.RIGHT);
                                      
                    result.Add(Inputs.ClassicControllerPro.L,      Inputs.Xbox360.LB);
                    result.Add(Inputs.ClassicControllerPro.R,      Inputs.Xbox360.RB);
                    result.Add(Inputs.ClassicControllerPro.ZL,     Inputs.Xbox360.LT);
                    result.Add(Inputs.ClassicControllerPro.ZR,     Inputs.Xbox360.RT);
                                      
                    result.Add(Inputs.ClassicControllerPro.LUP,    Inputs.Xbox360.LUP);
                    result.Add(Inputs.ClassicControllerPro.LDOWN,  Inputs.Xbox360.LDOWN);
                    result.Add(Inputs.ClassicControllerPro.LLEFT,  Inputs.Xbox360.LLEFT);
                    result.Add(Inputs.ClassicControllerPro.LRIGHT, Inputs.Xbox360.LRIGHT);
                                      
                    result.Add(Inputs.ClassicControllerPro.RUP,    Inputs.Xbox360.RUP);
                    result.Add(Inputs.ClassicControllerPro.RDOWN,  Inputs.Xbox360.RDOWN);
                    result.Add(Inputs.ClassicControllerPro.RLEFT,  Inputs.Xbox360.RLEFT);
                    result.Add(Inputs.ClassicControllerPro.RRIGHT, Inputs.Xbox360.RRIGHT);
                                      
                    result.Add(Inputs.ClassicControllerPro.SELECT, Inputs.Xbox360.BACK);
                    result.Add(Inputs.ClassicControllerPro.START,  Inputs.Xbox360.START);
                    result.Add(Inputs.ClassicControllerPro.HOME,   Inputs.Xbox360.GUIDE);

                    result.Add(Inputs.Wiimote.UP,    Inputs.Xbox360.UP);
                    result.Add(Inputs.Wiimote.DOWN,  Inputs.Xbox360.DOWN);
                    result.Add(Inputs.Wiimote.LEFT,  Inputs.Xbox360.LEFT);
                    result.Add(Inputs.Wiimote.RIGHT, Inputs.Xbox360.RIGHT);
                    result.Add(Inputs.Wiimote.A,     Inputs.Xbox360.A);
                    result.Add(Inputs.Wiimote.B,     Inputs.Xbox360.B);
                    result.Add(Inputs.Wiimote.ONE,   Inputs.Xbox360.LS);
                    result.Add(Inputs.Wiimote.TWO,   Inputs.Xbox360.RS);
                    result.Add(Inputs.Wiimote.PLUS,  Inputs.Xbox360.BACK);
                    result.Add(Inputs.Wiimote.MINUS, Inputs.Xbox360.START);
                    result.Add(Inputs.Wiimote.HOME,  Inputs.Xbox360.GUIDE);
                    result.Add(Inputs.Wiimote.ACC_SHAKE_X, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Y, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Z, "");
                    result.Add(Inputs.Wiimote.TILT_RIGHT, "");
                    result.Add(Inputs.Wiimote.TILT_LEFT, "");
                    result.Add(Inputs.Wiimote.TILT_UP, "");
                    result.Add(Inputs.Wiimote.TILT_DOWN, "");
                    break;

                case ControllerType.ClassicController:
                    result.Add(Inputs.ClassicController.B,      Inputs.Xbox360.B);
                    result.Add(Inputs.ClassicController.A,      Inputs.Xbox360.A);
                    result.Add(Inputs.ClassicController.Y,      Inputs.Xbox360.X);
                    result.Add(Inputs.ClassicController.X,      Inputs.Xbox360.Y);

                    result.Add(Inputs.ClassicController.UP,     Inputs.Xbox360.UP);
                    result.Add(Inputs.ClassicController.DOWN,   Inputs.Xbox360.DOWN);
                    result.Add(Inputs.ClassicController.LEFT,   Inputs.Xbox360.LEFT);
                    result.Add(Inputs.ClassicController.RIGHT,  Inputs.Xbox360.RIGHT);

                    result.Add(Inputs.ClassicController.ZL,     Inputs.Xbox360.LB);
                    result.Add(Inputs.ClassicController.ZR,     Inputs.Xbox360.RB);
                    result.Add(Inputs.ClassicController.LT,     Inputs.Xbox360.LT);
                    result.Add(Inputs.ClassicController.RT,     Inputs.Xbox360.RT);

                    result.Add(Inputs.ClassicController.LUP,    Inputs.Xbox360.LUP);
                    result.Add(Inputs.ClassicController.LDOWN,  Inputs.Xbox360.LDOWN);
                    result.Add(Inputs.ClassicController.LLEFT,  Inputs.Xbox360.LLEFT);
                    result.Add(Inputs.ClassicController.LRIGHT, Inputs.Xbox360.LRIGHT);

                    result.Add(Inputs.ClassicController.RUP,    Inputs.Xbox360.RUP);
                    result.Add(Inputs.ClassicController.RDOWN,  Inputs.Xbox360.RDOWN);
                    result.Add(Inputs.ClassicController.RLEFT,  Inputs.Xbox360.RLEFT);
                    result.Add(Inputs.ClassicController.RRIGHT, Inputs.Xbox360.RRIGHT);

                    result.Add(Inputs.ClassicController.SELECT, Inputs.Xbox360.BACK);
                    result.Add(Inputs.ClassicController.START,  Inputs.Xbox360.START);
                    result.Add(Inputs.ClassicController.HOME,   Inputs.Xbox360.GUIDE);

                    result.Add(Inputs.Wiimote.UP,    Inputs.Xbox360.UP);
                    result.Add(Inputs.Wiimote.DOWN,  Inputs.Xbox360.DOWN);
                    result.Add(Inputs.Wiimote.LEFT,  Inputs.Xbox360.LEFT);
                    result.Add(Inputs.Wiimote.RIGHT, Inputs.Xbox360.RIGHT);
                    result.Add(Inputs.Wiimote.A,     Inputs.Xbox360.A);
                    result.Add(Inputs.Wiimote.B,     Inputs.Xbox360.B);
                    result.Add(Inputs.Wiimote.ONE,   Inputs.Xbox360.LS);
                    result.Add(Inputs.Wiimote.TWO,   Inputs.Xbox360.RS);
                    result.Add(Inputs.Wiimote.PLUS,  Inputs.Xbox360.BACK);
                    result.Add(Inputs.Wiimote.MINUS, Inputs.Xbox360.START);
                    result.Add(Inputs.Wiimote.HOME,  Inputs.Xbox360.GUIDE);
                    result.Add(Inputs.Wiimote.ACC_SHAKE_X, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Y, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Z, "");
                    result.Add(Inputs.Wiimote.TILT_RIGHT, "");
                    result.Add(Inputs.Wiimote.TILT_LEFT, "");
                    result.Add(Inputs.Wiimote.TILT_UP, "");
                    result.Add(Inputs.Wiimote.TILT_DOWN, "");
                    break;

                case ControllerType.Nunchuk:
                case ControllerType.NunchukB:
                    result.Add(Inputs.Nunchuk.UP,    Inputs.Xbox360.LUP);
                    result.Add(Inputs.Nunchuk.DOWN,  Inputs.Xbox360.LDOWN);
                    result.Add(Inputs.Nunchuk.LEFT,  Inputs.Xbox360.LLEFT);
                    result.Add(Inputs.Nunchuk.RIGHT, Inputs.Xbox360.LRIGHT);
                    result.Add(Inputs.Nunchuk.Z,     Inputs.Xbox360.RT);
                    result.Add(Inputs.Nunchuk.C,     Inputs.Xbox360.LT);
                    result.Add(Inputs.Nunchuk.TILT_RIGHT, "");
                    result.Add(Inputs.Nunchuk.TILT_LEFT, "");
                    result.Add(Inputs.Nunchuk.TILT_UP, "");
                    result.Add(Inputs.Nunchuk.TILT_DOWN, "");
                    result.Add(Inputs.Nunchuk.ACC_SHAKE_X, "");
                    result.Add(Inputs.Nunchuk.ACC_SHAKE_Y, "");
                    result.Add(Inputs.Nunchuk.ACC_SHAKE_Z, "");

                    result.Add(Inputs.Wiimote.UP,    Inputs.Xbox360.UP);
                    result.Add(Inputs.Wiimote.DOWN,  Inputs.Xbox360.DOWN);
                    result.Add(Inputs.Wiimote.LEFT,  Inputs.Xbox360.LB);
                    result.Add(Inputs.Wiimote.RIGHT, Inputs.Xbox360.RB);
                    result.Add(Inputs.Wiimote.A,     Inputs.Xbox360.A);
                    result.Add(Inputs.Wiimote.B,     Inputs.Xbox360.B);
                    result.Add(Inputs.Wiimote.ONE,   Inputs.Xbox360.X);
                    result.Add(Inputs.Wiimote.TWO,   Inputs.Xbox360.Y);
                    result.Add(Inputs.Wiimote.PLUS,  Inputs.Xbox360.BACK);
                    result.Add(Inputs.Wiimote.MINUS, Inputs.Xbox360.START);
                    result.Add(Inputs.Wiimote.HOME,  Inputs.Xbox360.GUIDE);
                    result.Add(Inputs.Wiimote.ACC_SHAKE_X, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Y, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Z, "");
                    result.Add(Inputs.Wiimote.TILT_RIGHT, "");
                    result.Add(Inputs.Wiimote.TILT_LEFT, "");
                    result.Add(Inputs.Wiimote.TILT_UP, "");
                    result.Add(Inputs.Wiimote.TILT_DOWN, "");
                    break;

                case ControllerType.Wiimote:
                    result.Add(Inputs.Wiimote.UP,    Inputs.Xbox360.LEFT);
                    result.Add(Inputs.Wiimote.DOWN,  Inputs.Xbox360.RIGHT);
                    result.Add(Inputs.Wiimote.LEFT,  Inputs.Xbox360.DOWN);
                    result.Add(Inputs.Wiimote.RIGHT, Inputs.Xbox360.UP);
                    result.Add(Inputs.Wiimote.A,     Inputs.Xbox360.X);
                    result.Add(Inputs.Wiimote.B,     Inputs.Xbox360.Y);
                    result.Add(Inputs.Wiimote.ONE,   Inputs.Xbox360.A);
                    result.Add(Inputs.Wiimote.TWO,   Inputs.Xbox360.B);
                    result.Add(Inputs.Wiimote.PLUS,  Inputs.Xbox360.BACK);
                    result.Add(Inputs.Wiimote.MINUS, Inputs.Xbox360.START);
                    result.Add(Inputs.Wiimote.HOME,  Inputs.Xbox360.GUIDE);
                    result.Add(Inputs.Wiimote.ACC_SHAKE_X, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Y, "");
                    result.Add(Inputs.Wiimote.ACC_SHAKE_Z, "");
                    result.Add(Inputs.Wiimote.TILT_RIGHT, "");
                    result.Add(Inputs.Wiimote.TILT_LEFT, "");
                    result.Add(Inputs.Wiimote.TILT_UP, "");
                    result.Add(Inputs.Wiimote.TILT_DOWN, "");
                    result.Add(Inputs.Wiimote.IR_RIGHT, "");
                    result.Add(Inputs.Wiimote.IR_LEFT, "");
                    result.Add(Inputs.Wiimote.IR_UP, "");
                    result.Add(Inputs.Wiimote.IR_DOWN, "");
                    break;
            }

            return result;
        }

        public XInputHolder()
        {
#if MouseMode
            _inputSim = new WindowsInput.InputSimulator();
#endif
            //Values = new Dictionary<string, float>();
            Values = new System.Collections.Concurrent.ConcurrentDictionary<string, float>();
            Mappings = new Dictionary<string, string>();
            Flags = new Dictionary<string, bool>();
            ResetReport();

            //if (!Flags.ContainsKey(Inputs.Flags.RUMBLE))
            //{
            //    Flags.Add(Inputs.Flags.RUMBLE, false);
            //}
            //
            //if (!Values.ContainsKey(Inputs.Flags.RUMBLE))
            //{
            //    Values.TryAdd(Inputs.Flags.RUMBLE, 0f);
            //}
        }

        private void ResetReport()
        {
            writeReport = new Dictionary<string, float>()
            {
                {Inputs.Xbox360.A, 0},
                {Inputs.Xbox360.B, 0},
                {Inputs.Xbox360.X, 0},
                {Inputs.Xbox360.Y, 0},
                {Inputs.Xbox360.UP, 0},
                {Inputs.Xbox360.DOWN, 0},
                {Inputs.Xbox360.LEFT, 0},
                {Inputs.Xbox360.RIGHT, 0},
                {Inputs.Xbox360.LB, 0},
                {Inputs.Xbox360.RB, 0},
                {Inputs.Xbox360.BACK, 0},
                {Inputs.Xbox360.START, 0},
                {Inputs.Xbox360.GUIDE, 0},
                {Inputs.Xbox360.LS, 0},
                {Inputs.Xbox360.RS, 0},
            };
        }

        public XInputHolder(ControllerType t) : this()
        {
            Mappings = GetDefaultMapping(t);
        }

        public override void Update()
        {
            if (!connected)
            {
                return;
            }

#if MouseMode
            if (InMouseMode)
            {
                UpdateMouseMode();
                return;
            }
#endif

            var controller = bus.GetController(ID);
            if (controller == null)
            {
                return;
            }

            float LX = 0f;
            float LY = 0f;
            float RX = 0f;
            float RY = 0f;

            float LT = 0f;
            float RT = 0f;

            ResetReport();

            //foreach (KeyValuePair<string, string> map in Mappings)
            for (int i = 0; i < Mappings.Count; i++)
            {
                var map = Mappings.ElementAt(i);

                if (!Values.ContainsKey(map.Key))
                {
                    continue;
                }

                if (writeReport.ContainsKey(map.Value))
                {
                    try
                    {
                        writeReport[map.Value] += Values[map.Key];
                    }
                    catch(KeyNotFoundException) { }
                }
                else if (Values.ContainsKey(map.Key))
                {
                    switch (map.Value)
                    {
                        case Inputs.Xbox360.LLEFT : try { LX -= Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.LRIGHT: try { LX += Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.LUP   : try { LY += Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.LDOWN : try { LY -= Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.RLEFT : try { RX -= Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.RRIGHT: try { RX += Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.RUP   : try { RY += Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.RDOWN : try { RY -= Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.LT    : try { LT += Values[map.Key]; } catch { } break;
                        case Inputs.Xbox360.RT    : try { RT += Values[map.Key]; } catch { } break;

#if MouseMode
                        case "MouseMode": 
                            if (Values[map.Key] > 0f && DateTime.Now.Subtract(_mmLastTime).TotalSeconds > 3)
                            {
                                _mmLastTime = DateTime.Now;
                                InMouseMode = true;
                            }
                            break;
#endif
                    }
                }
            }

                        controller.SetButtonState(Xbox360Button.A, writeReport[Inputs.Xbox360.A] > 0f);
            controller.SetButtonState(Xbox360Button.B, writeReport[Inputs.Xbox360.B] > 0f);
            controller.SetButtonState(Xbox360Button.X, writeReport[Inputs.Xbox360.X] > 0f);
            controller.SetButtonState(Xbox360Button.Y, writeReport[Inputs.Xbox360.Y] > 0f);

            controller.SetButtonState(Xbox360Button.Up, writeReport[Inputs.Xbox360.UP] > 0f);
            controller.SetButtonState(Xbox360Button.Down, writeReport[Inputs.Xbox360.DOWN] > 0f);
            controller.SetButtonState(Xbox360Button.Left, writeReport[Inputs.Xbox360.LEFT] > 0f);
            controller.SetButtonState(Xbox360Button.Right, writeReport[Inputs.Xbox360.RIGHT] > 0f);

            controller.SetButtonState(Xbox360Button.LeftShoulder, writeReport[Inputs.Xbox360.LB] > 0f);
            controller.SetButtonState(Xbox360Button.RightShoulder, writeReport[Inputs.Xbox360.RB] > 0f);
            controller.SetButtonState(Xbox360Button.LeftThumb, writeReport[Inputs.Xbox360.LS] > 0f);
            controller.SetButtonState(Xbox360Button.RightThumb, writeReport[Inputs.Xbox360.RS] > 0f);

            controller.SetButtonState(Xbox360Button.Start, writeReport[Inputs.Xbox360.START] > 0f);
            controller.SetButtonState(Xbox360Button.Back, writeReport[Inputs.Xbox360.BACK] > 0f);
            controller.SetButtonState(Xbox360Button.Guide, writeReport[Inputs.Xbox360.GUIDE] > 0f);

            controller.SetAxisValue(Xbox360Axis.LeftThumbX, GetRawAxis(LX));
            controller.SetAxisValue(Xbox360Axis.LeftThumbY, GetRawAxis(LY));
            controller.SetAxisValue(Xbox360Axis.RightThumbX, GetRawAxis(RX));
            controller.SetAxisValue(Xbox360Axis.RightThumbY, GetRawAxis(RY));

            controller.SetSliderValue(Xbox360Slider.LeftTrigger, GetRawTrigger(LT));
            controller.SetSliderValue(Xbox360Slider.RightTrigger, GetRawTrigger(RT));

            controller.SubmitReport();
        }

        private void OnRumble(object sender, Xbox360FeedbackReceivedEventArgs args)
        {
            int strength = (args.LargeMotor << 8) | args.SmallMotor;
            Flags[Inputs.Flags.RUMBLE] = strength > minRumble;
            RumbleAmount = strength > minRumble ? strength : 0;
        }

        public override void Close()
        {
            Flags[Inputs.Flags.RUMBLE] = false;
            bus.Unplug(ID);
            
            if (ID > 0 && ID < 5)
            {
                availabe[ID - 1] = true;
            }

            ID = 0;
            connected = false;
        }

        public override void AddMapping(ControllerType controller)
        {
            var additional = GetDefaultMapping(controller);

            foreach (KeyValuePair<string, string> map in additional)
            {
                if (!Mappings.ContainsKey(map.Key) && map.Key[0] != 'w')
                {
                    SetMapping(map.Key, map.Value);
                }
            }
        }

        public bool ConnectXInput(int id)
        {
            if (id > 0 && id < 5)
            {
                availabe[id - 1] = false;
            }
            else
            {
                return false;
            }

            bus = XBus.Default;
            bus.Unplug(id);
            bus.Plugin(id);
            var controller = bus.GetController(id);
            if (controller == null)
            {
                RemoveXInput(id);
                return false;
            }
            controller.FeedbackReceived += OnRumble;
            ID = id;
            connected = true;
            return true;
        }

        public bool RemoveXInput(int id)
        {
            if (id > 0 && id < 5)
            {
                availabe[id - 1] = true;
            }


            Flags[Inputs.Flags.RUMBLE] = false;
            if (bus.Unplug(id))
            {
                ID = 0;
                connected = false;
                return true;
            }

            return false;
        }

        public short GetRawAxis(double axis)
        {
            if (axis > 1.0)
            {
                return 32767;
            }
            if (axis < -1.0)
            {
                return -32767;
            }

            return (short)(axis * 32767);
        }

        public byte GetRawTrigger(double trigger)
        {
            if (trigger > 1.0)
            {
                return 255;
            }
            if (trigger < 0.0)
            {
                return 0;
            }

            return (Byte)(trigger * 255);
        }
    }

    public class XBus
    {
        private static XBus defaultInstance;
        private ViGEmClient viGEmClient;
        private Dictionary<int, IXbox360Controller> targets;
        private List<IXbox360Controller> connected;

        // Default Bus
        public static XBus Default
        {
            get
            {
                // if it hasn't been created create one
                if (defaultInstance == null)
                {
                    defaultInstance = new XBus();
                }

                return defaultInstance;
            }
        }

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

        public XBus() 
        {
            Client = new ViGEmClient();
            targets = new Dictionary<int, IXbox360Controller>();
            connected = new List<IXbox360Controller>();
            App.Current.Exit += StopDevice;
        }

        private void StopDevice(object sender, System.Windows.ExitEventArgs e)
        {
            if (defaultInstance != null)
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

         public void Plugin(int id, ushort vid = 0, ushort pid = 0)
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
    }
}
