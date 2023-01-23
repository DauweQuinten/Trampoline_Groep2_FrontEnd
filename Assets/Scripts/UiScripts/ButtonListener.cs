using Models;
using Newtonsoft.Json;
using UnityEngine;

namespace UiScripts
{
    public static class ButtonListener
    {
        public static BtnValue Left;
        public static BtnValue Right;
        public static BtnValue Both;
        public static int BtnUpdate;

        public static void ListenToButtons()
        {
            // link to socketController Events
            var socketObject = GameObject.Find("SocketController");
            var events = socketObject.GetComponent<SocketEvents>();
            events.btnPressedLeft2.AddListener(LeftMoveDown);
            events.btnPressedLeft.AddListener(LeftMoveDown2);
            events.btnPressedRight.AddListener(RightMoveDown);
            events.btnReleasedLeft.AddListener(LeftMoveUp);
            events.btnReleasedRight.AddListener(RightMoveUp);
            events.btnPressedBoth.AddListener(BothButtonsOnClicked);
            events.btnReleasedBoth.AddListener(BothButtonsOnReleased);
        }


        public static void UpdateLed(LedType type, LedValue state)
        {
            // todo: add led events to socketController
            var socketObject = GameObject.Find("SocketController");
            var events = socketObject.GetComponent<SocketEvents>();
            switch (type, state)
            {
                case (LedType.Left, LedValue.On):
                    // events.ledLeftOn.Invoke();
                    break;
                case (LedType.Left, LedValue.Off):
                    // events.ledLeftOff.Invoke();
                    break;
                case (LedType.Right, LedValue.On):
                    // events.ledRightOn.Invoke();
                    break;
                case (LedType.Right, LedValue.Off):
                    // events.ledRightOff.Invoke();
                    break;
            }
        }

        public enum LedType
        {
            Left,
            Right
        }
        public enum LedValue
        {
            [JsonProperty("ON")] On,
            [JsonProperty("OFF")] Off
        }
        

        private static void LeftMoveDown2(Color arg0)
        {
            Left = BtnValue.Pressed;
            BtnUpdate ++;
        }

        private static void RightMoveUp()
        {
            Right = BtnValue.Released;
            BtnUpdate ++;
        }

        private static void LeftMoveUp()
        {
            Left = BtnValue.Released;
            BtnUpdate ++;
        }

        private static void RightMoveDown()
        {
            Right = BtnValue.Pressed;
            BtnUpdate ++;
        }

        private static void LeftMoveDown()
        {
            Left = BtnValue.Pressed;
            BtnUpdate ++;
        }

        private static void BothButtonsOnClicked()
        {
            Both = BtnValue.Pressed;
            BtnUpdate ++;
        }

        private static void BothButtonsOnReleased()
        {
            Both = BtnValue.Released;
            BtnUpdate ++;
        }
    }
}