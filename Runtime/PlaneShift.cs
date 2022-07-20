using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Pattern
{
    class PlaneShift : SystemBehaviour
    {
        public enum Theme { Dark, Light}

        public Action<Theme> onThemeChange;
        Theme currentTheme;

        protected internal override void Update()
        {
            //blah blah
            //onThemeChange.Invoke(currentTheme);
        }
    }
}
