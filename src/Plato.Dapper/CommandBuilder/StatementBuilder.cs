// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Dapper;
using Plato.Dapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plato.Dapper.CommandBuilder
{
    /// <summary>
    ///
    /// </summary>
    public class StatementBuilder
    {
        private class Command
        {
            public string Statement { get; set; }
            public StatementParameterList ParameterList { get; set; }
        }

        private readonly List<Command> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementBuilder"/> class.
        /// </summary>
        public StatementBuilder()
        {
            Commands = new List<Command>();
        }

        /// <summary>
        /// Adds the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <exception cref="System.ArgumentNullException">statement</exception>
        public void Add(string statement, StatementParameterList paramList = null)
        {
            if (statement == null)
            {
                throw new ArgumentNullException(nameof(statement));
            }

            Commands.Add(new Command() { Statement = statement, ParameterList = paramList });
        }

        /// <summary>
        /// Adds the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="paramList">The parameter list.</param>
        public void Add(StringBuilder statement, StatementParameterList paramList = null)
        {
            Add(statement.ToString(), paramList);
        }

        /// <summary>
        /// Adds the specified statement builder.
        /// </summary>
        /// <param name="statementBuilder">The statement builder.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Add(StatementBuilder statementBuilder)
        {
            if (statementBuilder == null)
            {
                throw new ArgumentNullException(nameof(statementBuilder));
            }

            Commands.AddRange(statementBuilder.Commands);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return Commands.Count;
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Commands.Clear();
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns></returns>
        public StatementBuilderResult Build()
        {
            var combinedCmd = new StringBuilder();
            var parameters = new DynamicParameters();

            for (var i = 0; i < Commands.Count; i++)
            {
                combinedCmd.AppendLine(Commands[i].Statement.Replace("[@]", string.Format("@{0}_", i)));

                if (Commands[i].ParameterList != null)
                {
                    Commands[i].ParameterList.AddDynamicParametersById(parameters, i);
                }
            }

            return new StatementBuilderResult() { Statement = combinedCmd.ToString(), ParameterList = parameters };
        }

        /// <summary>
        /// Builds the specified inject builder.
        /// </summary>
        /// <param name="injectBuilder">The inject builder.</param>
        /// <param name="injectors">The injectors.</param>
        /// <returns></returns>
        public static StatementBuilderResult Build(StatementBuilder injectBuilder, List<StatementBuilderInject> injectors)
        {
            var injectCmd = new StringBuilder();
            var parameters = new DynamicParameters();
            var commandId = 0;
            
            foreach (var cmd in injectBuilder.Commands)
            {
                injectCmd.AppendLine(cmd.Statement.Replace("[@]", string.Format("@{0}_", commandId)));

                if (cmd.ParameterList != null)
                {
                    cmd.ParameterList.AddDynamicParametersById(parameters, commandId);
                }

                commandId++;
            }


            foreach (var injector in injectors)
            {
                var combinedCmd = new StringBuilder();
                foreach (var cmd in injector.Builder.Commands)
                {
                    combinedCmd.AppendLine(cmd.Statement.Replace("[@]", string.Format("@{0}_", commandId)));
                    if (cmd.ParameterList != null)
                    {
                        cmd.ParameterList.AddDynamicParametersById(parameters, commandId);
                    }

                    commandId++;
                }

                injectCmd.Replace(injector.InjectId, combinedCmd.ToString());
            }

            return new StatementBuilderResult() { Statement = injectCmd.ToString(), ParameterList = parameters };
        }

        /// <summary>
        /// Builds the specified inject statement.
        /// </summary>
        /// <param name="injectStatement">The inject statement.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <param name="injectors">The injectors.</param>
        /// <returns></returns>
        public static StatementBuilderResult Build(string injectStatement, StatementParameterList paramList, List<StatementBuilderInject> injectors)
        {
            var injectBuilder = new StatementBuilder();
            injectBuilder.Add(injectStatement, paramList);

            return Build(injectBuilder, injectors);
        }

        /// <summary>
        /// Builds the specified inject statement.
        /// </summary>
        /// <param name="injectStatement">The inject statement.</param>
        /// <param name="injectors">The injectors.</param>
        /// <returns></returns>
        public static StatementBuilderResult Build(string injectStatement, List<StatementBuilderInject> injectors)
        {
            return Build(injectStatement, null, injectors);
        }

        /// <summary>
        /// Builds the specified inject statement.
        /// </summary>
        /// <param name="injectStatement">The inject statement.</param>
        /// <param name="paramList">The parameter list.</param>
        /// <param name="injectors">The injectors.</param>
        /// <returns></returns>
        public static StatementBuilderResult Build(StringBuilder injectStatement, StatementParameterList paramList, List<StatementBuilderInject> injectors)
        {
            return Build(injectStatement.ToString(), paramList, injectors);
        }

        /// <summary>
        /// Builds the specified inject statement.
        /// </summary>
        /// <param name="injectStatement">The inject statement.</param>
        /// <param name="injectors">The injectors.</param>
        /// <returns></returns>
        public static StatementBuilderResult Build(StringBuilder injectStatement, List<StatementBuilderInject> injectors)
        {
            return Build(injectStatement.ToString(), injectors);
        }

        /// <summary>
        /// Builds the chunks.
        /// </summary>
        /// <returns></returns>
        public List<StatementBuilderResult> BuildChunks()
        {
            var chunks = new List<StatementBuilderResult>();
            var combinedCmd = new StringBuilder();
            var parameters = new DynamicParameters();

            for (var i = 0; i < Commands.Count; i++)
            {
                var nextCommand = Commands[i].Statement.Replace("[@]", string.Format("@{0}_", i));
                var nextParamCount = Commands[i].ParameterList != null ? Commands[i].ParameterList.Count : 0;

                if (parameters.Count() + nextParamCount > 2000)
                {
                    chunks.Add(new StatementBuilderResult() { Statement = combinedCmd.ToString(), ParameterList = parameters });
                    combinedCmd.Length = 0;
                    parameters = new DynamicParameters();
                }

                combinedCmd.AppendLine(nextCommand);

                if (Commands[i].ParameterList != null)
                {
                    Commands[i].ParameterList.AddDynamicParametersById(parameters, i);
                }
            }
            
            chunks.Add(new StatementBuilderResult() { Statement = combinedCmd.ToString(), ParameterList = parameters });

            return chunks;
        }
    }
}
