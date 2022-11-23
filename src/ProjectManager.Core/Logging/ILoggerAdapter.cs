using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Core.Logging;
public interface ILoggerAdapter<TType>
{
  void LogInformation(string? message, params object?[] args);

  void LogError(Exception exception, string message, params object?[] args);
}
