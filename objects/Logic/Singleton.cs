using Godot;

namespace ProjectBriseis.objects.Logic {
    public partial class Singleton<T> : Node
        where T : Node {
        public sealed override void _Ready() {
            _instance = this;
            _SingletonReady();
        }

        public virtual void _SingletonReady() {
        }

        private static Node _instance;

        public static T instance => (T) _instance;
    }
}