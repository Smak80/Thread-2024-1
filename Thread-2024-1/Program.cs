// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

var done = 0;
var locker = new object();
int n = 2_000_000_000;

var sw = new Stopwatch();
sw.Start();
long s = 0;
for (int i = 1; i <= n; i++)
{
    s += i;
}
sw.Stop();
Console.WriteLine("{0} за {1}мс", s, sw.ElapsedMilliseconds);

sw.Restart();
s = 0;
var t1 = new Thread(_ =>
{
    long s1 = 0;
    for (int i = 1; i <= n; i += 2)
    {
        s1 += i;
    }
    OnStop(s1);
});
var t2 = new Thread(() =>
{
    long s2 = 0;
    for (int i = 2; i <= n; i += 2)
    {
        s2 += i;
    }
    OnStop(s2);
});
t1.Start();
t2.Start();
void OnStop(long result)
{
    lock (locker)
    {
        s += result;
        if (++done == 2)
        {
            sw.Stop();
            Console.WriteLine("{0} за {1}мс", s, sw.ElapsedMilliseconds);
        }
    }
}