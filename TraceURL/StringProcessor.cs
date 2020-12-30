namespace TraceURL
{
    using System.Linq;

    public class StringProcessor
    {
        public int AllTimeouts { get; set; }
        private readonly string to = "Request timed out.";
        private readonly object alltoLock = new object();

        public int IsTimedOut(string input)
        {
            if (input.Contains(to))
            {
                string[] lines = input.Split('\n');
                int tos = lines.Where(x => x.Contains(to)).Count();
                lock (alltoLock)
                {
                    AllTimeouts += tos;
                }
                return tos;
            }
            return 0;
        }
    }
}
