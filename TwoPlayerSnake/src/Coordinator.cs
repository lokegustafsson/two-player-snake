using System;
using System.Threading;

namespace TwoPlayerSnake
{
    abstract class Coordinator
    {
        private Timer _timer;

        internal void Run(TimeSpan period)
        {
            // System.Threading.Timer silently swallows exceptions,
            // so we need to explicitly log any errors and exit manually
            _timer = new Timer((state) =>
            {
                try { Update(); }
                catch (Exception e)
                {
                    Program.Log(this).Fatal(e, "Something unexpected happened:");
                    System.Environment.Exit(-1);
                }
            }, null, 0, (int)period.TotalMilliseconds);
        }
        protected abstract void Update();
    }
}