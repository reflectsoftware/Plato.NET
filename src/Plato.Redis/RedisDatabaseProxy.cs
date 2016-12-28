// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Plato.Redis
{
    public class RedisDatabaseProxy : RealProxy
    {
        private readonly IDatabase _database;
        private readonly RedisDatabaseProxyConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisDatabaseProxy"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        [PermissionSet(SecurityAction.LinkDemand)]
        public RedisDatabaseProxy(IDatabase database, RedisDatabaseProxyConfiguration config = null) : base(typeof(IDatabase))
        {
            _database = database;
            _configuration = config ?? new RedisDatabaseProxyConfiguration();

        }

        /// <summary>
        /// Considers the retry.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private bool ConsiderRetry(Exception ex)
        {
            var testException = (ex is AggregateException) ? ex.InnerException : ex;
            return (testException is RedisServerException) || (testException is RedisConnectionException);
        }

        /// <summary>
        /// When overridden in a derived class, invokes the method that is specified in the provided <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> on the remote object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> that contains a <see cref="T:System.Collections.IDictionary" /> of information about the method call.</param>
        /// <returns>
        /// The message returned by the invoked method, containing the return value and any out or ref parameters.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
        /// </PermissionSet>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;

            var retry = _configuration.Retry;
            while (true)
            {
                try
                {
                    var result = methodInfo.Invoke(_database, methodCall.InArgs);
                    var retMessage = new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                    var asyncMessage = retMessage.ReturnValue as Task;
                    var exception = retMessage.Exception;

                    if (asyncMessage != null)
                    {
                        try
                        {
                            asyncMessage.Wait();
                        }
                        catch(AggregateException ex)
                        {
                            exception = ex;
                        }
                    }

                    if (exception != null)
                    {
                        throw new TargetInvocationException(exception);
                    }

                    return retMessage;
                }
                catch (TargetInvocationException ex)
                {
                    retry--;
                    if (retry >= 0 && ConsiderRetry(ex.InnerException))
                    {
                        Task.Delay(_configuration.RetryWait).Wait();
                        continue;
                    }

                    return new ReturnMessage(ex.InnerException, methodCall);
                }
            }
        }
    }
}
