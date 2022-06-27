﻿namespace Sparrow.Video.Shortcuts
{
    public class Plan
    {
        /*
         * Что нужно для создания нарезки?
         * 1. Получить видео файлы +
         *      1.1. Обработать их (получить анализ)
         * 2. Произвести над ними обработку (перекодирование под один формат)
         *      2.1. Знать во что перекодировать + 
         *      2.2. Запустить процесс ffmpeg и начать кодирование + 
         *      2.3. Накладывать шум, если нет аудио +
         *      2.4. Изменять формат (разрешение) +
         * 3. Готовые файлы запихнуть в Pipipline
         *      3.1. Но сначала создать его через енгине +
         *      3.2. Черех опции указать сохранять или нет -
         *      3.3. В пиплине применить все правила нарезки +
         *      3.4. Запустить процессор, который правильно отрендерит правила
         *      3.5. Итоговым проетом нарезки будет IShortcutScript
         *      3.6. (Условие) по проекту можно будет восстановить файлы
         */

        /*
         * Инструменты:
         * 1. Clean Code
         * 2. DI
         * 3. FFmpeg (process)
         * 4. ShortcutEngine - in process
         */
    }
}