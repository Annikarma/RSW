using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSW
{
    public enum SortBy
    {
        /// <summary>
        /// Ticker = Kürzel
        /// </summary>
        Ticker,

        /// <summary>
        /// Score = Score
        /// </summary>
        Score,

        /// <summary>
        ///  Sentiment = Bär, Stier
        /// </summary>
        Sentiment,

        /// <summary>
        ///  Sentiment = Stier
        /// </summary>
        SentimentStier,

        /// <summary>
        /// NoOfComments = Anzahl Kommentare
        /// </summary>
        NoOfComments
    }
}
