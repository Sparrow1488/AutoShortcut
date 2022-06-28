using Sparrow.Console;

var cli = new CLI();
var token = new CancellationToken();

await cli.OnStart(token);