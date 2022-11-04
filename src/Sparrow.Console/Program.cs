using Sparrow.Console;
using Sparrow.Video.Shortcuts.Enums;

Startup cli = new AutoshortcutStartup();
CancellationToken token = new();

Environment.SetEnvironmentVariable(EnvironmentVariableNames.Environment, "dev", EnvironmentVariableTarget.User);

/*
 * TODO: 1. Параметры запуска (develop из конфига, production из консоли)
 *       2. Сервис сохранения (включен/выключен)
 *       3. Удаление .restore файлов
 */

await cli.StartAsync(token);