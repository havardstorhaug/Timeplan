using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timeplan.BL;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Net.Mail;

namespace Timeplan.Tests
{
    [TestClass]
    public class DivTest
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member.Name;
        }

        [TestMethod]
        public void DivTestMethod()
        {
            var size = 0;
            var array = new int[5];

            array[++size] = 1;

            int i = 0;

            //var collection = new List<int>();
            ////for (int i = 1; i <= 110; i++)
            ////{
            ////    var t = i % 8;
            ////    Debug.WriteLine(t);
            ////}

            //var db = new TimeplanEntities();
            //var ansatte = db.Ansatts.Select(ansatt => ansatt);

            //object nullVerdi = new object();

            //var test = nullVerdi != null ? db.Ansatts.Select(ansatt => ansatt) : new List<Ansatt>().AsQueryable();

            //var test5 = from a in db.Ansatts
            //            where a.Id == 1 && test.Contains(a)
            //            select a;

            //var test6 = test5.ToList();

            //var test2 = test != null ? test.Where(a => a.Id == 0) : null;

            //Stopwatch stopWatch = new Stopwatch();

            //stopWatch.Start();
            //foreach (var ansatt in ansatte)
            //{
            //    var test3 = ansatt.Avdeling;
            //}
            //stopWatch.Stop();

            //int xy = 0;
            //decimal test = 67;

            //var t = test % 4;

            //if (test % 4 == 0) 
            //{
            //    int i = 0;
            //}
            //else
            //{
            //    int i = 0;
            //}

            //var db = new TimeplanEntities();
            //var ansattEn = db.Ansatts.First(ansatt => ansatt.Id == 1);

            //var test = GetPropertyName(() => ansattEn.Id);


            //var stopwatch = new Stopwatch();


            //var db = new TimeplanEntities();

            ////var ansattEn = db.Ansatts.First(ansatt => ansatt.Id == 1);
            //var ansattTre = db.Ansatts.First(ansatt => ansatt.Id == 4);

            ////var varslerTilAnsatte = ansattEn.VarslerTilAnsatte;

            //ansattTre.fk_VarslesAvAnsattId = null;

            //db.SaveChanges();

            //stopwatch.Start();

            ////var test = Utilities.GetPropertyName(() => ansattTre.Avdeling.Id);
            //var test = "Id";


            //stopwatch.Stop();

            //Debug.WriteLine("Tid brukt: '" + stopwatch.Elapsed + "'");

        }

        [TestMethod]
        public void TestStack()
        {
            IStack<int> stack = new Stack<int>(0);
            for (int i = 1; i < 20; i++)
                stack.Push(i);

            //foreach(var i in stack)
            //{
            //    Console.WriteLine(i);
            //}

            var peeked = stack.Peek();

            for (int i = 1; i < 20; i++)
                stack.Pop();

            for (int i = 1; i < 20; i++)
                stack.Push(i);

            for (int i = 1; i < 10; i++)
                stack.Pop();

            stack.Clear();

        }

        [TestMethod]
        public void TestQueue()
        {
            IQueue<int> queue = new Queue<int>(0);
            for (int i = 1; i <= 20; i++)
                queue.Enqueue(i);

            for (int i = 1; i <= 10; i++)
                queue.Dequeue();

            for (int i = 21; i <= 40; i++)
                queue.Enqueue(i);

            for (int i = 1; i <= 10; i++)
                queue.Dequeue();

            queue.Clear();
        }

        [TestMethod]
        public void TestList()
        {
            IList<int> list = new List<int>(0);
            for (int i = 1; i <= 20; i++)
                list.Add(i);

            if (list.Contains(100))
            {
                int i = 0;
            }

            var index = list.IndexOf(100);
            index = list.IndexOf(10);

            if (list.Contains(10))
            {
                int i = 0;
            }

            list.Remove(100);

            list.Remove(10);

            list.RemoveAt(100);

            list.Clear();
        }
    }




    //public class List<T> : IList<T>
    //{
    //    private T[] list;
    //    private int size;       // Number of elements.

    //    public List(int capacity)
    //    {
    //        if (size < 0)
    //            throw new ArgumentOutOfRangeException("Size: '" + size + "'");

    //        list = new T[size];
    //        size = 0;
    //    }

    //    public void Add(T value)
    //    {
    //        if (size == list.Length)
    //        {
    //            // Notice the + 1, to counter the 0 length problem
    //            Array.Resize(ref list, (list.Length + 1) * 2);
    //        }

    //        list[size++] = value;
    //    }

    //    // Clears the contents of List.
    //    public void Clear()
    //    {
    //        if (size > 0)
    //        {
    //            Array.Clear(list, 0, size);
    //            size = 0;
    //        }
    //    }

    //    public bool Contains(T item)
    //    {
    //        if (item == null)
    //        {
    //            for (int i = 0; i < size; i++)
    //                if (list[i] == null)
    //                    return true;
    //            return false;
    //        }
    //        else
    //        {
    //            //EqualityComparer<T> c = EqualityComparer<T>.Default;
    //            //for (int i = 0; i < size; i++)
    //            //{
    //            //    if (c.Equals(list[i], item)) return true;
    //            //}
    //            //return false;

    //            for (int i = 0; i < size; i++)
    //            {
    //                if (list[i].Equals(item)) return true;
    //            }
    //            return false;
    //        }
    //    }

    //    public int IndexOf(T item)
    //    {
    //        return Array.IndexOf(list, item, 0, size);
    //    }

    //    public bool Remove(T item)
    //    {
    //        int index = IndexOf(item);
    //        if (index >= 0)
    //        {
    //            RemoveAt(index);
    //            return true;
    //        }

    //        return false;
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        if (index >= size)
    //        {
    //            throw new ArgumentOutOfRangeException("Size: '" + size + "', Index: '" + index + "'");
    //        }

    //        size--;

    //        if (index < size)
    //        {
    //            Array.Copy(list, index + 1, list, index, size - index);
    //        }

    //        list[size] = default(T);
    //    }
    //}



    public interface IQueue<T>
    {
        void Enqueue(T item);
        T Dequeue();
        void Clear();
    }





    public class Queue<T> : IQueue<T>
    {
        private T[] queue;
        private int head;       // First valid element in the queue
        private int tail;       // Last valid element in the queue
        private int size;       // Number of elements.

        public Queue(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("Size: '" + size + "'");

            queue = new T[size];

            head = 0;
            tail = 0;
            size = 0;
        }

        public void Clear()
        {
            if (head < tail)
                Array.Clear(queue, head, size);
            else
            {
                Array.Clear(queue, head, queue.Length - head);
                Array.Clear(queue, 0, tail);
            }

            head = 0;
            tail = 0;
            size = 0;
        }

        public void Enqueue(T item)
        {
            if (size == queue.Length)
            {
                int newcapacity = (queue.Length + 1) * 2;
                SetCapacity(newcapacity);
            }

            queue[tail] = item;
            tail = (tail + 1) % queue.Length;
            size++;
        }


        public T Dequeue()
        {
            if (size == 0)
                throw new InvalidOperationException("The queue is empty");

            T removed = queue[head];
            queue[head] = default(T);
            head = (head + 1) % queue.Length;
            size--;

            return removed;
        }

        private void SetCapacity(int capacity)
        {
            T[] newarray = new T[capacity];
            if (size > 0)
            {
                if (head < tail)
                {
                    Array.Copy(queue, head, newarray, 0, size);
                }
                else
                {
                    Array.Copy(queue, head, newarray, 0, queue.Length - head);
                    Array.Copy(queue, 0, newarray, queue.Length - head, tail);
                }
            }

            queue = newarray;
            head = 0;
            tail = (size == capacity) ? 0 : size;
        }
    }


    public interface IStack<T>
    {
        void Push(T value);
        T Pop();
        T Peek();
        void Clear();
        IEnumerator GetEnumerator();
    }

    public class Stack<T> : IStack<T>, IEnumerable
    {
        private T[] stack;
        private int currentStackIndex;

        public Stack(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("Size: '" + size + "'");

            stack = new T[size];
            currentStackIndex = 0;
        }

        public void Push(T value)
        {
            if (currentStackIndex >= stack.Length)
            {
                //Notice the + 1, to counter the 0 length problem
                Array.Resize(ref stack, (stack.Length + 1) * 2);
            }

            stack[currentStackIndex++] = value;
        }

        public T Pop()
        {
            if (currentStackIndex == 0)
                throw new InvalidOperationException("The stack is empty");

            T value = stack[--currentStackIndex];
            stack[currentStackIndex] = default(T);
            return value;
        }

        public T Peek()
        {
            if (currentStackIndex == 0)
                throw new InvalidOperationException("The stack is empty");

            return stack[currentStackIndex - 1]; ;
        }

        public void Clear()
        {
            Array.Clear(stack, 0, currentStackIndex);
            currentStackIndex = 0;
        }

        public IEnumerator GetEnumerator()
        {
            //return stack.GetEnumerator();
            for (int i = 0; i < stack.Length; i++)
                yield return stack[i];
        }
    }


    public class Logger
    {
        public static void Log(string tekst)
        {
            File.WriteAllText(@"C:\Feil", tekst);
        }
    }

    public class Kunde
    {
        int kundetype = 0;

        public virtual void Lagre()
        {
            try
            {

                // forsøker å lagre...
            }
            catch (Exception e)
            {
                Logger.Log("Feil");
            }
        }

    }

    public class Kunde0 : Kunde
    {
        public override void Lagre()
        {
            base.Lagre();
            // Spesial lagrefunk for denne klasse
        }
    }

    
    public class Notificator
    {
        public void Notify()
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            client.Send(mail);
        }
    }

    public class solid
    {
        // SRP - singel responsibility principle
        // OCP - open closed principle
        // LSP - liskov substitution principle
        // ISP - interface segregation principle
        // DIP - dependency inversion principle
    }

    //public class Notificator
    //{
    //    int NotificationType { get; set; }

    //    public IMessenger Messenger { get; set; }

    //    public void Notify()
    //    {
    //        // Not DIP
    //        if (NotificationType == 1)
    //        {
    //            MailMessage email = new MailMessage();
    //            SmtpClient client = new SmtpClient();
    //            client.Send(email);
    //        }
    //        else if (NotificationType == 2)
    //        {
    //            SMS sms = new SMS();
    //            Sender.Send(sms);
    //        }

    //        // DIP
    //        Messenger.SendMessage();
    //    }
    //}

    //public class Kontroller
    //{

    //    public void Kontroll()
    //    {
    //        Notificator notificator = new Notificator();
    //        EmailNotificator emailNotificator = new EmailNotificator();
    //        SMSNotificator smsNotificator = new SMSNotificator();

    //        notificator.Messenger = emailNotificator;
    //        notificator.Notify();

    //        notificator.Messenger = smsNotificator;
    //        notificator.Notify();

    //    }
    //}

    public interface IMessenger
    {
        void SendMessage();
    }

    public class EmailNotificator : IMessenger
    {
        public void SendMessage()
        { }
    }

    public class SMSNotificator : IMessenger
    {
        public void SendMessage()
        { }
    }

    public static class Sender
    {
        public static void Send(Email email)
        { }

        public static void Send(SMS sms)
        { }
    }

    public class Email
    {

    }

    public class SMS
    { }


    //public class Customer
    //{
    //    public int Kundetype { get; set; }

    //    public IRabatt Rabatt { get; set; }

    //    public double GetRabatt(double totalKjopesum)
    //    {
    //        // Ikke OCP
    //        if (Kundetype == 1)
    //        {
    //            return totalKjopesum - 100;
    //        }
    //        else if (Kundetype == 2)
    //        {
    //            return totalKjopesum - 200;
    //        }

    //        // OCP
    //        return Rabatt.GetRabatt(totalKjopesum);
    //    }
    //}

    //public interface IRabatt
    //{
    //    double GetRabatt(double totalKjopesum);
    //}

    //public class Rabatt : IRabatt
    //{
    //    private readonly double rabatt;

    //    public Rabatt(double verdi)
    //    {
    //        rabatt = verdi;
    //    }

    //    public double GetRabatt(double totalKjopesum)
    //    {
    //        return totalKjopesum - rabatt;
    //    }

    //}

    //public class Kontroller
    //{
    //    public void Kontroll()
    //    {
    //        Customer goldCustomer = new Customer();
    //        IRabatt goldRabatt = new Rabatt(300);
    //        goldCustomer.Rabatt = goldRabatt;
    //    }
    //}


    //public class Customer
    //{
    //    public void Save()
    //    {
    //        try
    //        {
    //            // Trying to save stuff...
    //        }
    //        catch (Exception e)
    //        {
    //            //File.WriteAllText(@"C:\Temp", e.ToString());
    //            Logger.Log(e.ToString());
    //        }
    //    }
    //}

    //public static class Logger
    //{
    //    public static void Log(string logtext)
    //    {
    //        // Do logging whatever way...
    //    }
    //}




}
