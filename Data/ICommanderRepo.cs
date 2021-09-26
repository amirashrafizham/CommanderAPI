using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface ICommanderRepo
    {
        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int id);
        void CreateCommand(Command cmd);
        bool SaveChanges();
        void DeleteCommand(Command cmd);
        void UpdateCommand(Command cmd);
    }
}