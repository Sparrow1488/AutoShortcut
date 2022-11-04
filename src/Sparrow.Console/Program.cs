using Serilog;
using Sparrow.Console;
using Sparrow.Video.Abstractions.Exceptions;
using Sparrow.Video.Shortcuts.Exceptions;
using System.Security.Cryptography;

Startup cli = new AutoshortcutStartup();
CancellationToken token = new();

// TODO: сохраняем и читаем все сохраненные сериализованные файлы используя шифрование (а если пользакам захочется поиграться????)
//using var aes = Aes.Create();
//aes.GenerateKey();
//var s = aes.KeySize;
//var keyBase64 = Convert.ToBase64String(aes.Key);

try
{
    await cli.StartAsync(token);
}
catch (InvalidEnvironmentVariableException ex)
{
    Log.Error(ex.Message);
}
catch (InputResolutionNameNotRequiredException ex)
{
    Log.Error(ex.Message);
}