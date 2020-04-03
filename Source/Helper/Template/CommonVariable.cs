namespace Captain.Application {
  /// <summary>
  ///   Contains common locale-independent variables
  /// </summary>
  internal enum CommonVariable {
    #region Date/time

    Year = 0, // 2036
    ShortYear = 1, // 36

    Month = 2, // 04
    Day = 3, // 30

    Hour = 4, // 23
    Minute = 5, // 59
    Second = 6, // 30

    #endregion

    #region File

    Type = 7, // Screenshot, Recording
    Extension = 8, // .png
    HomeDirectory = 9, // C:\Users\sanlyx
    PicturesDirectory = 10, // C:\Users\sanlyx\Pictures

    #endregion
  }
}