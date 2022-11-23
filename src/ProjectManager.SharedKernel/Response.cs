using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.SharedKernel;
public class Response<T>
{
  public T Data { get; set; }
  public IEnumerable<Error> Errors { get; set; }
  public bool IsSuccess => Errors == null || !Errors.Any();
}
