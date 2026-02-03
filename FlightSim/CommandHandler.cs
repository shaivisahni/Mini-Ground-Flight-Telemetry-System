public static class CommandHandler
{
    public static Ack Handle(Command cmd, Telemetry telemetry)
    {
        if (cmd.Name == "SET_MODE")
        {
            telemetry.Mode = cmd.Argument;

            return new Ack
            {
                Command = cmd.Name,
                Status = "OK",
                Message = $"Mode set to {telemetry.Mode}"
            };
        }

        return new Ack
        {
            Command = cmd.Name,
            Status = "ERROR",
            Message = "Unknown command"
        };
    }
}
