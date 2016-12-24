// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Plato.Core.Miscellaneous
{
    /// <summary>
    /// Helper class for guard statements, that allow prettier code for guard clauses
    /// </summary>
    /// <example>
    /// Sample usage:
    /// <code>
    /// <![CDATA[
    /// Guard.Against(name.Length == 0).With<ArgumentException>("Name must have at least 1 char length");
    /// Guard.AgainstNull(obj, "obj");
    /// Guard.AgainstNullOrEmpty(name, "name", "Name must have a value");
    /// ]]>
    /// </code>
    /// </example>
    public static class Guard
	{
		/// <summary>
		/// Checks the supplied condition and act with exception if condition resolves to <c>true</c>.
		/// </summary>
		public static Act Against(Boolean assertion)
		{
			return new Act(assertion);
		}

		/// <summary>
		/// Checks the actual value of the supplied <paramref name="selector"/> and throws an
		/// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
		/// </summary>
		/// <param name="selector">The expression which should return argument value.</param>
		/// <exception cref="System.ArgumentNullException">
		/// If the supplied value of <paramref name="selector"/> is <see langword="null"/>.
		/// </exception>
		public static void AgainstNull<T>(Expression<Func<T>> selector)
		{
			var memberSelector = (MemberExpression)selector.Body;
			var constantSelector = (ConstantExpression)memberSelector.Expression;
			var value = ((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);

			if (value != null)
				return;

			var name = ((MemberExpression)selector.Body).Member.Name;
			throw new ArgumentNullException(name);
		}

		/// <summary>
		/// Checks the value of the supplied <paramref name="value"/> and throws an
		/// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
		/// </summary>
		/// <param name="value">The object to check.</param>
		/// <param name="variableName">The variable name.</param>
		/// <exception cref="System.ArgumentNullException">
		/// If the supplied <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public static void AgainstNull<T>(T value, String variableName) where T : class
		{
			AgainstNull(value, variableName, String.Format(CultureInfo.InvariantCulture, "'{0}' cannot be null.", variableName));
		}

		/// <summary>
		/// Checks the value of the supplied <paramref name="value"/> and throws an
		/// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/>.
		/// </summary>
		/// <param name="value">The object to check.</param>
		/// <param name="variableName">The variable name.</param>
		/// <param name="message">The message to include in exception description</param>
		/// <exception cref="System.ArgumentNullException">
		/// If the supplied <paramref name="value"/> is <see langword="null"/>.
		/// </exception>
		public static void AgainstNull<T>(T value, String variableName, String message) where T : class
		{
			if (value == null)
				throw new ArgumentNullException(variableName, message);
		}

		/// <summary>
		/// Checks the actual value of the supplied <paramref name="selector"/> and throws an
		/// <see cref="System.ArgumentNullException"/> if it is <see langword="null"/> or empty.
		/// </summary>
		/// <param name="selector">The expression which should return argument value.</param>
		/// <param name="options">The value trimming options.</param>
		/// <exception cref="System.ArgumentNullException">
		/// If the supplied value of <paramref name="selector"/> is <see langword="null"/> or empty string.
		/// </exception>
		public static void AgainstNullOrEmpty(Expression<Func<String>> selector, TrimOptions options = TrimOptions.DoTrim)
		{
			var memberSelector = (MemberExpression)selector.Body;
			var constantSelector = (ConstantExpression)memberSelector.Expression;
			var value = ((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);
			var name = ((MemberExpression)selector.Body).Member.Name;

			var message = String.Format(
				CultureInfo.InvariantCulture,
				"'{0}' cannot be null or resolve to an empty string", name);

			AgainstNullOrEmpty(value as String, name, message, options);
		}

		/// <summary>
		/// Checks the actual value of the supplied <paramref name="selector"/> and throws an
		/// <see cref="System.ArgumentException"/> if it is empty.
		/// </summary>
		/// <param name="selector">The expression which should return argument value.</param>
		/// <exception cref="System.ArgumentException">
		/// If the supplied value of <paramref name="selector"/> is Guid.Empty.
		/// </exception>
		public static void AgainstEmpty(Expression<Func<Guid>> selector)
		{
			var memberSelector = (MemberExpression)selector.Body;
			var constantSelector = (ConstantExpression)memberSelector.Expression;

			var value = ((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);

			if ((Guid)value != Guid.Empty)
				return;

			var name = ((MemberExpression)selector.Body).Member.Name;

			var message = String.Format(CultureInfo.InvariantCulture, "'{0}' cannot be an empty guid", name);

			throw new ArgumentException(message, name);
		}

		/// <summary>
		/// Checks the value of the supplied string <paramref name="value"/> and throws an
		/// <see cref="System.ArgumentException"/> if it is <see langword="null"/> or empty.
		/// </summary>
		/// <param name="value">The string value to check.</param>
		/// <param name="variableName">The argument name.</param>
		/// <param name="options">The value trimming options.</param>
		/// <exception cref="System.ArgumentException">
		/// If the supplied <paramref name="value"/> is <see langword="null"/> or empty.
		/// </exception>
		public static void AgainstNullOrEmpty(String value, String variableName, TrimOptions options = TrimOptions.DoTrim)
		{
			var message = String.Format(
				CultureInfo.InvariantCulture,
				"'{0}' cannot be null or resolve to an empty string", variableName);

			AgainstNullOrEmpty(value, variableName, message, options);
		}

		/// <summary>
		/// Checks the value of the supplied string <paramref name="value"/> and throws an
		/// <see cref="System.ArgumentException"/> if it is <see langword="null"/> or empty.
		/// </summary>
		/// <param name="value">The string value to check.</param>
		/// <param name="variableName">The variable name.</param>
		/// <param name="message">The message to include in exception description</param>
		/// <param name="options">The value trimming options.</param>
		/// <exception cref="System.ArgumentException">
		/// If the supplied <paramref name="value"/> is <see langword="null"/> or empty.
		/// </exception>
		public static void AgainstNullOrEmpty(String value, String variableName, String message, TrimOptions options)
		{
			if (value != null && options == TrimOptions.DoTrim)
				value = value.Trim();

			if (String.IsNullOrEmpty(value))
				throw new ArgumentException(message, variableName);
		}

		/// <summary>
		/// Represents action taken when assertion is true
		/// </summary>
		public class Act
		{
			readonly Boolean _assertion;

			internal Act(Boolean assertion)
			{
				_assertion = assertion;
			}

			/// <summary>
			/// Will throw an exception of type <typeparamref name="TException"/>
			/// with given arguments, if the "Against" assertion is true
			/// </summary>
			/// <typeparam name="TException">Exception type</typeparam>
			/// <param name="args">Exception arguments </param>
			public void With<TException>(params Object[] args) where TException : Exception
			{
				if (_assertion)
					throw (TException)Activator.CreateInstance(typeof(TException), args);
			}


			/// <summary>
			/// Will throw an exception of type <typeparamref name="TException"/>
			/// with given arguments, if the "Against" assertion is true
			/// </summary>
			/// <typeparam name="TException">Exception type</typeparam>
			public void With<TException>(Func<TException> getExceptionFunc) where TException : Exception
			{
				if (_assertion)
					throw getExceptionFunc();
			}

			/// <summary>
			/// Will throw an exception of type <typeparamref name="TException"/>
			/// with the specified message and format it with given arguments, if the "Against" assertion is true
			/// </summary>
			/// <typeparam name="TException">Exception type</typeparam>
			/// <param name="message">Exception message</param>
			/// <param name="args">Message arguments </param>
			public void With<TException>(String message, params Object[] args) where TException : Exception
			{
				if (_assertion)
					throw (TException)Activator.CreateInstance(typeof(TException), String.Format(message, args));
			}
		}

		public enum TrimOptions
		{
			DoTrim = 0,
			NoTrim = 1
		}
	}
}
