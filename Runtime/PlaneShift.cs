using System;

namespace System_Pattern
{
    class SomeComponent : SceneSystem
    {
        public enum Theme { Dark, Light}

        public Action<Theme> onThemeChange;
        Theme currentTheme;

        void Update()
        {
            //blah blah
            //onThemeChange.Invoke(currentTheme);
        }
    }
}
