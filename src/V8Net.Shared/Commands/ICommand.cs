namespace V8Net.Shared.Commands
{
    // Local onde armazeno os ICOMMANDS compartilhados pela aplicação
    public interface ICommand
    {
        bool IsValidCommand();
    }
}
