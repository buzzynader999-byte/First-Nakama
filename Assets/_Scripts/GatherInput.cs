
namespace _Scripts
{
    public static class GatherInput
    {
        private static InputSystem_Actions _controll;
        public static InputSystem_Actions Controll
        {
            get => _controll;
            private set => _controll = value;
        }

        static GatherInput()
        {   
            Controll = new InputSystem_Actions();
            Controll.Enable();
        }
    }
}