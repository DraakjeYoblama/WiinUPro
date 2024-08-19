using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace WiinUPro
{
    public class XInputSliderAssignment : IAssignment
    {
        /// <summary>
        /// The XInput Device to use
        /// </summary>
        public ScpDirector.XInput_Device Device { get; set; }

        /// <summary>
        /// The XInput Slider to be simulated.
        /// </summary>
        public Xbox360Slider Slider { get; set; }

        public XInputSliderAssignment() { }

        public XInputSliderAssignment(Xbox360Slider slider, ScpDirector.XInput_Device device = ScpDirector.XInput_Device.Device_A)
        {
            Slider = slider;
            Device = device;
        }

        public void Apply(float value)
        {
            ScpDirector.Access.SetSlider(Slider, value, Device);
        }

        public bool SameAs(IAssignment assignment)
        {
            var other = assignment as XInputSliderAssignment;

            if (other == null)
            {
                return false;
            }

            bool result = true;

            result &= Slider == other.Slider;
            result &= Device == other.Device;

            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as XInputSliderAssignment;

            if (other == null)
            {
                return false;
            }
            else
            {
                return Slider == other.Slider && Device == other.Device;
            }
        }

        public override int GetHashCode()
        {
            int hash = Slider.Id + (int)Device;
            return hash;
        }

        public override string ToString()
        {
            return Slider.ToString();
        }

        public string GetDisplayName()
        {
            return $"X{ToString()}";
        }
    }
}
