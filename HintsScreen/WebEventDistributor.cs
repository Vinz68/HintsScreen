using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeSoft.WinIoTSolution.WebEventDistributor
{

    // Define a class to hold web event info
    public class WebHintEventArgs : EventArgs
    {
        public WebHintEventArgs(int time, string hint)
        {
            hintDisplayTime = time;
            hintText = hint;
        }

        #region attributes

        private int hintDisplayTime;
        public int Time
        {
            get { return hintDisplayTime; }
            set { hintDisplayTime = value; }
        }


        private string hintText;
        public string Hint
        {
            get { return hintText; }
            set { hintText = value; }
        }

        public string TimeStamp { get; set;  }

        #endregion
    }


    /// <summary>
    /// This class is used to distribute incomming web events to internal subscribers
    /// </summary>
    public class WebEventDistributor
    {
        private static volatile WebEventDistributor instance;
        private static object lockObj = new Object();

        // Declare the event using EventHandler<T>
        public event EventHandler<WebHintEventArgs> RaiseWebHintEvent;

        #region Singleton
        /// <summary>
        /// Constructor
        /// </summary>
        private WebEventDistributor() { }

        /// <summary>
        /// One and only instance
        /// </summary>
        public static WebEventDistributor Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new WebEventDistributor();
                    }
                }
                return instance;
            }
        }
        #endregion



        public void DistributeHint(int time, string hint)
        {
            // raise the event. 
            OnRaiseWebHintEvent(new WebHintEventArgs(time, hint));
        }


        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        protected virtual void OnRaiseWebHintEvent(WebHintEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<WebHintEventArgs> handler = RaiseWebHintEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Add time stamp in the event 
                e.TimeStamp = String.Format("{0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                // Distribute the WebHintEvent
                handler(this, e);
            }
        }

    }
}










