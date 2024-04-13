using CrudOperationNetCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CrudOperationNetCore.Controllers
{
    public class JsonPatchDocument<T>
    {
        internal void ApplyTo(Brand brand, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}