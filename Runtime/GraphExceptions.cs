namespace SadSapphicGames.CustomGraphs {
    [System.Serializable]
    public class NotInGraphException : System.Exception
    {
        public NotInGraphException(int id) : base($"there is no node {id} in the graph"){ }
        public NotInGraphException(string message) : base(message) { }
        public NotInGraphException(string message, System.Exception inner) : base(message, inner) { }
        protected NotInGraphException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}