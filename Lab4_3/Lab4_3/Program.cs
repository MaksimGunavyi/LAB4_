using System;

public class RetryPolicy
{
    private readonly int maxRetries;
    private readonly TimeSpan initialDelay;
    private readonly TimeSpan maxDelay;
    private readonly Random random;

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, TimeSpan maxDelay)
    {
        this.maxRetries = maxRetries;
        this.initialDelay = initialDelay;
        this.maxDelay = maxDelay;
        this.random = new Random();
    }

    public void Execute(Action action)
    {
        int retries = 0;
        TimeSpan delay = this.initialDelay;

        while (true)
        {
            try
            {
                action();
                return; // Success!
            }
            catch (Exception ex)
            {
                if (retries >= this.maxRetries)
                {
                    throw new Exception($"Failed after {retries} retries.", ex);
                }

                retries++;
                Console.WriteLine($"Retrying in {delay.TotalSeconds:F1} seconds (attempt {retries}).");

                // Randomize the delay to avoid multiple retries happening at the same time
                int randomizedDelay = (int)(delay.TotalMilliseconds * 0.8 + this.random.NextDouble() * delay.TotalMilliseconds * 0.4);
                delay = TimeSpan.FromMilliseconds(Math.Min(randomizedDelay, this.maxDelay.TotalMilliseconds));

                System.Threading.Thread.Sleep(delay);
            }
        }
    }
}
