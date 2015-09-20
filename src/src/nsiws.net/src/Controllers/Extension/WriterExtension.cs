// -----------------------------------------------------------------------
// <copyright file="WriterExtension.cs" company="Eurostat">
//   Date Created : 2013-11-15
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Extension
{
    using System;
    using System.Collections.Generic;

    public static class WriterExtension
    {
        /// <summary>
        /// Runs all actions in the queue.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public static void RunAll(this Queue<Action> actions)
        {
            while (actions.Count > 0)
            {
                actions.Dequeue()();
            }
        }
    }
}