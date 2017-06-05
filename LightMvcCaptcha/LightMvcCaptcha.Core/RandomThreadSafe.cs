using System;

namespace LightMvcCaptcha.Core
{
    public static class RandomThreadSafe
    {
        private static readonly Random _global = new Random();
        [ThreadStatic]
        private static Random _local;

        public static Random Instance
        {
            get
            {
                if (_local == null)
                {
                    int seed;
                    lock (_global) seed = _global.Next();
                    _local = new Random(seed);
                }

                return _local;
            }
        }
    }
}
