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
    public class IncompatibleEdgeException : System.Exception
    {
        public IncompatibleEdgeException() : base("Cannot mix directed and undirected edges") { }
        public IncompatibleEdgeException(string message) : base(message) { }
        public IncompatibleEdgeException(string message, System.Exception inner) : base(message, inner) { }
        protected IncompatibleEdgeException(
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
    [System.Serializable]
    public class DifferentGraphsException : System.Exception
    {
        public DifferentGraphsException() : base("parent graphs do not match"){ }
        public DifferentGraphsException(string message) : base(message) { }
        public DifferentGraphsException(string message, System.Exception inner) : base(message, inner) { }
        protected DifferentGraphsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class NotATreeException : System.Exception
    {
        public NotATreeException() : base("this graph does not constitute a tree"){ }
        public NotATreeException(string message) : base(message) { }
        public NotATreeException(string message, System.Exception inner) : base(message, inner) { }
        protected NotATreeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class NonUniqueIDException : System.Exception
    {
        public NonUniqueIDException(int id) : base($"there is already a element with id {id} in the graph") { }
        public NonUniqueIDException(string message) : base(message) { }
        public NonUniqueIDException(string message, System.Exception inner) : base(message, inner) { }
        protected NonUniqueIDException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class OrphanException : System.Exception
    {
        public OrphanException() : base("this object does no have a parent graph - this can occur when copying graph objects with the copy constructor. Did you forget to add the copy to a new graph?") { }
        public OrphanException(string message) : base(message) { }
        public OrphanException(string message, System.Exception inner) : base(message, inner) { }
        protected OrphanException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class NotDAGException : System.Exception
    {
        public NotDAGException() : base("This is not a directed Acyclic graph") { }
        public NotDAGException(string message) : base(message) { }
        public NotDAGException(string message, System.Exception inner) : base(message, inner) { }
        protected NotDAGException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}