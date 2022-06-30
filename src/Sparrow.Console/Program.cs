using Sparrow.Console;

var cli = new Startup();
var token = new CancellationToken();

await cli.OnStart(token);