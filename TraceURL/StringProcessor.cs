namespace TraceURL
{
    using System.Linq;

    public class StringProcessor
    {
        public int AllTimeouts { get; set; }
        private readonly string to = "Request timed out.";
        private readonly object alltoLock = new ();

        public int IsTimedOut(string input)
        {
            if (input is null) throw new System.ArgumentNullException(nameof(input), "input was NULL");

            int tos = input.Split('\n')
                .Where(x => x.Contains(to, System.StringComparison.InvariantCultureIgnoreCase))
                .Count();
            lock (alltoLock)
            {
                AllTimeouts += tos;
            }
            return tos;
        }
    }
}
