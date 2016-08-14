namespace Chess.Models.Base
{
    /// <summary>
    /// Simple enum for Black/White color representation in chess. There's also
    /// Light/Dark, as they are used interchangeably.
    /// </summary>
    public enum ChessColor
    {
        LIGHT,
        DARK,

        WHITE = LIGHT,
        BLACK = DARK,

    }
}
