namespace JsonAPI.Model
{
    public class Post
    {
    public int UserId { get; init; }
    public int Id { get; init; }
    public string Title { get; init; }
    public string Body { get; init; }
    }
    public class CreatePost
    {
        public int UserId { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }

    }

    public class UpdatePost
    {
        public int UserId { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }

    }
}
