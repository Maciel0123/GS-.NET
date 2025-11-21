namespace WScoreTests
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public List<object>? Links { get; set; }
    }
}
