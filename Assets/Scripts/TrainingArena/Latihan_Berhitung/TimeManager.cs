using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public QuizManager Manager;
    public float gameTime;
    public float barSpeed;

    public static bool TimeOut = false;
    public static bool isOn = false;

    /*public struct bg_task
    {
        public CancellationTokenSource tokenSource;
        public Task task;
    }
    public List<bg_task> tasks = new List<bg_task>();*/
    public List<Thread> threads = new List<Thread>();

    //string @string;

    void Start()
    {
        //RunQuizTimer();
        KillThreads();
    }

    public void KillThreads()
    {
        threads.ForEach(x => { x.Abort(); });
    }

    public void SetQuizTimer()
    {
        if (AnswerChecker.Confirmed == false)
        {
            if (threads.Capacity != 0)
            {
                try
                {
                    KillThreads();
                    threads.RemoveAt(threads.Capacity-1);
                }
                catch {  }
            }

            barSpeed = gameTime = CSVFileConverter.quizTime;

            /*CancellationTokenSource tokenSource = new CancellationTokenSource();
            Task task = new Task(() => RunQuizTimerRoutine(tokenSource.Token));

            bg_task taskNew;
            taskNew.task = task;
            taskNew.tokenSource = tokenSource;

            tasks.Add(taskNew);*/

            Thread newTime = new Thread(new ThreadStart(RunQuizTimerRoutine));
            threads.Add(newTime);
        }
    }

    public void RunQuizTimer()
    {
        //StartCoroutine(RunQuizTimerRoutine());
        // Stopwatch sw = new Stopwatch();
        // sw.Start();
        //tasks.LastOrDefault().task.Start();
        threads.Last().Start();
        //  sw.Stop();
        // Manager.TimeExecuted.text = "T " + sw.Elapsed;
    }

    private void RunQuizTimerRoutine()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        while (true)
        {
            gameTime = barSpeed - (float)sw.Elapsed.TotalSeconds;
            if (isOn.Equals(false) | sw.Elapsed.TotalSeconds >= barSpeed)
            {
                sw.Stop();
                break;
            }
        }
        TimeOut = true;
    }

    //private IEnumerator RunQuizTimerRoutine()
    //{
    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();

    //    while (gameTime > 0)
    //    {
    //        yield return new WaitForSecondsRealtime(barSpeed);
    //        gameTime -= barSpeed;
    //    }
    //    TimeOut = true;

    //    sw.Stop();
    //    Manager.TimeExecuted.text = ""+sw.Elapsed;
    //}

    /*private void RunQuizTimerRoutine(CancellationToken token)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        while (true)
        {
            gameTime = barSpeed - (float)sw.Elapsed.TotalSeconds;
            if (sw.Elapsed.TotalSeconds >= barSpeed || token.IsCancellationRequested)
            {
                sw.Stop();
                break;
            }
        }
        TimeOut = true;

        //@string = new StringBuilder();
       // @string = "SW " + sw.Elapsed;
    }*/

}
