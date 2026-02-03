using Xunit;

public class CommandHandlerTests
{
    [Fact]
    public void SetMode_ChangesTelemetryMode_AndReturnsOkAck()
    {
        // Arrange (set up)
        var telemetry = new Telemetry { Mode = "NOMINAL" };
        var cmd = new Command { Name = "SET_MODE", Argument = "SAFE" };

        // Act (do the thing)
        var ack = CommandHandler.Handle(cmd, telemetry);

        // Assert (check result)
        Assert.Equal("SAFE", telemetry.Mode);
        Assert.Equal("OK", ack.Status);
        Assert.Equal("SET_MODE", ack.Command);
    }
}
