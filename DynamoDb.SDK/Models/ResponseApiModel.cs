using System.Collections.Generic;

namespace DynamoDb.SDK.Models
{
    public class ResponseApiModel<T>
    {
        public ResponseApiModel()
        {
            ErrorMessage = new List<string>();
            ResultList = new List<T>();
        }

        public T Result { get; set; }
        public IEnumerable<T> ResultList { get; set; }
        public bool Status { get; set; }
        public List<string> ErrorMessage { set; get; }
        public string PaginationToken { set; get; }
    }
}
