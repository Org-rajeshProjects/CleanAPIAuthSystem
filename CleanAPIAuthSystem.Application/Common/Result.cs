using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPIAuthSystem.Application.Common
{
    /// <summary>
    /// Result Pattern - Represents success or failure without exceptions
    /// Theory: Railway Oriented Programming
    /// Instead of throwing exceptions for expected errors (user not found, etc.)
    /// we return a Result object that contains either success data or error info
    /// Benefits:
    /// 1. More explicit - caller knows function can fail
    /// 2. Better performance - no exception overhead
    /// 3. Easier to test - no try/catch needed
    /// 4. Functional programming style
    /// </summary>
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? Error { get; }
        public string? ErrorCode { get; }

        private Result(bool isSuccess, T? data, string? error, string? errorCode)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Factory method for success result
        /// Theory: Static factory methods provide clear intent
        /// Better than constructors for creating different types of results
        /// </summary>
        public static Result<T> Success(T data) => new(true, data, null, null);

        /// <summary>
        /// Factory method for failure result
        /// </summary>
        public static Result<T> Failure(string error, string errorCode = "ERROR")
            => new(false, default, error, errorCode);
    }

    /// <summary>
    /// Result without data - for operations that don't return anything
    /// Example: DeleteUser() just needs to indicate success/failure
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public string? ErrorCode { get; }

        private Result(bool isSuccess, string? error, string? errorCode)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorCode = errorCode;
        }

        public static Result Success() => new(true, null, null);
        public static Result Failure(string error, string errorCode = "ERROR")
            => new(false, error, errorCode);
    }
}
