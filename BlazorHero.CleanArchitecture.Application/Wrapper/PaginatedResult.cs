using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorHero.CleanArchitecture.Application.Wrapper
{
    public class PaginatedResult<T> : Result
    {
        public PaginatedResult(List<T> data)
        {
            Data = data;
        }
        public List<T> Data { get; set; }

        internal PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, long count = 0, int page = 1, int pageSize = 10)
        {
            Data = data;
            Page = page;
            Succeeded = succeeded;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }
        public static new PaginatedResult<T> Failure(List<string> messages)
        {
            return new PaginatedResult<T>(false, default, messages);
        }

        public static PaginatedResult<T> Success(List<T> data, long count, int page, int pageSize)
        {

            return new PaginatedResult<T>(true, data, null, count, page, pageSize);
        }
        public int Page { get; set; }

        public int TotalPages { get; set; }

        public long TotalCount { get; set; }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}
