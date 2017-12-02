using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrimitiveTest
{
    public class ToggleAction
    {
        public Action<SpriteBatch> Action;
        public bool IsActive;
        public Keys Key;

        public ToggleAction(Keys key, Action<SpriteBatch> action, bool isActive = false)
        {
            Action = action;
            Key = key;
            IsActive = isActive;
        }
    }
}
