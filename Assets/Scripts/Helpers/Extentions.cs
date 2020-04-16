using System;

namespace Orbitality
{
    public static class Extentions
    {
        public static void Fire(this Action action)
        {
            if (action != null)
                action.Invoke();
        }

        public static void Fire<T>(this Action<T> action, T t)
        {
            if (action != null)
                action.Invoke(t);
        }
    }
}