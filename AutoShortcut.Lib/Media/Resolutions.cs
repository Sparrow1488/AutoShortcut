namespace AutoShortcut.Lib.Media;

public record Resolution(int Width, int Height);
public record Resolution8K() : Resolution(7680, 4320);
public record Resolution4K() : Resolution(3840, 2160);
public record Resolution2K() : Resolution(2048, 1080);
public record Resolution1440Px() : Resolution(2560, 1440);
public record Resolution1080Px() : Resolution(1920, 1080);
public record Resolution720Px() : Resolution(1280, 720);
public record Resolution480Px() : Resolution(854, 480);
public record Resolution360Px() : Resolution(640, 360);
public record Resolution240Px() : Resolution(432, 240);