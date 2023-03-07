using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogV6.ViewModels
{
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors.AddRange(errors);
        }

        public ResultViewModel(T data, string error)
        {
            Data = data;
            Errors.Add(error);
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }

        public ResultViewModel(List<string> errors)
        {
            Errors.AddRange(errors);
        }

        public ResultViewModel(string error)
        {
            Errors.Add(error);
        }

        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new(); // A partir do C# 10 é possível instanciar aqui não precisando do construtor
    }
}