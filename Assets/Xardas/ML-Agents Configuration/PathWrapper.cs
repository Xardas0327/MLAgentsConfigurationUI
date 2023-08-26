using System;

namespace Xardas.MLAgents
{
    public class PathWrapper<T>
    {
        T value;
        Func<T, string> get;
        Action<T, string> set;

        public string Path
        {
            get => get(value);
            set => set(this.value, value);
        }

        public PathWrapper(T value, Func<T, string> get, Action<T, string> set) 
        {
            this.value = value;
            this.get = get;
            this.set = set;
        }
    }
}