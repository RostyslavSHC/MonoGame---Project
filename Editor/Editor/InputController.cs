using Microsoft.Xna.Framework;
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

        // Store the current position/last position of the mouse AND the start/end of a drag    
        public Vector2 MousePosition { get; set; } = Vector2.Zero;
        public Vector2 LastPosition { get; private set; } = Vector2.Zero;
        public Vector2 DragStart { get; set; } = Vector2.Zero;
        public Vector2 DragEnd { get; set; } = Vector2.Zero;

        // Store whether each keyboard key and mouse button is pressed
        private Dictionary<Keys, bool> m_keyState = new();
        private Dictionary<MouseButtons, bool> m_buttonState = new();
        private int m_mouseWheel = 0;

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


        public void SetWheel(int _delta)
        {
            m_mouseWheel += _delta; // Adds mouse scroll movement
        }

        public int GetWheel()
        {
            return m_mouseWheel; // Returns the mouse scroll value
        }

        public void Clear()
        {
            m_mouseWheel = 0; // Resets mouse scroll value
            LastPosition = MousePosition; // Saves current mouse position
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
