// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaxObsStatus.cs" company="EUROSTAT">
//   Date Created : 2014-09-30
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The max OBS status.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.DataRetriever.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The max OBS status.
    /// </summary>
    internal class MaxObsStatus : IDisposable
    {
        /// <summary>
        /// The _write observation function
        /// </summary>
        private readonly Func<DataRetrievalInfoSeries, string, string, IEnumerable<ComponentValue>, int> _writeObsFunc;

        #region Fields
        /// <summary>
        /// The _queue last observation action
        /// </summary>
        private readonly Func<Tuple<string, string, IEnumerable<ComponentValue>>, int> _queueLastObsAction;

        /// <summary>
        /// The _last observation queue
        /// </summary>
        private readonly Queue<Tuple<string, string, IEnumerable<ComponentValue>>> _lastObsQueue;

        /// <summary>
        /// The _last n observations
        /// </summary>
        private readonly int _lastNObservations;

        /// <summary>
        /// The limit
        /// </summary>
        private readonly int _limit;

        /// <summary>
        /// The _info
        /// </summary>
        private readonly DataRetrievalInfoSeries _info;

        /// <summary>
        /// The _last observation written
        /// </summary>
        private int _lastObsWritten;

        /// <summary>
        ///     The _count
        /// </summary>
        private int _count;

        /// <summary>
        /// The _total count
        /// </summary>
        private int _totalWrittenCount;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxObsStatus"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="writeObsFunc">
        /// The write observation method.
        /// </param>
        public MaxObsStatus(DataRetrievalInfoSeries info, Func<DataRetrievalInfoSeries, string, string, IEnumerable<ComponentValue>, int> writeObsFunc)
        {
            this._info = info;
            this._writeObsFunc = writeObsFunc;
            this._limit = info.Limit;
            var firstNObservations = info.FirstNObservations;
            this._lastNObservations = info.LastNObservations;
            this._lastObsQueue = new Queue<Tuple<string, string, IEnumerable<ComponentValue>>>(Math.Max(this._lastNObservations, 0));
            if (info.HasFirstAndLastNObservations)
            {
                this._queueLastObsAction = func =>
                    {
                        var val = this.WriteOrSkip(firstNObservations, func);
                        if (val != 0)
                        {
                            return val;
                        }

                        this._lastObsQueue.Enqueue(func);

                        if (this._lastObsQueue.Count > this._lastNObservations)
                        {
                            this._lastObsQueue.Dequeue();
                        }

                        return 0;
                    };
            }
            else if (info.HasFirstNObservations)
            {
                this._queueLastObsAction = func => this.WriteOrSkip(firstNObservations, func);
            }
            else if (info.HasLastNObservations)
            {
                // NOTE Assume that we have "DESC".
                this._queueLastObsAction = func => this.WriteOrSkip(this._lastNObservations, func);
            }
            else
            {
                this._queueLastObsAction = this.WriteObs;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the last observation written
        /// </summary>
        public int LastObservationsWritten
        {
            get
            {
                return this._lastObsWritten;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the specified write observation.
        /// </summary>
        /// <param name="writeObs">The write observation action..</param>
        /// <returns>The number of observation actually written.</returns>
        public int Add(Tuple<string, string, IEnumerable<ComponentValue>> writeObs)
        {
            return this._queueLastObsAction(writeObs);
        }
        
        /// <summary>
        ///     Resets the count.
        /// </summary>
        public void ResetCount()
        {
            int lastObs = 0;
            while (this._lastObsQueue.Count > 0)
            {
                var func = this._lastObsQueue.Dequeue();
                if (this._lastObsQueue.Count < this._lastNObservations && (this._limit <= 0 || this._limit < this._totalWrittenCount))
                {
                    lastObs += this.LastObservationsWritten + this.WriteObs(func); 
                }
            }

            this._totalWrittenCount += lastObs;

            this._lastObsWritten = lastObs;
            this._count = 0;
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                this.ResetCount();
            }
        }

        /// <summary>
        /// Writes the observation.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns>The number of observations.</returns>
        private int WriteObs(Tuple<string, string, IEnumerable<ComponentValue>> tuple)
        {
            return this._writeObsFunc(this._info, tuple.Item1, tuple.Item2, tuple.Item3);
        }

        /// <summary>
        /// Writes the observation or skips it.
        /// </summary>
        /// <param name="firstNObservations">
        /// The first n observations.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The number of observations written.
        /// </returns>
        private int WriteOrSkip(int firstNObservations, Tuple<string, string, IEnumerable<ComponentValue>> values)
        {
            if (this._count < firstNObservations)
            {
                var val = this.WriteObs(values);
                this._count += val;
                this._totalWrittenCount += val;
                return val;
            }

            this._count++;
            return 0;
        }
    }
}