﻿/* Copyright (c) 2018 noahc3
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace PromiseBlazorTest
{
    public static class Promises
    {
        private static readonly ConcurrentDictionary<string, IPromiseCallbackHandler> CallbackHandlers =
            new ConcurrentDictionary<string, IPromiseCallbackHandler>();

        [JSInvokable]
        public static void PromiseCallback(string callbackId, string result)
        {
            if (CallbackHandlers.TryGetValue(callbackId, out IPromiseCallbackHandler handler))
            {
                handler.SetResult(result);
                CallbackHandlers.TryRemove(callbackId, out IPromiseCallbackHandler _);
            }
        }

        [JSInvokable]
        public static void PromiseError(string callbackId, string error)
        {
            if (CallbackHandlers.TryGetValue(callbackId, out IPromiseCallbackHandler handler))
            {
                handler.SetError(error);
                CallbackHandlers.TryRemove(callbackId, out IPromiseCallbackHandler _);
            }
        }

        public static Task<TResult> ExecuteAsync<TResult>(IJSRuntime jsRuntime, string fnName, object data = null)
        {
            var tcs = new TaskCompletionSource<TResult>();
            
            string callbackId = Guid.NewGuid().ToString();
            if (CallbackHandlers.TryAdd(callbackId, new PromiseCallbackHandler<TResult>(tcs)))
            {
                if (data == null)
                {
                    jsRuntime.InvokeAsync<bool>("runFunction", callbackId, fnName);
                }
                else
                {
                    jsRuntime.InvokeAsync<bool>("runFunction", callbackId, fnName, data);
                }

                return tcs.Task;
            }

            throw new Exception("An entry with the same callback id already existed, really should never happen");
        }
    }
}
