
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Editor.Engine
{
    internal class InputController
    {
        // Create one shared InputController for the whole program
        private static readonly Lazy<InputController> lazy = new(() => new InputController());
        public static InputController Instance { get { return lazy.Value; } }

        // Store whether each keyboard key and mouse button is pressed
        private Dictionary<Keys, bool> m_keyState = new();
        private Dictionary<MouseButtons, bool> m_buttonState = new();

        private InputController()
        {
            // Add all keyboard keys and set them to not pressed
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (m_keyState.ContainsKey(key)) continue;
                m_keyState.Add(key, false);
            }

            // Add all mouse buttons and set them to not pressed
            foreach (MouseButtons button in Enum.GetValues(typeof(MouseButtons)))
            {
                if (m_buttonState.ContainsKey(button)) continue;
                m_buttonState.Add(button, false);
            }
        }

        // Mark a keyboard key as pressed
        public void SetKeyDown(Keys _key)
        {
            m_keyState[_key] = true;
        }

        // Mark a keyboard key as released
        public void SetKeyUp(Keys _key)
        {
            m_keyState[_key] = false;
        }

        // Check if a keyboard key is currently pressed
        public bool IsKeyDown(Keys _key)
        {
            return m_keyState[_key];
        }

        // Mark a mouse button as pressed
        public void SetButtonDown(MouseButtons _button)
        {
            m_buttonState[_button] = true;
        }

        // Mark a mouse button as released
        public void SetButtonUp(MouseButtons _button)
        {
            m_buttonState[_button] = false;
        }

        // Check if a mouse button is currently pressed
        public bool IsButtonDown(MouseButtons _button)
        {
            return m_buttonState[_button];
        }

        // Return a list of all currently pressed keys and buttons
        public override string ToString()
        {
            string s = "Keys Down: ";

            // Add the names of pressed keyboard keys
            foreach (var key in m_keyState)
            {
                if (key.Value == true)
                {
                    s += key.Key + " ";
                }
            }

            s += "\nButtons Down: ";

            // Add the names of pressed mouse buttons
            foreach (var button in m_buttonState)
            {
                if (button.Value == true)
                {
                    s += button.Key + " ";
                }
            }

            return s;
        }
    }
}
