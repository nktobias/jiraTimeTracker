namespace Triosoft.JiraTimeTracker.Settings
{
   public interface IProtobufContract<T>
   {
      void InitializeFromDataObject(T dataObject);
      T ToDataObject();
   }
}
