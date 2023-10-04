using ProjectBriseis.objects.Logic;

namespace ProjectBriseis.objects.AutoLoad {
    public partial class MultiplayerAutoLoad : Singleton<MultiplayerAutoLoad> {
        
        public override void _SingletonReady() {
            Multiplayer.PeerConnected += PeerConnected;
            Multiplayer.PeerDisconnected += PeerDisconnected;
            Multiplayer.ConnectedToServer += ConnectedToServer;
            Multiplayer.ConnectionFailed += ConnectionFailed;
        }

        private void ConnectionFailed() {
            throw new System.NotImplementedException();
        }

        private void ConnectedToServer() {
            throw new System.NotImplementedException();
        }

        private void PeerDisconnected(long id) {
            throw new System.NotImplementedException();
        }

        private void PeerConnected(long id) {
            throw new System.NotImplementedException();
        }

        public void Connect(string s) {
            throw new System.NotImplementedException();
        }

        public void Host() {
            throw new System.NotImplementedException();
        }

        public void Disconnect() {
            throw new System.NotImplementedException();
        }
    }
}