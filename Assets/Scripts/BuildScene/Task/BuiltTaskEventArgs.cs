using System;

public class BuiltTaskEventArgs : EventArgs
{
   public Task Task { get; set; }
   public Planet planet { get; set; }
   public Task constantTask { get; set; }
   
   public BuiltTaskEventArgs(Task Task, Planet planet, Task constantTask) { this.Task = Task; this.planet = planet; this.constantTask = constantTask; }
}
