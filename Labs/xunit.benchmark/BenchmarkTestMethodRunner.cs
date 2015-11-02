// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.Xunit
{
    class BenchmarkTestMethodRunner : TestMethodRunner<BenchmarkTestCase>
    {
        private readonly IMessageSink msgSink;
        public BenchmarkTestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method, IEnumerable<BenchmarkTestCase> testCases, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, IMessageSink msgSink)
            : base(testMethod, @class, method, testCases, messageBus, aggregator, cancellationTokenSource)
        {
            this.msgSink = msgSink;
        }

        protected override Task<RunSummary> RunTestCaseAsync(BenchmarkTestCase testCase)
        {
            return new BenchmarkTestCaseRunner(testCase, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource, msgSink).RunAsync();
        }
    }
}
