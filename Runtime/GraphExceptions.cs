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
    [System.Serializable]
    public class NotAttachedToEdgeException : System.Exception
    {
        public NotAttachedToEdgeException() : base("node not attached to edge") { }
        public NotAttachedToEdgeException(string message) : base(message) { }
        public NotAttachedToEdgeException(string message, System.Exception inner) : base(message, inner) { }
        protected NotAttachedToEdgeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class EmptyGraphException : System.Exception
    {
        public EmptyGraphException() : base("this graph must have at least one node"){ }
        public EmptyGraphException(string message) : base(message) { }
        public EmptyGraphException(string message, System.Exception inner) : base(message, inner) { }
        protected EmptyGraphException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}